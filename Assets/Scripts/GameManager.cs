using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace VintageBeef
{
    /// <summary>
    /// Main game manager that handles game state and coordination
    /// Supports both single-player and multiplayer modes
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject networkPlayerPrefab;
        [SerializeField] private Vector3 spawnPosition = new Vector3(0, 2, 0);
        [SerializeField] private float spawnRadius = 3f;

        private GameObject currentPlayer;
        private bool isMultiplayerMode = false;

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

        private void Start()
        {
            // Subscribe to scene loaded events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene loaded: {scene.name}");

            // Spawn player in game world
            if (scene.name == "GameWorld")
            {
                // Wait for terrain to be ready before spawning
                StartCoroutine(WaitForTerrainAndSpawn());
            }
        }

        private System.Collections.IEnumerator WaitForTerrainAndSpawn()
        {
            // Wait for TerrainManager to be ready
            while (TerrainManager.Instance == null || !TerrainManager.Instance.IsTerrainReady())
            {
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("[GameManager] Terrain ready, spawning player...");

            // Check if we're in multiplayer mode
            Network.VintageBeefNetworkManager networkManager = Network.VintageBeefNetworkManager.Instance;
            isMultiplayerMode = networkManager != null && 
                               (networkManager.IsHost() || networkManager.IsClient());

            if (isMultiplayerMode)
            {
                SetupNetworkSpawning();
            }
            else
            {
                SpawnPlayer();
            }
        }

        private void SetupNetworkSpawning()
        {
            Network.VintageBeefNetworkManager networkManager = Network.VintageBeefNetworkManager.Instance;
            if (networkManager == null) return;

            Unity.Netcode.NetworkManager netManager = networkManager.GetNetworkManager();
            if (netManager == null) return;

            // Setup network prefab if not already set
            if (networkPlayerPrefab != null && netManager.NetworkConfig.Prefabs.NetworkPrefabsLists.Count == 0)
            {
                // Register network player prefab
                netManager.NetworkConfig.Prefabs.Add(new NetworkPrefab { Prefab = networkPlayerPrefab });
                Debug.Log("Network player prefab registered");
            }

            // If we're the server/host, handle player spawning
            if (networkManager.IsServer())
            {
                // Connection approval callback
                netManager.ConnectionApprovalCallback += ApprovalCheck;
                Debug.Log("Connection approval callback set");
            }
        }

        private void ApprovalCheck(Unity.Netcode.NetworkManager.ConnectionApprovalRequest request, 
                                   Unity.Netcode.NetworkManager.ConnectionApprovalResponse response)
        {
            Network.VintageBeefNetworkManager networkManager = Network.VintageBeefNetworkManager.Instance;
            Unity.Netcode.NetworkManager netManager = networkManager?.GetNetworkManager();
            
            // Check if we've reached max players (including the host)
            int maxPlayers = networkManager?.GetMaxPlayers() ?? 12;
            int currentPlayers = netManager?.ConnectedClientsIds.Count ?? 0;
            
            if (currentPlayers >= maxPlayers)
            {
                // Reject connection - server is full
                response.Approved = false;
                response.Reason = "Server is full";
                response.CreatePlayerObject = false;
                Debug.Log($"Player connection rejected. Server is full ({currentPlayers}/{maxPlayers})");
            }
            else
            {
                // Accept connection
                response.Approved = true;
                response.CreatePlayerObject = true;
                
                // Get spawn position from TerrainManager
                Vector3 spawnPos = spawnPosition;
                if (TerrainManager.Instance != null)
                {
                    spawnPos = TerrainManager.Instance.GetRandomSpawnPosition(spawnRadius);
                }
                else
                {
                    // Fallback to random offset
                    Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                    randomOffset.y = 0;
                    spawnPos = spawnPosition + randomOffset;
                }

                response.Position = spawnPos;
                response.Rotation = Quaternion.identity;

                Debug.Log($"Player connection approved. Spawn position: {response.Position}. Current players: {currentPlayers}/{maxPlayers}, new player joining");
            }
            
            response.Pending = false;
        }

        private void SpawnPlayer()
        {
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);
            }

            // Get safe spawn position from TerrainManager
            Vector3 safeSpawnPosition = spawnPosition;
            if (TerrainManager.Instance != null)
            {
                safeSpawnPosition = TerrainManager.Instance.GetSafeSpawnPosition();
                Debug.Log($"[GameManager] Using TerrainManager spawn position: {safeSpawnPosition}");
            }
            else
            {
                Debug.LogWarning("[GameManager] TerrainManager not found, using default spawn position");
            }

            if (playerPrefab != null)
            {
                currentPlayer = Instantiate(playerPrefab, safeSpawnPosition, Quaternion.identity);
                currentPlayer.tag = "Player";
                Debug.Log("Player spawned!");
            }
            else
            {
                // Create default player if no prefab
                CreateDefaultPlayer(safeSpawnPosition);
            }
        }

        private void CreateDefaultPlayer()
        {
            Vector3 safeSpawnPosition = spawnPosition;
            if (TerrainManager.Instance != null)
            {
                safeSpawnPosition = TerrainManager.Instance.GetSafeSpawnPosition();
            }
            CreateDefaultPlayer(safeSpawnPosition);
        }

        private void CreateDefaultPlayer(Vector3 position)
        {
            currentPlayer = new GameObject("Player");
            currentPlayer.tag = "Player";
            currentPlayer.transform.position = position;

            // Add character controller
            CharacterController controller = currentPlayer.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;

            // Add player controller script
            currentPlayer.AddComponent<PlayerController>();

            // Add camera
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(currentPlayer.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.6f, 0);
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.tag = "MainCamera";

            // Add audio listener
            cameraObj.AddComponent<AudioListener>();

            Debug.Log("Default player created!");
        }

        public void ReturnToMainMenu()
        {
            // Shutdown network if in multiplayer mode
            if (isMultiplayerMode && Network.VintageBeefNetworkManager.Instance != null)
            {
                Network.VintageBeefNetworkManager.Instance.Shutdown();
            }

            SceneManager.LoadScene("MainMenu");
        }

        private void Update()
        {
            // ESC to return to menu (for testing)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public bool IsMultiplayerMode()
        {
            return isMultiplayerMode;
        }
    }
}
