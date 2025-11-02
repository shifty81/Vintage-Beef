using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VintageBeef.Editor
{
    /// <summary>
    /// Helper script to set up the terrain system in GameWorld scene
    /// Provides a menu item to automatically configure the terrain
    /// </summary>
    public class TerrainSetupHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("Vintage Beef/Setup Terrain System")]
        public static void SetupTerrainSystem()
        {
            // Check if we're in the GameWorld scene
            UnityEngine.SceneManagement.Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (currentScene.name != "GameWorld")
            {
                EditorUtility.DisplayDialog("Wrong Scene", 
                    "Please open the GameWorld scene before setting up the terrain system.", 
                    "OK");
                return;
            }

            // Check if TerrainSystem already exists
            GameObject terrainSystem = GameObject.Find("TerrainSystem");
            if (terrainSystem == null)
            {
                // Create new TerrainSystem GameObject
                terrainSystem = new GameObject("TerrainSystem");
                terrainSystem.transform.position = Vector3.zero;
                Debug.Log("[TerrainSetup] Created TerrainSystem GameObject");
            }

            // Add TerrainManager if not present
            TerrainManager terrainManager = terrainSystem.GetComponent<TerrainManager>();
            if (terrainManager == null)
            {
                terrainManager = terrainSystem.AddComponent<TerrainManager>();
                Debug.Log("[TerrainSetup] Added TerrainManager component");
            }

            // Disable any existing world generators on other objects
            ProceduralWorldGenerator[] proceduralGens = FindObjectsOfType<ProceduralWorldGenerator>();
            foreach (var gen in proceduralGens)
            {
                if (gen.gameObject != terrainSystem)
                {
                    gen.enabled = false;
                    Debug.Log($"[TerrainSetup] Disabled ProceduralWorldGenerator on {gen.gameObject.name}");
                }
            }

            SimpleWorldGenerator[] simpleGens = FindObjectsOfType<SimpleWorldGenerator>();
            foreach (var gen in simpleGens)
            {
                if (gen.gameObject != terrainSystem)
                {
                    gen.enabled = false;
                    Debug.Log($"[TerrainSetup] Disabled SimpleWorldGenerator on {gen.gameObject.name}");
                }
            }

            // Mark scene as dirty so changes are saved
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(currentScene);

            EditorUtility.DisplayDialog("Terrain Setup Complete", 
                "TerrainSystem has been set up successfully!\n\n" +
                "The TerrainManager will automatically add the appropriate world generator when you press Play.\n\n" +
                "Configure settings in the Inspector:\n" +
                "- Use Procedural Terrain: Toggle for varied vs flat terrain\n" +
                "- Spawn Point: Player spawn location", 
                "OK");
        }

        [MenuItem("Vintage Beef/Documentation/Open Terrain System Docs")]
        public static void OpenTerrainDocs()
        {
            string path = Application.dataPath + "/../TERRAIN_SYSTEM.md";
            if (System.IO.File.Exists(path))
            {
                Application.OpenURL("file://" + path);
            }
            else
            {
                EditorUtility.DisplayDialog("File Not Found", 
                    "TERRAIN_SYSTEM.md not found in project root.", 
                    "OK");
            }
        }
#endif
    }
}
