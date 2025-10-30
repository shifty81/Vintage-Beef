using UnityEngine;
using Unity.Netcode;
using TMPro;
using VintageBeef;

namespace VintageBeef.Network
{
    /// <summary>
    /// Network-enabled player component
    /// Handles player name display and network ownership
    /// </summary>
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkPlayer : NetworkBehaviour
    {
        [Header("Player Info")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private float nameTextHeight = 2.5f;

        private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>(
            new NetworkString("Player"),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        private NetworkVariable<int> professionIndex = new NetworkVariable<int>(
            -1,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            // Subscribe to network variable changes
            playerName.OnValueChanged += OnPlayerNameChanged;
            professionIndex.OnValueChanged += OnProfessionChanged;

            // Set initial values if this is the local player
            if (IsOwner)
            {
                // Set player name from PlayerData
                if (PlayerData.Instance != null)
                {
                    playerName.Value = new NetworkString(PlayerData.Instance.PlayerName);
                    professionIndex.Value = PlayerData.Instance.ProfessionIndex;
                }
                
                // Enable player controller for local player
                PlayerController controller = GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.enabled = true;
                }
            }
            else
            {
                // Disable player controller for remote players
                PlayerController controller = GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.enabled = false;
                }
            }

            // Setup name display
            SetupNameDisplay();
            UpdateNameDisplay(playerName.Value.ToString());
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            // Unsubscribe from network variable changes
            playerName.OnValueChanged -= OnPlayerNameChanged;
            professionIndex.OnValueChanged -= OnProfessionChanged;
        }

        private void SetupNameDisplay()
        {
            if (nameText == null)
            {
                // Create name text display above player
                GameObject nameCanvas = new GameObject("NameCanvas");
                nameCanvas.transform.SetParent(transform);
                nameCanvas.transform.localPosition = new Vector3(0, nameTextHeight, 0);

                Canvas canvas = nameCanvas.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.sortingOrder = 10;

                RectTransform canvasRect = nameCanvas.GetComponent<RectTransform>();
                canvasRect.sizeDelta = new Vector2(2f, 0.5f);
                canvasRect.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                GameObject textObj = new GameObject("NameText");
                textObj.transform.SetParent(nameCanvas.transform);
                textObj.transform.localPosition = Vector3.zero;
                textObj.transform.localScale = Vector3.one;

                nameText = textObj.AddComponent<TMP_Text>();
                nameText.alignment = TextAlignmentOptions.Center;
                nameText.fontSize = 36;
                nameText.color = Color.white;
                nameText.outlineWidth = 0.2f;
                nameText.outlineColor = Color.black;

                RectTransform textRect = textObj.GetComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.sizeDelta = Vector2.zero;

                // Make the name face camera
                nameCanvas.AddComponent<Billboard>();
            }
        }

        private void OnPlayerNameChanged(NetworkString oldName, NetworkString newName)
        {
            UpdateNameDisplay(newName.ToString());
        }

        private void OnProfessionChanged(int oldProf, int newProf)
        {
            Debug.Log($"Player profession changed to {newProf}");
        }

        private void UpdateNameDisplay(string name)
        {
            if (nameText != null)
            {
                string professionName = "";
                if (ProfessionManager.Instance != null && professionIndex.Value >= 0)
                {
                    Profession prof = ProfessionManager.Instance.GetProfession(professionIndex.Value);
                    if (prof != null)
                    {
                        professionName = $"\n[{prof.professionName}]";
                    }
                }
                nameText.text = name + professionName;
            }
        }

        /// <summary>
        /// Set player name (only owner can call this)
        /// </summary>
        public void SetPlayerName(string name)
        {
            if (IsOwner)
            {
                playerName.Value = new NetworkString(name);
            }
        }

        /// <summary>
        /// Set profession (only owner can call this)
        /// </summary>
        public void SetProfession(int index)
        {
            if (IsOwner)
            {
                professionIndex.Value = index;
            }
        }

        public string GetPlayerName()
        {
            return playerName.Value.ToString();
        }

        public int GetProfessionIndex()
        {
            return professionIndex.Value;
        }
    }

    /// <summary>
    /// Helper struct for network string
    /// </summary>
    public struct NetworkString : INetworkSerializable
    {
        private string value;

        public NetworkString(string value)
        {
            this.value = value;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out value);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(value);
            }
        }

        public override string ToString()
        {
            return value ?? "";
        }
    }
}
