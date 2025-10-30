using UnityEngine;
using VintageBeef;

namespace VintageBeef.World
{
    /// <summary>
    /// Resource node that can be gathered by players
    /// Provides materials based on resource type and biome
    /// </summary>
    public class ResourceNode : MonoBehaviour
    {
        [Header("Resource Info")]
        public ResourceType resourceType;
        public BiomeType biome;
        
        [Header("Resource Properties")]
        [SerializeField] private int hitPoints = 3;
        [SerializeField] private float gatherRadius = 3f;
        [SerializeField] private float respawnTime = 60f;

        private int currentHitPoints;
        private bool isDepleted = false;
        private float respawnTimer = 0f;
        private Renderer[] renderers;

        private void Awake()
        {
            currentHitPoints = hitPoints;
            renderers = GetComponentsInChildren<Renderer>();
        }

        private void Update()
        {
            if (isDepleted)
            {
                respawnTimer += Time.deltaTime;
                if (respawnTimer >= respawnTime)
                {
                    Respawn();
                }
                return;
            }

            // Check for nearby players with 'E' key
            CheckForGathering();
        }

        private void CheckForGathering()
        {
            // Find all player objects
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            
            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                
                if (distance <= gatherRadius)
                {
                    // Show gathering prompt (in a real implementation, this would be UI)
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Gather(player);
                    }
                }
            }
        }

        private void Gather(GameObject player)
        {
            currentHitPoints--;
            
            Debug.Log($"Gathered {resourceType} from {biome} biome. HP: {currentHitPoints}/{hitPoints}");

            // Award resources to player
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddResource(GetResourceItem());
            }

            // Visual feedback - make smaller
            float scale = currentHitPoints / (float)hitPoints;
            transform.localScale = transform.localScale * 0.9f;

            if (currentHitPoints <= 0)
            {
                Deplete();
            }
        }

        private ResourceItem GetResourceItem()
        {
            ResourceItem item = new ResourceItem();
            
            switch (resourceType)
            {
                case ResourceType.Tree:
                    item.name = "Wood";
                    item.amount = Random.Range(2, 5);
                    item.type = "material";
                    break;
                
                case ResourceType.Rock:
                    item.name = biome == BiomeType.Mountains ? "Iron Ore" : "Stone";
                    item.amount = Random.Range(1, 3);
                    item.type = "material";
                    break;
                
                case ResourceType.Plant:
                    item.name = biome == BiomeType.Desert ? "Cactus Fruit" : "Herbs";
                    item.amount = Random.Range(1, 2);
                    item.type = "consumable";
                    break;
            }

            return item;
        }

        private void Deplete()
        {
            isDepleted = true;
            respawnTimer = 0f;
            
            // Hide the resource
            foreach (Renderer r in renderers)
            {
                r.enabled = false;
            }

            // Disable colliders
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }

            Debug.Log($"{resourceType} depleted. Will respawn in {respawnTime} seconds.");
        }

        private void Respawn()
        {
            isDepleted = false;
            currentHitPoints = hitPoints;
            
            // Reset scale
            transform.localScale = Vector3.one;
            
            // Show the resource
            foreach (Renderer r in renderers)
            {
                r.enabled = true;
            }

            // Enable colliders
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = true;
            }

            Debug.Log($"{resourceType} respawned!");
        }

        private void OnDrawGizmosSelected()
        {
            // Show gather radius in editor
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, gatherRadius);
        }
    }

    /// <summary>
    /// Represents a gathered resource item
    /// </summary>
    [System.Serializable]
    public class ResourceItem
    {
        public string name;
        public int amount;
        public string type; // material, consumable, etc.
    }
}
