using UnityEngine;
using System.Collections.Generic;
using VintageBeef.World;

namespace VintageBeef.Voxel
{
    /// <summary>
    /// Generates voxel terrain procedurally using noise functions
    /// Supports caves, overhangs, and multiple biomes
    /// </summary>
    public class VoxelWorldGenerator : MonoBehaviour
    {
        [Header("World Settings")]
        [SerializeField] private int chunkSize = 16;
        [SerializeField] private int worldHeightInChunks = 8;
        [SerializeField] private int renderDistanceInChunks = 4;
        [SerializeField] private int seed = 12345;

        [Header("Terrain Generation")]
        [SerializeField] private float surfaceNoiseScale = 0.03f;
        [SerializeField] private float surfaceAmplitude = 40f;
        [SerializeField] private float surfaceOffset = 32f;

        [Header("Cave Generation")]
        [SerializeField] private bool generateCaves = true;
        [SerializeField] private float caveNoiseScale = 0.05f;
        [SerializeField] private float caveThreshold = 0.6f;

        [Header("Biome Settings")]
        [SerializeField] private float biomeNoiseScale = 0.01f;

        private Dictionary<Vector3Int, VoxelChunk> chunks = new Dictionary<Vector3Int, VoxelChunk>();
        private Transform playerTransform;
        private System.Random rng;
        private Vector3Int lastPlayerChunk = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

        private void Start()
        {
            rng = new System.Random(seed);
            
            // Find player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }

            // Generate initial chunks
            GenerateInitialChunks();
        }

        private void Update()
        {
            if (playerTransform != null)
            {
                Vector3Int currentPlayerChunk = WorldToChunkPosition(playerTransform.position);
                
                // Check if player moved to new chunk
                if (currentPlayerChunk != lastPlayerChunk)
                {
                    UpdateChunks(currentPlayerChunk);
                    lastPlayerChunk = currentPlayerChunk;
                }
            }
        }

        /// <summary>
        /// Generate initial chunks around spawn point
        /// </summary>
        private void GenerateInitialChunks()
        {
            Vector3Int centerChunk = Vector3Int.zero;
            
            for (int x = -renderDistanceInChunks; x <= renderDistanceInChunks; x++)
            {
                for (int z = -renderDistanceInChunks; z <= renderDistanceInChunks; z++)
                {
                    for (int y = 0; y < worldHeightInChunks; y++)
                    {
                        Vector3Int chunkPos = new Vector3Int(centerChunk.x + x, y, centerChunk.z + z);
                        GenerateChunk(chunkPos);
                    }
                }
            }
        }

        /// <summary>
        /// Update chunks based on player position
        /// </summary>
        private void UpdateChunks(Vector3Int playerChunkPos)
        {
            // Generate new chunks in range
            for (int x = -renderDistanceInChunks; x <= renderDistanceInChunks; x++)
            {
                for (int z = -renderDistanceInChunks; z <= renderDistanceInChunks; z++)
                {
                    for (int y = 0; y < worldHeightInChunks; y++)
                    {
                        Vector3Int chunkPos = new Vector3Int(playerChunkPos.x + x, y, playerChunkPos.z + z);
                        
                        if (!chunks.ContainsKey(chunkPos))
                        {
                            GenerateChunk(chunkPos);
                        }
                    }
                }
            }

            // Remove chunks out of range
            List<Vector3Int> chunksToRemove = new List<Vector3Int>();
            foreach (var kvp in chunks)
            {
                Vector3Int chunkPos = kvp.Key;
                float distance = Vector3.Distance(
                    new Vector3(chunkPos.x, 0, chunkPos.z), 
                    new Vector3(playerChunkPos.x, 0, playerChunkPos.z)
                );

                if (distance > renderDistanceInChunks + 2)
                {
                    chunksToRemove.Add(chunkPos);
                }
            }

            foreach (var chunkPos in chunksToRemove)
            {
                if (chunks.ContainsKey(chunkPos))
                {
                    Destroy(chunks[chunkPos].gameObject);
                    chunks.Remove(chunkPos);
                }
            }
        }

        /// <summary>
        /// Generate a single chunk at the specified position
        /// </summary>
        private void GenerateChunk(Vector3Int chunkPos)
        {
            GameObject chunkObj = new GameObject($"Chunk_{chunkPos.x}_{chunkPos.y}_{chunkPos.z}");
            chunkObj.transform.SetParent(transform);
            chunkObj.transform.position = ChunkToWorldPosition(chunkPos);

            VoxelChunk chunk = chunkObj.AddComponent<VoxelChunk>();
            chunk.Initialize(chunkPos, chunkSize);

            // Generate voxel data for chunk
            Voxel[,,] voxelData = GenerateVoxelData(chunkPos);
            chunk.FillVoxelData(voxelData);
            chunk.GenerateMesh();

            chunks[chunkPos] = chunk;
        }

        /// <summary>
        /// Generate voxel data for a chunk using noise functions
        /// </summary>
        private Voxel[,,] GenerateVoxelData(Vector3Int chunkPos)
        {
            Voxel[,,] voxels = new Voxel[chunkSize, chunkSize, chunkSize];
            Vector3 chunkWorldPos = ChunkToWorldPosition(chunkPos);

            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    float worldX = chunkWorldPos.x + x;
                    float worldZ = chunkWorldPos.z + z;

                    // Calculate surface height using Perlin noise
                    float surfaceHeight = GetSurfaceHeight(worldX, worldZ);
                    
                    // Get biome for this position
                    BiomeType biome = GetBiomeType(worldX, worldZ);

                    for (int y = 0; y < chunkSize; y++)
                    {
                        float worldY = chunkWorldPos.y + y;

                        // Determine voxel type based on height and biome
                        VoxelType voxelType = DetermineVoxelType(worldX, worldY, worldZ, surfaceHeight, biome);
                        
                        // Check for caves if enabled
                        if (generateCaves && worldY < surfaceHeight && voxelType != VoxelType.Air)
                        {
                            if (IsCave(worldX, worldY, worldZ))
                            {
                                voxelType = VoxelType.Air;
                            }
                        }

                        voxels[x, y, z] = new Voxel(voxelType, voxelType == VoxelType.Air ? (byte)0 : (byte)255);
                    }
                }
            }

            return voxels;
        }

        /// <summary>
        /// Calculate surface height using multi-octave Perlin noise
        /// </summary>
        private float GetSurfaceHeight(float x, float z)
        {
            float height = 0f;
            float amplitude = surfaceAmplitude;
            float frequency = surfaceNoiseScale;
            
            // Multiple octaves for varied terrain
            for (int octave = 0; octave < 3; octave++)
            {
                height += Mathf.PerlinNoise(x * frequency + seed, z * frequency + seed) * amplitude;
                amplitude *= 0.5f;
                frequency *= 2f;
            }

            return height + surfaceOffset;
        }

        /// <summary>
        /// Determine biome type based on position
        /// </summary>
        private BiomeType GetBiomeType(float x, float z)
        {
            float biomeNoise = Mathf.PerlinNoise(x * biomeNoiseScale + seed, z * biomeNoiseScale + seed);
            
            if (biomeNoise < 0.3f)
                return BiomeType.Desert;
            else if (biomeNoise < 0.5f)
                return BiomeType.Plains;
            else if (biomeNoise < 0.7f)
                return BiomeType.Forest;
            else
                return BiomeType.Mountains;
        }

        /// <summary>
        /// Determine voxel type based on position and biome
        /// </summary>
        private VoxelType DetermineVoxelType(float x, float y, float z, float surfaceHeight, BiomeType biome)
        {
            // Above surface = air
            if (y > surfaceHeight)
                return VoxelType.Air;

            // At surface level
            if (y >= surfaceHeight - 1f)
            {
                switch (biome)
                {
                    case BiomeType.Desert:
                        return VoxelType.Sand;
                    case BiomeType.Mountains:
                        return y > surfaceHeight - 0.5f ? VoxelType.Stone : VoxelType.Stone;
                    case BiomeType.Forest:
                    case BiomeType.Plains:
                    default:
                        return VoxelType.Grass;
                }
            }

            // Subsurface layers
            float depthBelowSurface = surfaceHeight - y;

            // Top layer (dirt/sand)
            if (depthBelowSurface < 4f)
            {
                return biome == BiomeType.Desert ? VoxelType.Sand : VoxelType.Dirt;
            }

            // Stone layer with occasional ore
            float oreNoise = Mathf.PerlinNoise(x * 0.1f, z * 0.1f + y * 0.1f);
            if (oreNoise > 0.85f && y < 40f)
            {
                return y < 25f ? VoxelType.Ore_Iron : VoxelType.Ore_Copper;
            }

            return VoxelType.Stone;
        }

        /// <summary>
        /// Check if position should be a cave using 3D noise
        /// </summary>
        private bool IsCave(float x, float y, float z)
        {
            float caveNoise = Mathf.PerlinNoise(x * caveNoiseScale + seed, z * caveNoiseScale + seed) +
                             Mathf.PerlinNoise(y * caveNoiseScale + seed, x * caveNoiseScale + seed) * 0.5f;
            
            return caveNoise > caveThreshold;
        }

        /// <summary>
        /// Convert world position to chunk position
        /// </summary>
        private Vector3Int WorldToChunkPosition(Vector3 worldPos)
        {
            return new Vector3Int(
                Mathf.FloorToInt(worldPos.x / chunkSize),
                Mathf.FloorToInt(worldPos.y / chunkSize),
                Mathf.FloorToInt(worldPos.z / chunkSize)
            );
        }

        /// <summary>
        /// Convert chunk position to world position
        /// </summary>
        private Vector3 ChunkToWorldPosition(Vector3Int chunkPos)
        {
            return new Vector3(
                chunkPos.x * chunkSize,
                chunkPos.y * chunkSize,
                chunkPos.z * chunkSize
            );
        }

        /// <summary>
        /// Modify a voxel in the world (terraforming)
        /// </summary>
        public void SetVoxel(Vector3 worldPos, VoxelType type)
        {
            Vector3Int chunkPos = WorldToChunkPosition(worldPos);
            
            if (chunks.ContainsKey(chunkPos))
            {
                VoxelChunk chunk = chunks[chunkPos];
                
                // Convert world position to local chunk position
                Vector3 localPos = worldPos - ChunkToWorldPosition(chunkPos);
                int x = Mathf.FloorToInt(localPos.x);
                int y = Mathf.FloorToInt(localPos.y);
                int z = Mathf.FloorToInt(localPos.z);

                chunk.SetVoxel(x, y, z, new Voxel(type, type == VoxelType.Air ? (byte)0 : (byte)255));
                chunk.GenerateMesh();
            }
        }

        /// <summary>
        /// Get voxel at world position
        /// </summary>
        public Voxel GetVoxel(Vector3 worldPos)
        {
            Vector3Int chunkPos = WorldToChunkPosition(worldPos);
            
            if (chunks.ContainsKey(chunkPos))
            {
                VoxelChunk chunk = chunks[chunkPos];
                Vector3 localPos = worldPos - ChunkToWorldPosition(chunkPos);
                int x = Mathf.FloorToInt(localPos.x);
                int y = Mathf.FloorToInt(localPos.y);
                int z = Mathf.FloorToInt(localPos.z);

                return chunk.GetVoxel(x, y, z);
            }

            return new Voxel(VoxelType.Air, 0);
        }

        // Note: BiomeType is defined in VintageBeef.World namespace (ProceduralWorldGenerator.cs)
    }
}
