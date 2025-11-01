using UnityEngine;
using System.Collections.Generic;

namespace VintageBeef
{
    /// <summary>
    /// Simple world generator for creating a basic explorable world
    /// Optimized for performance like Palia's art style
    /// </summary>
    public class SimpleWorldGenerator : MonoBehaviour
    {
        [Header("World Settings")]
        [SerializeField] private int worldSize = 100;
        
        [Header("Dungeon Settings")]
        [SerializeField] private int numberOfDungeons = 5;
        [SerializeField] private GameObject dungeonEntrancePrefab;

        [Header("Terrain Settings")]
        [SerializeField] private Material groundMaterial;

        private void Start()
        {
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            Debug.Log("Generating world...");

            // Create simple ground plane
            CreateGround();

            // Place dungeon entrances
            PlaceDungeonEntrances();

            Debug.Log("World generation complete!");
        }

        private void CreateGround()
        {
            // Create a simple plane for the ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(worldSize / 10f, 1f, worldSize / 10f);

            if (groundMaterial != null)
            {
                ground.GetComponent<Renderer>().material = groundMaterial;
            }
            else
            {
                // Default stylized grass-like color (Palia-style)
                Material mat = ground.GetComponent<Renderer>().material;
                mat.color = new Color(0.4f, 0.7f, 0.3f);
            }
        }

        private void PlaceDungeonEntrances()
        {
            for (int i = 0; i < numberOfDungeons; i++)
            {
                Vector3 position = GetRandomWorldPosition();
                
                GameObject entrance;
                if (dungeonEntrancePrefab != null)
                {
                    entrance = Instantiate(dungeonEntrancePrefab, position, Quaternion.identity);
                }
                else
                {
                    // Create simple visual representation
                    entrance = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    entrance.transform.position = position;
                    entrance.transform.localScale = new Vector3(2f, 3f, 2f);
                    entrance.name = $"DungeonEntrance_{i}";
                    
                    // Add dungeon entrance script
                    entrance.AddComponent<DungeonEntrance>();
                    
                    // Make it visually distinct (dark purple for dungeon portal)
                    entrance.GetComponent<Renderer>().material.color = new Color(0.4f, 0.2f, 0.8f);
                }
            }
        }

        private Vector3 GetRandomWorldPosition()
        {
            float halfSize = worldSize / 2f;
            float x = Random.Range(-halfSize + 10f, halfSize - 10f);
            float z = Random.Range(-halfSize + 10f, halfSize - 10f);
            return new Vector3(x, 2f, z); // Slightly above ground
        }
    }
}
