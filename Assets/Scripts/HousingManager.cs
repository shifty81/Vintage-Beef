using UnityEngine;
using System.Collections.Generic;

namespace VintageBeef.World
{
    /// <summary>
    /// Manages all housing areas in the world
    /// Tracks claimed areas and coordinates with world generation
    /// </summary>
    public class HousingManager : MonoBehaviour
    {
        public static HousingManager Instance { get; private set; }

        [Header("Housing Settings")]
        [SerializeField] private int housingAreaSize = 100; // 100x100 chunks per housing area
        [SerializeField] private int numberOfHousingAreas = 4; // Total housing areas to create
        [SerializeField] private float housingAreaSpacing = 50f; // Spacing between areas in world units

        [Header("Housing Area Layout")]
        [SerializeField] private bool arrangeInGrid = true; // Arrange areas in a grid pattern
        [SerializeField] private int gridColumns = 2; // Number of columns in grid layout

        private Dictionary<Vector2Int, HousingArea> housingAreas = new Dictionary<Vector2Int, HousingArea>();
        private List<HousingArea> allAreas = new List<HousingArea>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Creates housing areas in the world
        /// Should be called by the world generator after terrain generation
        /// </summary>
        public void GenerateHousingAreas(int chunkSize, int worldSize)
        {
            Debug.Log($"[Housing Manager] Generating {numberOfHousingAreas} housing areas (each {housingAreaSize}x{housingAreaSize} chunks)...");

            if (arrangeInGrid)
            {
                GenerateGridLayout(chunkSize, worldSize);
            }
            else
            {
                GenerateScatteredLayout(chunkSize, worldSize);
            }

            Debug.Log($"[Housing Manager] Generated {allAreas.Count} housing areas.");
        }

        private void GenerateGridLayout(int chunkSize, int worldSize)
        {
            int rows = Mathf.CeilToInt((float)numberOfHousingAreas / gridColumns);
            float areaWorldSize = housingAreaSize * chunkSize;
            
            // Calculate starting position to center the grid
            float totalWidth = (gridColumns * areaWorldSize) + ((gridColumns - 1) * housingAreaSpacing);
            float totalHeight = (rows * areaWorldSize) + ((rows - 1) * housingAreaSpacing);
            float startX = -totalWidth / 2f;
            float startZ = -totalHeight / 2f;

            int areasCreated = 0;
            for (int row = 0; row < rows && areasCreated < numberOfHousingAreas; row++)
            {
                for (int col = 0; col < gridColumns && areasCreated < numberOfHousingAreas; col++)
                {
                    Vector2Int gridPos = new Vector2Int(col, row);
                    
                    float x = startX + (col * (areaWorldSize + housingAreaSpacing));
                    float z = startZ + (row * (areaWorldSize + housingAreaSpacing));
                    
                    Vector3 worldPosition = new Vector3(x, 0, z);
                    CreateHousingArea(gridPos, worldPosition, chunkSize);
                    
                    areasCreated++;
                }
            }
        }

        private void GenerateScatteredLayout(int chunkSize, int worldSize)
        {
            float areaWorldSize = housingAreaSize * chunkSize;
            float halfWorld = worldSize / 2f;
            
            System.Random rng = new System.Random(12345); // Use consistent seed for reproducibility

            for (int i = 0; i < numberOfHousingAreas; i++)
            {
                // Generate random position, avoiding world edges
                float margin = areaWorldSize;
                float x = (float)(rng.NextDouble() * (worldSize - 2 * margin) - halfWorld + margin);
                float z = (float)(rng.NextDouble() * (worldSize - 2 * margin) - halfWorld + margin);

                Vector2Int gridPos = new Vector2Int(i, 0);
                Vector3 worldPosition = new Vector3(x, 0, z);
                
                CreateHousingArea(gridPos, worldPosition, chunkSize);
            }
        }

        private void CreateHousingArea(Vector2Int gridPosition, Vector3 worldPosition, int chunkSize)
        {
            GameObject areaObj = new GameObject($"HousingArea_{gridPosition.x}_{gridPosition.y}");
            areaObj.transform.SetParent(transform);
            areaObj.transform.position = worldPosition;

            HousingArea area = areaObj.AddComponent<HousingArea>();
            area.Initialize(gridPosition, chunkSize);

            housingAreas[gridPosition] = area;
            allAreas.Add(area);

            Debug.Log($"[Housing Manager] Created housing area at grid position {gridPosition}, world position {worldPosition}");
        }

        /// <summary>
        /// Called when a housing area is claimed by a player
        /// </summary>
        public void OnAreaClaimed(HousingArea area, string playerName)
        {
            Debug.Log($"[Housing Manager] Housing area at {area.AreaPosition} claimed by {playerName}");
            
            // You could add additional logic here, such as:
            // - Saving the claim to persistent storage
            // - Network synchronization in multiplayer
            // - Triggering events or achievements
            // - Notifying other players
        }

        /// <summary>
        /// Get housing area at a specific grid position
        /// </summary>
        public HousingArea GetHousingArea(Vector2Int gridPosition)
        {
            if (housingAreas.ContainsKey(gridPosition))
            {
                return housingAreas[gridPosition];
            }
            return null;
        }

        /// <summary>
        /// Get all housing areas
        /// </summary>
        public List<HousingArea> GetAllHousingAreas()
        {
            return allAreas;
        }

        /// <summary>
        /// Get all housing areas claimed by a specific player
        /// </summary>
        public List<HousingArea> GetPlayerHousingAreas(string playerName)
        {
            List<HousingArea> playerAreas = new List<HousingArea>();
            
            foreach (HousingArea area in allAreas)
            {
                if (area.IsOwnedByPlayer(playerName))
                {
                    playerAreas.Add(area);
                }
            }

            return playerAreas;
        }

        /// <summary>
        /// Get the number of unclaimed housing areas
        /// </summary>
        public int GetUnclaimedAreaCount()
        {
            int count = 0;
            foreach (HousingArea area in allAreas)
            {
                if (!area.IsClaimed)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Check if a world position is within any housing area
        /// </summary>
        public bool IsPositionInHousingArea(Vector3 worldPosition, out HousingArea area)
        {
            area = null;
            
            foreach (HousingArea housingArea in allAreas)
            {
                Vector3 areaPos = housingArea.transform.position;
                float areaSize = housingArea.AreaSize * 20f; // Assuming default chunk size
                
                if (worldPosition.x >= areaPos.x && worldPosition.x <= areaPos.x + areaSize &&
                    worldPosition.z >= areaPos.z && worldPosition.z <= areaPos.z + areaSize)
                {
                    area = housingArea;
                    return true;
                }
            }

            return false;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
