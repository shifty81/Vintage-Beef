#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.Netcode;
using Unity.Netcode.Components;
using VintageBeef;
using VintageBeef.Network;
using System.IO;

namespace VintageBeef.Editor
{
    /// <summary>
    /// Helper tool to create prefabs for Vintage Beef
    /// </summary>
    public class PrefabCreationHelper : EditorWindow
    {
        [MenuItem("Vintage Beef/Prefab Creation Helper")]
        public static void ShowWindow()
        {
            GetWindow<PrefabCreationHelper>("Prefab Creation");
        }

        private void OnGUI()
        {
            GUILayout.Label("Vintage Beef Prefab Creation", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "Use these buttons to create required prefabs for the game. " +
                "Prefabs will be saved in Assets/Prefabs/ folder.", 
                MessageType.Info);

            GUILayout.Space(10);

            if (GUILayout.Button("Create NetworkPlayer Prefab", GUILayout.Height(40)))
            {
                CreateNetworkPlayerPrefab();
            }

            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "Resource node prefabs (Trees, Rocks, Plants) should be created " +
                "in the Unity Editor manually. See PREFAB_GUIDE.md for instructions.", 
                MessageType.Info);
        }

        private void CreateNetworkPlayerPrefab()
        {
            Debug.Log("Creating NetworkPlayer prefab...");

            // Ensure Prefabs directory exists
            string prefabPath = "Assets/Prefabs";
            if (!AssetDatabase.IsValidFolder(prefabPath))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }

            // Create player GameObject
            GameObject playerObj = new GameObject("NetworkPlayer");

            // Add CharacterController
            CharacterController charController = playerObj.AddComponent<CharacterController>();
            charController.height = 2f;
            charController.radius = 0.5f;
            charController.center = new Vector3(0, 1, 0);

            // Add NetworkObject
            NetworkObject networkObject = playerObj.AddComponent<NetworkObject>();
            // Note: IsPlayerObject should be set to true in the prefab inspector after creation

            // Add NetworkTransform
            NetworkTransform networkTransform = playerObj.AddComponent<NetworkTransform>();
            // Configure NetworkTransform settings
            networkTransform.SyncPositionX = true;
            networkTransform.SyncPositionY = true;
            networkTransform.SyncPositionZ = true;
            networkTransform.SyncRotAngleX = false;
            networkTransform.SyncRotAngleY = true;
            networkTransform.SyncRotAngleZ = false;

            // Add NetworkPlayer script
            NetworkPlayer networkPlayer = playerObj.AddComponent<NetworkPlayer>();

            // Add PlayerController
            PlayerController playerController = playerObj.AddComponent<PlayerController>();
            playerController.enabled = false; // Will be enabled by NetworkPlayer for owner

            // Add PlayerInventory
            playerObj.AddComponent<PlayerInventory>();

            // Add PlayerInteraction
            playerObj.AddComponent<PlayerInteraction>();

            // Create camera child object
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(playerObj.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.6f, 0);
            cameraObj.transform.localRotation = Quaternion.identity;

            // Add Camera component
            Camera camera = cameraObj.AddComponent<Camera>();
            camera.fieldOfView = 60f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 1000f;

            // Add AudioListener
            cameraObj.AddComponent<AudioListener>();

            // Link camera to PlayerController using public setter
            playerController.SetCameraTransform(cameraObj.transform);

            // Create visual representation (temporary capsule)
            GameObject visualObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            visualObj.name = "PlayerVisual";
            visualObj.transform.SetParent(playerObj.transform);
            visualObj.transform.localPosition = new Vector3(0, 1, 0);
            visualObj.transform.localRotation = Quaternion.identity;
            visualObj.transform.localScale = Vector3.one;
            
            // Remove the collider from visual (CharacterController handles collision)
            DestroyImmediate(visualObj.GetComponent<Collider>());

            // Create and save material as an asset
            string materialPath = prefabPath + "/PlayerVisualMaterial.mat";
            Material playerMaterial = null;
            
            // Check if material already exists
            playerMaterial = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            if (playerMaterial == null)
            {
                playerMaterial = new Material(Shader.Find("Standard"));
                playerMaterial.color = new Color(0.3f, 0.5f, 0.8f); // Blue-ish color
                AssetDatabase.CreateAsset(playerMaterial, materialPath);
                AssetDatabase.SaveAssets();
                Debug.Log($"Created player visual material at {materialPath}");
            }

            // Apply material to visual
            MeshRenderer renderer = visualObj.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = playerMaterial;
            }

            // Save as prefab
            string prefabAssetPath = prefabPath + "/NetworkPlayer.prefab";
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(playerObj, prefabAssetPath);

            // Clean up the scene instance
            DestroyImmediate(playerObj);

            if (prefab != null)
            {
                // Select the prefab in the Project window
                Selection.activeObject = prefab;
                EditorGUIUtility.PingObject(prefab);

                Debug.Log($"NetworkPlayer prefab created successfully at {prefabAssetPath}");
                Debug.Log("IMPORTANT: Select the prefab and set 'Is Player Object' to TRUE in the NetworkObject component!");
                
                EditorUtility.DisplayDialog(
                    "Prefab Created Successfully!",
                    "NetworkPlayer prefab has been created.\n\n" +
                    "NEXT STEPS:\n" +
                    "1. Select the NetworkPlayer prefab in the Project window\n" +
                    "2. In the Inspector, find the NetworkObject component\n" +
                    "3. Check the 'Is Player Object' checkbox\n" +
                    "4. Add the prefab to your NetworkManager's player prefab list\n\n" +
                    "See UNITY_SETUP.md for complete multiplayer setup instructions.",
                    "OK");
            }
            else
            {
                Debug.LogError("Failed to create NetworkPlayer prefab!");
            }
        }
    }
}
#endif
