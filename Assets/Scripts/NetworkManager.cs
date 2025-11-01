using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

namespace VintageBeef.Network
{
    /// <summary>
    /// Network manager setup for multiplayer using Unity Netcode
    /// Supports up to 12 players for the 12 professions
    /// </summary>
    public class VintageBeefNetworkManager : MonoBehaviour
    {
        public static VintageBeefNetworkManager Instance { get; private set; }

        [Header("Network Settings")]
        [SerializeField] private int maxPlayers = 12;
        
        [Header("Connection Settings")]
        [SerializeField] private string ipAddress = "127.0.0.1";
        [SerializeField] private ushort port = 7777;

        private Unity.Netcode.NetworkManager networkManager;
        private UnityTransport transport;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                
                // Get or add Unity's NetworkManager component
                networkManager = GetComponent<Unity.Netcode.NetworkManager>();
                if (networkManager == null)
                {
                    networkManager = gameObject.AddComponent<Unity.Netcode.NetworkManager>();
                }
                
                // Get or add Unity Transport
                transport = GetComponent<UnityTransport>();
                if (transport == null)
                {
                    transport = gameObject.AddComponent<UnityTransport>();
                }
                
                // Configure transport
                transport.SetConnectionData(ipAddress, port);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Initialize network settings
            Debug.Log($"Vintage Beef Network Manager initialized. Max players: {maxPlayers}");
            
            // Configure NetworkManager
            if (networkManager != null)
            {
                // Enable connection approval to allow max player limit enforcement
                networkManager.NetworkConfig.ConnectionApproval = true;
            }
        }

        /// <summary>
        /// Start as host (server + client)
        /// </summary>
        public bool StartHost()
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not initialized!");
                return false;
            }

            Debug.Log("Starting as host...");
            bool success = networkManager.StartHost();
            
            if (success)
            {
                Debug.Log($"Host started successfully on {ipAddress}:{port}");
            }
            else
            {
                Debug.LogError("Failed to start host!");
            }
            
            return success;
        }

        /// <summary>
        /// Start as client and connect to host
        /// </summary>
        public bool StartClient()
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not initialized!");
                return false;
            }

            Debug.Log($"Connecting to {ipAddress}:{port}...");
            bool success = networkManager.StartClient();
            
            if (success)
            {
                Debug.Log("Client started successfully");
            }
            else
            {
                Debug.LogError("Failed to start client!");
            }
            
            return success;
        }

        /// <summary>
        /// Start as dedicated server (no local client)
        /// </summary>
        public bool StartServer()
        {
            if (networkManager == null)
            {
                Debug.LogError("NetworkManager not initialized!");
                return false;
            }

            Debug.Log("Starting as server...");
            bool success = networkManager.StartServer();
            
            if (success)
            {
                Debug.Log($"Server started successfully on port {port}");
            }
            else
            {
                Debug.LogError("Failed to start server!");
            }
            
            return success;
        }

        /// <summary>
        /// Shutdown network connection
        /// </summary>
        public void Shutdown()
        {
            if (networkManager != null && networkManager.IsListening)
            {
                networkManager.Shutdown();
                Debug.Log("Network connection shutdown");
            }
        }

        /// <summary>
        /// Set connection data before starting
        /// </summary>
        public void SetConnectionData(string ip, ushort portNumber)
        {
            ipAddress = ip;
            port = portNumber;
            
            if (transport != null)
            {
                transport.SetConnectionData(ipAddress, port);
                Debug.Log($"Connection data set to {ipAddress}:{port}");
            }
        }

        public int GetMaxPlayers()
        {
            return maxPlayers;
        }

        public bool IsHost()
        {
            return networkManager != null && networkManager.IsHost;
        }

        public bool IsClient()
        {
            return networkManager != null && networkManager.IsClient;
        }

        public bool IsServer()
        {
            return networkManager != null && networkManager.IsServer;
        }

        public bool IsConnected()
        {
            return networkManager != null && networkManager.IsConnectedClient;
        }

        public Unity.Netcode.NetworkManager GetNetworkManager()
        {
            return networkManager;
        }

        private void OnDestroy()
        {
            Shutdown();
        }
    }
}
