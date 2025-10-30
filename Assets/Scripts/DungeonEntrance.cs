using UnityEngine;

namespace VintageBeef
{
    /// <summary>
    /// Represents a dungeon entrance in the world
    /// Players can interact with this to enter a dungeon
    /// </summary>
    public class DungeonEntrance : MonoBehaviour
    {
        [Header("Dungeon Settings")]
        [SerializeField] private string dungeonName = "Unknown Dungeon";
        [SerializeField] private int recommendedLevel = 1;
        [SerializeField] private int maxPlayers = 4;

        [Header("Visual Settings")]
        [SerializeField] private Color glowColor = Color.cyan;
        [SerializeField] private float interactionRange = 3f;

        private Transform playerTransform;
        private bool isPlayerNearby = false;

        private void Update()
        {
            CheckPlayerProximity();
        }

        private void CheckPlayerProximity()
        {
            // Find player if not cached
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }

            if (playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                isPlayerNearby = distance <= interactionRange;

                // Check for interaction input
                if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
                {
                    OnInteract();
                }
            }
        }

        private void OnInteract()
        {
            Debug.Log($"Entering {dungeonName}!");
            // TODO: Load dungeon scene or instance
            // For now, just log the interaction
        }

        private void OnDrawGizmos()
        {
            // Draw interaction range in editor
            Gizmos.color = glowColor;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }

        private void OnGUI()
        {
            if (isPlayerNearby)
            {
                // Show interaction prompt
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                if (screenPos.z > 0)
                {
                    GUI.Label(new Rect(screenPos.x - 75, Screen.height - screenPos.y - 20, 150, 40), 
                        $"[E] Enter {dungeonName}");
                }
            }
        }
    }
}
