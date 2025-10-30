using UnityEngine;
using System.Collections.Generic;

namespace VintageBeef.World
{
    /// <summary>
    /// Advanced procedural world generator with biomes and terrain
    /// Generates diverse explorable worlds using Perlin noise
    /// Optimized for Palia-style performance
    /// </summary>
    public class ProceduralWorldGenerator : MonoBehaviour
    {
        [Header("World Settings")]
        [SerializeField] private int worldSize = 200;
        [SerializeField] private int chunkSize = 20;
        [SerializeField] private float heightMultiplier = 10f;
        [SerializeField] private float noiseScale = 0.05f;
        [SerializeField] private int seed = 12345;

        [Header("Biome Settings")]
        [SerializeField] private float biomeScale = 0.02f;
        [SerializeField] private float moistureScale = 0.03f;

        [Header("Resource Settings")]
        [SerializeField] private int treesPerChunk = 5;
        [SerializeField] private int rocksPerChunk = 3;
        [SerializeField] private int plantsPerChunk = 8;

        [Header("Dungeon Settings")]
        [SerializeField] private int numberOfDungeons = 8;

        private System.Random rng;
        private Dictionary<Vector2Int, TerrainChunk> chunks = new Dictionary<Vector2Int, TerrainChunk>();

        private void Start()
        {
            rng = new System.Random(seed);
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            Debug.Log($"Generating procedural world (size: {worldSize}x{worldSize}, seed: {seed})...");

            int chunksPerSide = worldSize / chunkSize;
            int halfChunks = chunksPerSide / 2;

            for (int x = -halfChunks; x < halfChunks; x++)
            {
                for (int z = -halfChunks; z < halfChunks; z++)
                {
                    GenerateChunk(x, z);
                }
            }

            PlaceDungeonEntrances();

            Debug.Log($"World generation complete! Generated {chunks.Count} chunks.");
        }

        private void GenerateChunk(int chunkX, int chunkZ)
        {
            Vector2Int chunkPos = new Vector2Int(chunkX, chunkZ);
            
            GameObject chunkObj = new GameObject($"Chunk_{chunkX}_{chunkZ}");
            chunkObj.transform.SetParent(transform);

            TerrainChunk chunk = new TerrainChunk
            {
                position = chunkPos,
                gameObject = chunkObj
            };

            // Generate terrain mesh for this chunk
            GenerateTerrainMesh(chunk, chunkX * chunkSize, chunkZ * chunkSize);

            // Place resources
            PlaceResourcesInChunk(chunk, chunkX * chunkSize, chunkZ * chunkSize);

            chunks[chunkPos] = chunk;
        }

        private void GenerateTerrainMesh(TerrainChunk chunk, int offsetX, int offsetZ)
        {
            Mesh mesh = new Mesh();
            int resolution = chunkSize + 1;
            Vector3[] vertices = new Vector3[resolution * resolution];
            int[] triangles = new int[chunkSize * chunkSize * 6];
            Vector2[] uvs = new Vector2[vertices.Length];
            Color[] colors = new Color[vertices.Length];

            int vertIndex = 0;
            for (int z = 0; z < resolution; z++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    float worldX = offsetX + x;
                    float worldZ = offsetZ + z;

                    // Generate height using Perlin noise
                    float height = GetTerrainHeight(worldX, worldZ);
                    
                    // Get biome at this position
                    BiomeType biome = GetBiomeAt(worldX, worldZ);
                    
                    vertices[vertIndex] = new Vector3(x, height, z);
                    uvs[vertIndex] = new Vector2(x / (float)chunkSize, z / (float)chunkSize);
                    colors[vertIndex] = GetBiomeColor(biome, height);
                    vertIndex++;
                }
            }

            // Generate triangles
            int triIndex = 0;
            for (int z = 0; z < chunkSize; z++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    int i = z * resolution + x;
                    
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution;
                    triangles[triIndex + 2] = i + 1;
                    
                    triangles[triIndex + 3] = i + 1;
                    triangles[triIndex + 4] = i + resolution;
                    triangles[triIndex + 5] = i + resolution + 1;
                    
                    triIndex += 6;
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.colors = colors;
            mesh.RecalculateNormals();

            MeshFilter meshFilter = chunk.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            MeshRenderer renderer = chunk.gameObject.AddComponent<MeshRenderer>();
            Material mat = new Material(Shader.Find("Standard"));
            mat.SetFloat("_Glossiness", 0.1f); // Low glossiness for Palia-style look
            renderer.material = mat;

            MeshCollider collider = chunk.gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;

            chunk.gameObject.transform.position = new Vector3(offsetX, 0, offsetZ);
        }

        private float GetTerrainHeight(float x, float z)
        {
            // Multi-octave Perlin noise for more interesting terrain
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

        private BiomeType GetBiomeAt(float x, float z)
        {
            // Use Perlin noise for temperature and moisture
            float temperature = Mathf.PerlinNoise(x * biomeScale, z * biomeScale);
            float moisture = Mathf.PerlinNoise(x * moistureScale + 1000, z * moistureScale + 1000);

            // Determine biome based on temperature and moisture
            if (temperature < 0.3f)
            {
                return moisture < 0.4f ? BiomeType.Desert : BiomeType.Plains;
            }
            else if (temperature < 0.6f)
            {
                return moisture < 0.5f ? BiomeType.Plains : BiomeType.Forest;
            }
            else
            {
                return moisture < 0.4f ? BiomeType.Plains : BiomeType.Mountains;
            }
        }

        private Color GetBiomeColor(BiomeType biome, float height)
        {
            // Palia-style stylized colors
            switch (biome)
            {
                case BiomeType.Forest:
                    return Color.Lerp(new Color(0.2f, 0.6f, 0.2f), new Color(0.3f, 0.5f, 0.3f), height / heightMultiplier);
                
                case BiomeType.Plains:
                    return Color.Lerp(new Color(0.5f, 0.7f, 0.3f), new Color(0.6f, 0.75f, 0.4f), height / heightMultiplier);
                
                case BiomeType.Desert:
                    return Color.Lerp(new Color(0.9f, 0.8f, 0.5f), new Color(0.85f, 0.75f, 0.45f), height / heightMultiplier);
                
                case BiomeType.Mountains:
                    return Color.Lerp(new Color(0.5f, 0.5f, 0.5f), new Color(0.7f, 0.7f, 0.7f), height / heightMultiplier);
                
                default:
                    return Color.green;
            }
        }

        private void PlaceResourcesInChunk(TerrainChunk chunk, int offsetX, int offsetZ)
        {
            GameObject resourceParent = new GameObject("Resources");
            resourceParent.transform.SetParent(chunk.gameObject.transform);

            BiomeType biome = GetBiomeAt(offsetX + chunkSize / 2f, offsetZ + chunkSize / 2f);

            // Place trees
            int treeCount = GetResourceCountForBiome(biome, ResourceType.Tree, treesPerChunk);
            for (int i = 0; i < treeCount; i++)
            {
                PlaceResource(resourceParent, ResourceType.Tree, offsetX, offsetZ, biome);
            }

            // Place rocks
            int rockCount = GetResourceCountForBiome(biome, ResourceType.Rock, rocksPerChunk);
            for (int i = 0; i < rockCount; i++)
            {
                PlaceResource(resourceParent, ResourceType.Rock, offsetX, offsetZ, biome);
            }

            // Place plants
            int plantCount = GetResourceCountForBiome(biome, ResourceType.Plant, plantsPerChunk);
            for (int i = 0; i < plantCount; i++)
            {
                PlaceResource(resourceParent, ResourceType.Plant, offsetX, offsetZ, biome);
            }
        }

        private int GetResourceCountForBiome(BiomeType biome, ResourceType resource, int baseCount)
        {
            float multiplier = 1f;
            
            switch (biome)
            {
                case BiomeType.Forest:
                    if (resource == ResourceType.Tree) multiplier = 2f;
                    if (resource == ResourceType.Plant) multiplier = 1.5f;
                    break;
                case BiomeType.Plains:
                    if (resource == ResourceType.Plant) multiplier = 2f;
                    break;
                case BiomeType.Desert:
                    if (resource == ResourceType.Rock) multiplier = 2f;
                    if (resource == ResourceType.Tree) multiplier = 0.2f;
                    break;
                case BiomeType.Mountains:
                    if (resource == ResourceType.Rock) multiplier = 3f;
                    if (resource == ResourceType.Tree) multiplier = 0.5f;
                    break;
            }

            return Mathf.RoundToInt(baseCount * multiplier);
        }

        private void PlaceResource(GameObject parent, ResourceType type, int offsetX, int offsetZ, BiomeType biome)
        {
            float x = offsetX + (float)rng.NextDouble() * chunkSize;
            float z = offsetZ + (float)rng.NextDouble() * chunkSize;
            float y = GetTerrainHeight(x, z);

            Vector3 position = new Vector3(x, y, z);

            GameObject resource = CreateResourceObject(type, biome);
            resource.transform.position = position;
            resource.transform.SetParent(parent.transform);
            resource.transform.rotation = Quaternion.Euler(0, (float)rng.NextDouble() * 360f, 0);

            // Add resource component
            ResourceNode node = resource.AddComponent<ResourceNode>();
            node.resourceType = type;
            node.biome = biome;
        }

        private GameObject CreateResourceObject(ResourceType type, BiomeType biome)
        {
            GameObject resource;
            
            switch (type)
            {
                case ResourceType.Tree:
                    resource = CreateTree(biome);
                    break;
                case ResourceType.Rock:
                    resource = CreateRock(biome);
                    break;
                case ResourceType.Plant:
                    resource = CreatePlant(biome);
                    break;
                default:
                    resource = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
            }

            return resource;
        }

        private GameObject CreateTree(BiomeType biome)
        {
            GameObject tree = new GameObject("Tree");
            
            // Trunk
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Trunk";
            trunk.transform.SetParent(tree.transform);
            trunk.transform.localPosition = new Vector3(0, 1.5f, 0);
            trunk.transform.localScale = new Vector3(0.3f, 1.5f, 0.3f);
            trunk.GetComponent<Renderer>().material.color = new Color(0.4f, 0.25f, 0.1f); // Brown

            // Foliage
            GameObject foliage = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            foliage.name = "Foliage";
            foliage.transform.SetParent(tree.transform);
            foliage.transform.localPosition = new Vector3(0, 3.5f, 0);
            foliage.transform.localScale = new Vector3(2f, 2f, 2f);
            
            // Biome-specific foliage color
            Color foliageColor = biome == BiomeType.Desert ? 
                new Color(0.5f, 0.6f, 0.3f) : // Dry green
                new Color(0.2f, 0.6f, 0.2f);  // Lush green
            foliage.GetComponent<Renderer>().material.color = foliageColor;

            return tree;
        }

        private GameObject CreateRock(BiomeType biome)
        {
            GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            rock.name = "Rock";
            rock.transform.localScale = new Vector3(1f, 0.6f, 1f);
            
            Color rockColor = biome == BiomeType.Desert ?
                new Color(0.7f, 0.6f, 0.4f) : // Sandy
                new Color(0.5f, 0.5f, 0.5f);  // Gray
            rock.GetComponent<Renderer>().material.color = rockColor;

            return rock;
        }

        private GameObject CreatePlant(BiomeType biome)
        {
            GameObject plant = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plant.name = "Plant";
            plant.transform.localScale = new Vector3(0.3f, 0.5f, 0.3f);
            
            Color plantColor = biome == BiomeType.Desert ?
                new Color(0.6f, 0.7f, 0.4f) : // Desert plant
                new Color(0.3f, 0.7f, 0.3f);  // Regular plant
            plant.GetComponent<Renderer>().material.color = plantColor;

            return plant;
        }

        private void PlaceDungeonEntrances()
        {
            for (int i = 0; i < numberOfDungeons; i++)
            {
                Vector3 position = GetRandomWorldPosition();
                
                GameObject entrance = GameObject.CreatePrimitive(PrimitiveType.Cube);
                entrance.transform.position = position;
                entrance.transform.localScale = new Vector3(2f, 3f, 2f);
                entrance.name = $"DungeonEntrance_{i}";
                
                entrance.AddComponent<DungeonEntrance>();
                entrance.GetComponent<Renderer>().material.color = new Color(0.4f, 0.2f, 0.8f); // Purple
            }
        }

        private Vector3 GetRandomWorldPosition()
        {
            float halfSize = worldSize / 2f;
            float x = (float)(rng.NextDouble() * worldSize - halfSize);
            float z = (float)(rng.NextDouble() * worldSize - halfSize);
            float y = GetTerrainHeight(x, z);
            return new Vector3(x, y + 2f, z);
        }
    }

    public enum BiomeType
    {
        Forest,
        Plains,
        Desert,
        Mountains
    }

    public enum ResourceType
    {
        Tree,
        Rock,
        Plant
    }

    public class TerrainChunk
    {
        public Vector2Int position;
        public GameObject gameObject;
    }
}
