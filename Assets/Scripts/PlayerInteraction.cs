using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VintageBeef.World;

namespace VintageBeef
{
    /// <summary>
    /// Handles player interaction with world objects like resource nodes
    /// Provides centralized interaction detection and UI feedback
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private KeyCode interactionKey = KeyCode.E;
        [SerializeField] private LayerMask interactableLayer = -1; // All layers by default
        
        [Header("UI References")]
        [SerializeField] private GameObject interactionPrompt;
        [SerializeField] private TMP_Text promptText;
        
        private ResourceNode nearestResource;
        private PlayerInventory inventory;
        private bool canInteract = true;

        private void Awake()
        {
            inventory = GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                inventory = gameObject.AddComponent<PlayerInventory>();
                Debug.Log("[PlayerInteraction] Added PlayerInventory component");
            }
        }

        private void Start()
        {
            // Create UI prompt if it doesn't exist
            if (interactionPrompt == null)
            {
                CreateInteractionPrompt();
            }
            
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void Update()
        {
            if (!canInteract) return;

            // Find nearest interactable resource
            nearestResource = FindNearestResource();

            // Show/hide interaction prompt
            UpdateInteractionPrompt();

            // Handle interaction input
            if (nearestResource != null && Input.GetKeyDown(interactionKey))
            {
                InteractWithResource();
            }
        }

        private ResourceNode FindNearestResource()
        {
            // Use OverlapSphere to find nearby resources efficiently
            Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);
            
            ResourceNode closest = null;
            float closestDistance = float.MaxValue;

            foreach (Collider col in colliders)
            {
                ResourceNode resource = col.GetComponent<ResourceNode>();
                if (resource != null && !resource.IsDepleted())
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closest = resource;
                    }
                }
            }

            return closest;
        }

        private void UpdateInteractionPrompt()
        {
            if (interactionPrompt == null) return;

            if (nearestResource != null)
            {
                interactionPrompt.SetActive(true);
                
                if (promptText != null)
                {
                    string resourceName = nearestResource.resourceType.ToString();
                    promptText.text = $"Press {interactionKey} to gather {resourceName}";
                }
            }
            else
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void InteractWithResource()
        {
            if (nearestResource == null || inventory == null) return;

            // Attempt to gather from the resource
            bool success = nearestResource.TryGather(gameObject);
            
            if (success)
            {
                Debug.Log($"[PlayerInteraction] Successfully gathered {nearestResource.resourceType}");
            }
            else
            {
                Debug.LogWarning("[PlayerInteraction] Failed to gather resource");
            }
        }

        private void CreateInteractionPrompt()
        {
            // Find or create Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("WorldCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
                canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                Debug.Log("[PlayerInteraction] Created Canvas for interaction prompt");
            }

            // Create prompt panel
            interactionPrompt = new GameObject("InteractionPrompt");
            interactionPrompt.transform.SetParent(canvas.transform, false);
            
            RectTransform promptRect = interactionPrompt.AddComponent<RectTransform>();
            promptRect.anchorMin = new Vector2(0.5f, 0.3f);
            promptRect.anchorMax = new Vector2(0.5f, 0.3f);
            promptRect.sizeDelta = new Vector2(300, 60);
            promptRect.anchoredPosition = Vector2.zero;

            // Add background
            Image background = interactionPrompt.AddComponent<Image>();
            background.color = new Color(0f, 0f, 0f, 0.7f);

            // Create text
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(interactionPrompt.transform, false);
            
            promptText = textObj.AddComponent<TMP_Text>();
            promptText.text = "Press E to interact";
            promptText.fontSize = 18;
            promptText.alignment = TextAlignmentOptions.Center;
            promptText.color = Color.white;
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = new Vector2(-20, -10);
            textRect.anchoredPosition = Vector2.zero;

            interactionPrompt.SetActive(false);
            Debug.Log("[PlayerInteraction] Created interaction prompt UI");
        }

        public void SetCanInteract(bool value)
        {
            canInteract = value;
            
            if (!canInteract && interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        /// <summary>
        /// Called by InventoryUI or other systems to notify when controls are disabled
        /// </summary>
        private void OnDisable()
        {
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Show interaction range in editor
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}
