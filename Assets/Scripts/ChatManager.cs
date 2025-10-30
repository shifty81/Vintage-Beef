using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using System.Collections.Generic;

namespace VintageBeef.Network
{
    /// <summary>
    /// Network chat system for player communication
    /// Supports text messages and basic commands
    /// </summary>
    public class ChatManager : NetworkBehaviour
    {
        public static ChatManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject chatPanel;
        [SerializeField] private TMP_InputField messageInput;
        [SerializeField] private TMP_Text chatDisplay;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int maxMessages = 50;

        [Header("Settings")]
        [SerializeField] private KeyCode toggleChatKey = KeyCode.Return;
        [SerializeField] private KeyCode sendMessageKey = KeyCode.Return;

        private Queue<string> messages = new Queue<string>();
        private bool isChatOpen = false;

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
            if (chatPanel != null)
            {
                chatPanel.SetActive(false);
            }

            if (messageInput != null)
            {
                messageInput.onSubmit.AddListener(OnMessageSubmit);
            }
        }

        private void Update()
        {
            // Toggle chat with Return key (when not in input field)
            if (Input.GetKeyDown(toggleChatKey) && !isChatOpen)
            {
                OpenChat();
            }
        }

        public void OpenChat()
        {
            if (chatPanel == null) return;

            isChatOpen = true;
            chatPanel.SetActive(true);

            if (messageInput != null)
            {
                messageInput.ActivateInputField();
                messageInput.Select();
            }

            // Disable player controls while typing
            PlayerController localPlayer = FindLocalPlayerController();
            if (localPlayer != null)
            {
                localPlayer.EnableControls(false);
            }
        }

        public void CloseChat()
        {
            if (chatPanel == null) return;

            isChatOpen = false;
            chatPanel.SetActive(false);

            if (messageInput != null)
            {
                messageInput.text = "";
            }

            // Re-enable player controls
            PlayerController localPlayer = FindLocalPlayerController();
            if (localPlayer != null)
            {
                localPlayer.EnableControls(true);
            }
        }

        private void OnMessageSubmit(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                CloseChat();
                return;
            }

            // Send message over network
            SendMessageServerRpc(message);

            // Clear input
            if (messageInput != null)
            {
                messageInput.text = "";
                messageInput.ActivateInputField();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendMessageServerRpc(string message, ServerRpcParams rpcParams = default)
        {
            // Get sender info
            ulong senderId = rpcParams.Receive.SenderClientId;
            string senderName = GetPlayerName(senderId);

            // Check for commands
            if (message.StartsWith("/"))
            {
                HandleCommand(message, senderId);
                return;
            }

            // Format message
            string formattedMessage = $"<color=yellow>{senderName}</color>: {message}";

            // Broadcast to all clients
            BroadcastMessageClientRpc(formattedMessage);
        }

        [ClientRpc]
        private void BroadcastMessageClientRpc(string message)
        {
            AddMessageToDisplay(message);
        }

        private void AddMessageToDisplay(string message)
        {
            messages.Enqueue(message);

            // Remove old messages if exceeding max
            while (messages.Count > maxMessages)
            {
                messages.Dequeue();
            }

            // Update display
            UpdateChatDisplay();

            // Scroll to bottom
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }

        private void UpdateChatDisplay()
        {
            if (chatDisplay == null) return;

            chatDisplay.text = string.Join("\n", messages);
        }

        private void HandleCommand(string command, ulong senderId)
        {
            string[] parts = command.Split(' ');
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "/help":
                    SendSystemMessageToClient(senderId, "Available commands: /help, /players, /time");
                    break;

                case "/players":
                    int playerCount = Unity.Netcode.NetworkManager.Singleton.ConnectedClientsList.Count;
                    SendSystemMessageToClient(senderId, $"Connected players: {playerCount}");
                    break;

                case "/time":
                    SendSystemMessageToClient(senderId, $"Server time: {System.DateTime.Now:HH:mm:ss}");
                    break;

                default:
                    SendSystemMessageToClient(senderId, $"Unknown command: {cmd}");
                    break;
            }
        }

        private void SendSystemMessageToClient(ulong clientId, string message)
        {
            string formattedMessage = $"<color=cyan>[System]</color> {message}";
            SendMessageToClientRpc(formattedMessage, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { clientId }
                }
            });
        }

        [ClientRpc]
        private void SendMessageToClientRpc(string message, ClientRpcParams rpcParams = default)
        {
            AddMessageToDisplay(message);
        }

        private string GetPlayerName(ulong clientId)
        {
            // Try to find the player's NetworkPlayer component
            foreach (var playerObj in FindObjectsOfType<NetworkPlayer>())
            {
                if (playerObj.OwnerClientId == clientId)
                {
                    return playerObj.GetPlayerName();
                }
            }

            return $"Player{clientId}";
        }

        private PlayerController FindLocalPlayerController()
        {
            // Find the local player's controller
            foreach (var player in FindObjectsOfType<NetworkPlayer>())
            {
                if (player.IsOwner)
                {
                    return player.GetComponent<PlayerController>();
                }
            }

            return null;
        }

        /// <summary>
        /// Public method to send system messages
        /// </summary>
        public void SendSystemMessage(string message)
        {
            if (IsServer)
            {
                string formattedMessage = $"<color=cyan>[System]</color> {message}";
                BroadcastMessageClientRpc(formattedMessage);
            }
        }

        /// <summary>
        /// Announce when a player joins
        /// </summary>
        public void AnnouncePlayerJoined(string playerName)
        {
            SendSystemMessage($"{playerName} joined the game!");
        }

        /// <summary>
        /// Announce when a player leaves
        /// </summary>
        public void AnnouncePlayerLeft(string playerName)
        {
            SendSystemMessage($"{playerName} left the game.");
        }
    }
}
