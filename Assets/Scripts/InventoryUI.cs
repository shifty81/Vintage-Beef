using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using VintageBeef;

namespace VintageBeef.UI
{
    /// <summary>
    /// Inventory UI manager for displaying and interacting with player inventory
    /// Simple and clean Palia-style interface
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject inventoryPanel;
        [SerializeField] private Transform slotContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private TMP_Text titleText;

        [Header("Settings")]
        [SerializeField] private KeyCode toggleKey = KeyCode.I;

        private PlayerInventory playerInventory;
        private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
        private bool isOpen = false;

        private void Start()
        {
            // Find player inventory
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerInventory = player.GetComponent<PlayerInventory>();
                if (playerInventory == null)
                {
                    playerInventory = player.AddComponent<PlayerInventory>();
                }
            }

            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(false);
            }

            if (titleText != null)
            {
                titleText.text = "INVENTORY";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                ToggleInventory();
            }
        }

        public void ToggleInventory()
        {
            isOpen = !isOpen;
            
            if (inventoryPanel != null)
            {
                inventoryPanel.SetActive(isOpen);
            }

            if (isOpen)
            {
                RefreshInventory();
                
                // Disable player controls while inventory is open
                PlayerController localPlayer = FindLocalPlayerController();
                if (localPlayer != null)
                {
                    localPlayer.EnableControls(false);
                }
                
                // Disable interactions while inventory is open
                PlayerInteraction interaction = FindLocalPlayerInteraction();
                if (interaction != null)
                {
                    interaction.SetCanInteract(false);
                }
            }
            else
            {
                // Re-enable player controls
                PlayerController localPlayer = FindLocalPlayerController();
                if (localPlayer != null)
                {
                    localPlayer.EnableControls(true);
                }
                
                // Re-enable interactions
                PlayerInteraction interaction = FindLocalPlayerInteraction();
                if (interaction != null)
                {
                    interaction.SetCanInteract(true);
                }
            }
        }

        public void RefreshInventory()
        {
            if (playerInventory == null) return;

            // Clear existing UI slots
            foreach (InventorySlotUI slotUI in slotUIs)
            {
                if (slotUI != null && slotUI.gameObject != null)
                {
                    Destroy(slotUI.gameObject);
                }
            }
            slotUIs.Clear();

            // Create UI slots
            List<InventorySlot> items = playerInventory.GetAllItems();
            int emptySlots = playerInventory.GetEmptySlotCount();
            int totalSlots = items.Count + Mathf.Min(emptySlots, 30 - items.Count);

            for (int i = 0; i < totalSlots; i++)
            {
                GameObject slotObj = CreateSlotUI();
                slotObj.transform.SetParent(slotContainer, false);

                InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
                if (slotUI == null)
                {
                    slotUI = slotObj.AddComponent<InventorySlotUI>();
                }

                if (i < items.Count)
                {
                    InventorySlot item = items[i];
                    slotUI.SetItem(item.itemName, item.quantity);
                }
                else
                {
                    slotUI.SetEmpty();
                }

                slotUIs.Add(slotUI);
            }
        }

        private GameObject CreateSlotUI()
        {
            if (slotPrefab != null)
            {
                return Instantiate(slotPrefab);
            }

            // Create default slot UI
            GameObject slotObj = new GameObject("InventorySlot");
            
            Image background = slotObj.AddComponent<Image>();
            background.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            
            RectTransform rect = slotObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(80, 80);

            // Item name text
            GameObject nameObj = new GameObject("ItemName");
            nameObj.transform.SetParent(slotObj.transform);
            TMP_Text nameText = nameObj.AddComponent<TMP_Text>();
            nameText.fontSize = 12;
            nameText.alignment = TextAlignmentOptions.Center;
            nameText.color = Color.white;
            
            RectTransform nameRect = nameObj.GetComponent<RectTransform>();
            nameRect.anchorMin = new Vector2(0, 0);
            nameRect.anchorMax = new Vector2(1, 0.5f);
            nameRect.sizeDelta = Vector2.zero;
            nameRect.anchoredPosition = Vector2.zero;

            // Quantity text
            GameObject qtyObj = new GameObject("Quantity");
            qtyObj.transform.SetParent(slotObj.transform);
            TMP_Text qtyText = qtyObj.AddComponent<TMP_Text>();
            qtyText.fontSize = 16;
            qtyText.fontStyle = FontStyles.Bold;
            qtyText.alignment = TextAlignmentOptions.BottomRight;
            qtyText.color = Color.yellow;
            
            RectTransform qtyRect = qtyObj.GetComponent<RectTransform>();
            qtyRect.anchorMin = new Vector2(0, 0);
            qtyRect.anchorMax = new Vector2(1, 1);
            qtyRect.sizeDelta = new Vector2(-10, -10);
            qtyRect.anchoredPosition = Vector2.zero;

            return slotObj;
        }

        private PlayerController FindLocalPlayerController()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                return player.GetComponent<PlayerController>();
            }
            return null;
        }

        private PlayerInteraction FindLocalPlayerInteraction()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                return player.GetComponent<PlayerInteraction>();
            }
            return null;
        }
    }

    /// <summary>
    /// UI component for a single inventory slot
    /// </summary>
    public class InventorySlotUI : MonoBehaviour
    {
        private TMP_Text nameText;
        private TMP_Text quantityText;

        private void Awake()
        {
            nameText = transform.Find("ItemName")?.GetComponent<TMP_Text>();
            quantityText = transform.Find("Quantity")?.GetComponent<TMP_Text>();
        }

        public void SetItem(string itemName, int quantity)
        {
            if (nameText != null)
            {
                nameText.text = itemName;
            }
            
            if (quantityText != null)
            {
                quantityText.text = quantity > 1 ? quantity.ToString() : "";
            }
        }

        public void SetEmpty()
        {
            if (nameText != null)
            {
                nameText.text = "";
            }
            
            if (quantityText != null)
            {
                quantityText.text = "";
            }
        }
    }
}
