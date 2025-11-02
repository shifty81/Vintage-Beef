using UnityEngine;
using VintageBeef.World;
using VintageBeef.Voxel;

namespace VintageBeef
{
    /// <summary>
    /// Manages terrain generation and coordinates with player spawning
    /// Ensures players spawn at proper terrain height
    /// Provides API for terrain modification (future terraforming support)
    /// Supports both heightmap and voxel-based terrain
    /// </summary>
    public class TerrainManager : MonoBehaviour
    {
        public static TerrainManager Instance { get; private set; }

        [Header("Terrain Type")]
        [SerializeField] private TerrainType terrainType = TerrainType.Procedural;
        [SerializeField] private Vector3 spawnPoint = new Vector3(0, 0, 0);

        [Header("References")]
        private ProceduralWorldGenerator proceduralGenerator;
        private SimpleWorldGenerator simpleGenerator;
        private VintageBeef.Voxel.VoxelWorldGenerator voxelGenerator;

        private bool isTerrainReady = false;

        public enum TerrainType
        {
            Simple,      // Flat terrain
            Procedural,  // Heightmap-based procedural terrain
            Voxel        // Voxel-based fully terraformable terrain
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            InitializeTerrain();
        }

        private void InitializeTerrain()
        {
            Debug.Log("[TerrainManager] Initializing terrain system...");

            switch (terrainType)
            {
                case TerrainType.Voxel:
                    InitializeVoxelTerrain();
                    break;
                case TerrainType.Procedural:
                    InitializeProceduralTerrain();
                    break;
                case TerrainType.Simple:
                    InitializeSimpleTerrain();
                    break;
            }

            // Wait a frame for terrain to generate
            Invoke(nameof(MarkTerrainReady), 0.5f);
        }

        private void InitializeVoxelTerrain()
        {
            // Use or create voxel generator
            voxelGenerator = GetComponent<VoxelWorldGenerator>();
            if (voxelGenerator == null)
            {
                voxelGenerator = gameObject.AddComponent<VoxelWorldGenerator>();
            }

            // Disable other generators
            DisableGenerator(proceduralGenerator);
            DisableGenerator(simpleGenerator);

            Debug.Log("[TerrainManager] Voxel terrain initialized");
        }

        private void InitializeProceduralTerrain()
        {
            // Use or create procedural generator
            proceduralGenerator = GetComponent<ProceduralWorldGenerator>();
            if (proceduralGenerator == null)
            {
                proceduralGenerator = gameObject.AddComponent<ProceduralWorldGenerator>();
            }

            // Disable other generators
            DisableGenerator(simpleGenerator);
            DisableGenerator(voxelGenerator);

            Debug.Log("[TerrainManager] Procedural terrain initialized");
        }

        private void InitializeSimpleTerrain()
        {
            // Use simple generator
            simpleGenerator = GetComponent<SimpleWorldGenerator>();
            if (simpleGenerator == null)
            {
                simpleGenerator = gameObject.AddComponent<SimpleWorldGenerator>();
            }

            // Disable other generators
            DisableGenerator(proceduralGenerator);
            DisableGenerator(voxelGenerator);

            Debug.Log("[TerrainManager] Simple terrain initialized");
        }

        private void DisableGenerator(MonoBehaviour generator)
        {
            if (generator != null)
            {
                generator.enabled = false;
            }
        }

        private void MarkTerrainReady()
        {
            isTerrainReady = true;
            Debug.Log("[TerrainManager] Terrain generation complete and ready!");
        }

        /// <summary>
        /// Gets the terrain height at a given world position
        /// </summary>
        public float GetTerrainHeight(float worldX, float worldZ)
        {
            if (terrainType == TerrainType.Voxel && voxelGenerator != null)
            {
                // For voxel terrain, scan downward to find surface
                return GetVoxelSurfaceHeight(worldX, worldZ);
            }
            else if (terrainType == TerrainType.Procedural && proceduralGenerator != null)
            {
                // Access the procedural generator's height function
                return GetProceduralTerrainHeight(worldX, worldZ);
            }
            else
            {
                // Simple flat terrain
                return 0f;
            }
        }

        /// <summary>
        /// Find surface height for voxel terrain
        /// </summary>
        private float GetVoxelSurfaceHeight(float worldX, float worldZ)
        {
            // Start from a high point and scan down
            for (float y = 100f; y >= 0f; y -= 1f)
            {
                Voxel voxel = voxelGenerator.GetVoxel(new Vector3(worldX, y, worldZ));
                if (voxel.IsSolid())
                {
                    return y + 1f; // Return position above solid voxel
                }
            }
            return 0f; // Default if no surface found
        }

        /// <summary>
        /// Calculate terrain height using same algorithm as ProceduralWorldGenerator
        /// </summary>
        private float GetProceduralTerrainHeight(float x, float z)
        {
            // Use same parameters as ProceduralWorldGenerator
            float noiseScale = 0.05f;
            float heightMultiplier = 10f;

            float height = 0f;
            float amplitude = 1f;
            float frequency = noiseScale;

            for (int octave = 0; octave < 3; octave++)
            {
                height += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }

            return height * heightMultiplier;
        }

        /// <summary>
        /// Gets a safe spawn position with correct terrain height
        /// </summary>
        public Vector3 GetSafeSpawnPosition()
        {
            float terrainHeight = GetTerrainHeight(spawnPoint.x, spawnPoint.z);
            return new Vector3(spawnPoint.x, terrainHeight + 2f, spawnPoint.z);
        }

        /// <summary>
        /// Gets a random spawn position within radius
        /// </summary>
        public Vector3 GetRandomSpawnPosition(float radius = 5f)
        {
            Vector2 randomOffset = Random.insideUnitCircle * radius;
            float x = spawnPoint.x + randomOffset.x;
            float z = spawnPoint.z + randomOffset.y;
            float terrainHeight = GetTerrainHeight(x, z);
            return new Vector3(x, terrainHeight + 2f, z);
        }

        /// <summary>
        /// Checks if terrain is ready for spawning players
        /// </summary>
        public bool IsTerrainReady()
        {
            return isTerrainReady;
        }

        /// <summary>
        /// Future: Modify terrain at position (for terraforming)
        /// </summary>
        public void ModifyTerrain(Vector3 position, float radius, float strength)
        {
            // TODO: Implement terrain modification for terraforming
            Debug.Log($"[TerrainManager] Terrain modification requested at {position} (not yet implemented)");
        }

        /// <summary>
        /// Future: Add voxel-like terrain editing capability
        /// </summary>
        public void AddTerrainBlock(Vector3 position)
        {
            // TODO: Implement voxel-style terrain addition
            Debug.Log($"[TerrainManager] Add terrain block at {position} (not yet implemented)");
        }

        /// <summary>
        /// Future: Remove terrain for mining/digging
        /// </summary>
        public void RemoveTerrainBlock(Vector3 position)
        {
            // TODO: Implement voxel-style terrain removal
            Debug.Log($"[TerrainManager] Remove terrain block at {position} (not yet implemented)");
        }
    }
}
