using UnityEngine;
using Unity.Netcode;

namespace VintageBeef.World
{
    /// <summary>
    /// Represents a claimable housing area of 100x100 chunks
    /// Players can claim these areas to build their homes
    /// </summary>
    public class HousingArea : MonoBehaviour
    {
        [Header("Housing Area Settings")]
        [SerializeField] private Vector2Int areaPosition; // Position in area grid
        [SerializeField] private int areaSize = 100; // Size in chunks (100x100)
        [SerializeField] private float interactionRadius = 5f;

        [Header("Ownership")]
        [SerializeField] private string ownerName = "";
        [SerializeField] private bool isClaimed = false;

        [Header("Visual Settings")]
        [SerializeField] private Color unclaimedColor = new Color(0.5f, 0.5f, 1f, 0.3f);
        [SerializeField] private Color claimedColor = new Color(0f, 1f, 0f, 0.3f);
        [SerializeField] private Color playerOwnedColor = new Color(1f, 0.8f, 0f, 0.3f);

        private GameObject boundaryMarker;
        private bool showingPrompt = false;

        public Vector2Int AreaPosition => areaPosition;
        public int AreaSize => areaSize;
        public bool IsClaimed => isClaimed;
        public string OwnerName => ownerName;

        public void Initialize(Vector2Int position, int chunkSize)
        {
            areaPosition = position;
            CreateBoundaryMarker(chunkSize);
        }

        private void CreateBoundaryMarker(int chunkSize)
        {
            // Create a visual marker at the center of the housing area
            boundaryMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            boundaryMarker.name = "HousingAreaMarker";
            boundaryMarker.transform.SetParent(transform);
            
            // Position at center of the area, slightly above ground
            float centerOffset = (areaSize * chunkSize) / 2f;
            boundaryMarker.transform.localPosition = new Vector3(centerOffset, 5f, centerOffset);
            boundaryMarker.transform.localScale = new Vector3(5f, 10f, 5f);

            // Set color based on claim status
            Renderer renderer = boundaryMarker.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.SetFloat("_Glossiness", 0.3f);
            mat.color = isClaimed ? claimedColor : unclaimedColor;
            
            // Make it slightly transparent
            mat.SetFloat("_Mode", 3); // Transparent mode
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            // Create boundary edges for better visibility
            CreateBoundaryEdges(chunkSize);
        }

        private void CreateBoundaryEdges(int chunkSize)
        {
            GameObject edges = new GameObject("BoundaryEdges");
            edges.transform.SetParent(transform);
            edges.transform.localPosition = Vector3.zero;

            float size = areaSize * chunkSize;
            float height = 2f;

            // Create four corner markers
            CreateCornerMarker(edges.transform, new Vector3(0, height, 0));
            CreateCornerMarker(edges.transform, new Vector3(size, height, 0));
            CreateCornerMarker(edges.transform, new Vector3(0, height, size));
            CreateCornerMarker(edges.transform, new Vector3(size, height, size));
        }

        private void CreateCornerMarker(Transform parent, Vector3 position)
        {
            GameObject corner = GameObject.CreatePrimitive(PrimitiveType.Cube);
            corner.transform.SetParent(parent);
            corner.transform.localPosition = position;
            corner.transform.localScale = new Vector3(2f, 4f, 2f);

            Renderer renderer = corner.GetComponent<Renderer>();
            Material mat = renderer.material;
            mat.color = isClaimed ? claimedColor : unclaimedColor;
            mat.SetFloat("_Glossiness", 0.5f);
        }

        private void Update()
        {
            // Check for nearby players
            if (!isClaimed)
            {
                CheckForClaimingPlayers();
            }
        }

        private void CheckForClaimingPlayers()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            bool playerNearby = false;
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(boundaryMarker.transform.position, player.transform.position);
                
                if (distance <= interactionRadius)
                {
                    playerNearby = true;
                    
                    if (!showingPrompt)
                    {
                        ShowClaimPrompt(player);
                        showingPrompt = true;
                    }

                    // Check for claim input
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        ClaimArea(player);
                    }
                    break;
                }
            }

            if (!playerNearby && showingPrompt)
            {
                showingPrompt = false;
            }
        }

        private void ShowClaimPrompt(GameObject player)
        {
            Debug.Log($"[Housing Area] Press 'C' to claim this housing area (100x100 chunks)");
        }

        public void ClaimArea(GameObject player)
        {
            if (isClaimed)
            {
                Debug.Log($"[Housing Area] This area is already claimed by {ownerName}");
                return;
            }

            // Get player name
            string playerName = "Player";
            PlayerData playerData = PlayerData.Instance;
            if (playerData != null)
            {
                playerName = playerData.PlayerName;
            }

            isClaimed = true;
            ownerName = playerName;

            Debug.Log($"[Housing Area] {playerName} claimed housing area at position {areaPosition}!");

            // Update visual appearance
            UpdateVisuals(true);

            // Notify housing manager
            HousingManager manager = HousingManager.Instance;
            if (manager != null)
            {
                manager.OnAreaClaimed(this, playerName);
            }
        }

        public void SetClaimed(string owner)
        {
            isClaimed = true;
            ownerName = owner;
            UpdateVisuals(false);
        }

        private void UpdateVisuals(bool isPlayerOwned)
        {
            if (boundaryMarker != null)
            {
                Renderer renderer = boundaryMarker.GetComponent<Renderer>();
                renderer.material.color = isPlayerOwned ? playerOwnedColor : claimedColor;
            }

            // Update corner markers
            Transform edges = transform.Find("BoundaryEdges");
            if (edges != null)
            {
                foreach (Transform corner in edges)
                {
                    Renderer renderer = corner.GetComponent<Renderer>();
                    renderer.material.color = isPlayerOwned ? playerOwnedColor : claimedColor;
                }
            }
        }

        public bool IsOwnedByPlayer(string playerName)
        {
            return isClaimed && ownerName == playerName;
        }

        private void OnDrawGizmos()
        {
            // Draw the housing area boundaries in the editor
            Gizmos.color = isClaimed ? Color.green : Color.blue;
            
            // This would need the chunk size to be accurate, using estimated value
            float estimatedSize = areaSize * 20f; // Assuming 20 as default chunk size
            Vector3 center = transform.position + new Vector3(estimatedSize / 2f, 0, estimatedSize / 2f);
            Gizmos.DrawWireCube(center, new Vector3(estimatedSize, 10f, estimatedSize));
        }

        private void OnDrawGizmosSelected()
        {
            // Draw interaction radius
            if (boundaryMarker != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(boundaryMarker.transform.position, interactionRadius);
            }
        }
    }
}
