using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VintageBeef.UI
{
    /// <summary>
    /// Manages network connection UI in the lobby
    /// </summary>
    public class ConnectionUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField usernameInput;
        [SerializeField] private TMP_InputField ipAddressInput;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private GameObject connectionPanel;
        [SerializeField] private GameObject professionPanel;

        private const ushort DEFAULT_PORT = 7777;
        private bool isConnecting = false;

        private void Start()
        {
            // Set default values
            if (usernameInput != null)
            {
                usernameInput.text = "Player" + Random.Range(1000, 9999);
            }

            if (ipAddressInput != null)
            {
                ipAddressInput.text = "127.0.0.1";
            }

            // Setup button listeners
            if (hostButton != null)
            {
                hostButton.onClick.AddListener(OnHostButtonClicked);
            }

            if (joinButton != null)
            {
                joinButton.onClick.AddListener(OnJoinButtonClicked);
            }

            // Show connection panel first
            ShowConnectionPanel(true);
            UpdateStatusText("Enter your name and choose Host or Join");
        }

        private void OnHostButtonClicked()
        {
            if (isConnecting) return;

            string username = usernameInput != null ? usernameInput.text : "Host";
            if (string.IsNullOrWhiteSpace(username))
            {
                UpdateStatusText("Please enter a username!");
                return;
            }

            // Set player name
            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.PlayerName = username;
            }

            // Get or create network manager
            Network.VintageBeefNetworkManager networkManager = Network.VintageBeefNetworkManager.Instance;
            if (networkManager == null)
            {
                GameObject networkObj = new GameObject("NetworkManager");
                networkManager = networkObj.AddComponent<Network.VintageBeefNetworkManager>();
            }

            // Start as host
            isConnecting = true;
            UpdateStatusText("Starting as host...");
            DisableButtons(true);

            bool success = networkManager.StartHost();

            if (success)
            {
                UpdateStatusText("Host started! Waiting for players...");
                ShowConnectionPanel(false);
                Debug.Log("Host started successfully");
            }
            else
            {
                UpdateStatusText("Failed to start host. Try again.");
                isConnecting = false;
                DisableButtons(false);
                Debug.LogError("Failed to start host");
            }
        }

        private void OnJoinButtonClicked()
        {
            if (isConnecting) return;

            string username = usernameInput != null ? usernameInput.text : "Client";
            if (string.IsNullOrWhiteSpace(username))
            {
                UpdateStatusText("Please enter a username!");
                return;
            }

            string ipAddress = ipAddressInput != null ? ipAddressInput.text : "127.0.0.1";
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                UpdateStatusText("Please enter an IP address!");
                return;
            }

            // Set player name
            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.PlayerName = username;
            }

            // Get or create network manager
            Network.VintageBeefNetworkManager networkManager = Network.VintageBeefNetworkManager.Instance;
            if (networkManager == null)
            {
                GameObject networkObj = new GameObject("NetworkManager");
                networkManager = networkObj.AddComponent<Network.VintageBeefNetworkManager>();
            }

            // Set connection data
            networkManager.SetConnectionData(ipAddress, DEFAULT_PORT);

            // Start as client
            isConnecting = true;
            UpdateStatusText($"Connecting to {ipAddress}...");
            DisableButtons(true);

            bool success = networkManager.StartClient();

            if (success)
            {
                UpdateStatusText("Connected! Select your profession...");
                ShowConnectionPanel(false);
                Debug.Log("Client connected successfully");
            }
            else
            {
                UpdateStatusText("Failed to connect. Check IP and try again.");
                isConnecting = false;
                DisableButtons(false);
                Debug.LogError("Failed to connect to host");
            }
        }

        private void UpdateStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[ConnectionUI] {message}");
        }

        private void DisableButtons(bool disabled)
        {
            if (hostButton != null)
            {
                hostButton.interactable = !disabled;
            }

            if (joinButton != null)
            {
                joinButton.interactable = !disabled;
            }
        }

        private void ShowConnectionPanel(bool show)
        {
            if (connectionPanel != null)
            {
                connectionPanel.SetActive(show);
            }

            if (professionPanel != null)
            {
                professionPanel.SetActive(!show);
            }
        }

        /// <summary>
        /// Called by external scripts when connection is lost
        /// </summary>
        public void OnDisconnected()
        {
            UpdateStatusText("Disconnected from server");
            ShowConnectionPanel(true);
            isConnecting = false;
            DisableButtons(false);
        }
    }
}
