using UnityEngine;
using Unity.Netcode;

namespace VintageBeef.Network
{
    /// <summary>
    /// Basic network manager setup for multiplayer
    /// Supports up to 12 players for the 12 professions
    /// </summary>
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager Instance { get; private set; }

        [Header("Network Settings")]
        [SerializeField] private int maxPlayers = 12;
        [SerializeField] private bool isHost = false;

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
            // Initialize network settings
            Debug.Log($"Network Manager initialized. Max players: {maxPlayers}");
        }

        public void StartHost()
        {
            isHost = true;
            // TODO: Start Unity Netcode as host
            Debug.Log("Starting as host...");
        }

        public void StartClient(string ipAddress)
        {
            isHost = false;
            // TODO: Connect to host
            Debug.Log($"Connecting to {ipAddress}...");
        }

        public int GetMaxPlayers()
        {
            return maxPlayers;
        }

        public bool IsHost()
        {
            return isHost;
        }
    }
}
