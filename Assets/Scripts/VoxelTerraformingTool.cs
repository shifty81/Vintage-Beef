using UnityEngine;

namespace VintageBeef.Voxel
{
    /// <summary>
    /// Provides terraforming tools for modifying voxel terrain
    /// Allows adding/removing voxels with raycast targeting
    /// </summary>
    public class VoxelTerraformingTool : MonoBehaviour
    {
        [Header("Tool Settings")]
        [SerializeField] private float maxReachDistance = 5f;
        [SerializeField] private VoxelType placeType = VoxelType.Stone;
        [SerializeField] private float toolCooldown = 0.2f;

        [Header("UI Feedback")]
        [SerializeField] private bool showTargetIndicator = true;
        
        private VoxelWorldGenerator worldGenerator;
        private Camera playerCamera;
        private float lastToolUseTime;

        private void Start()
        {
            worldGenerator = FindObjectOfType<VoxelWorldGenerator>();
            playerCamera = Camera.main;

            if (worldGenerator == null)
            {
                Debug.LogWarning("[VoxelTerraformingTool] No VoxelWorldGenerator found in scene!");
            }
        }

        private void Update()
        {
            if (worldGenerator == null || playerCamera == null) return;

            // Check for player input
            HandleInput();
        }

        private void HandleInput()
        {
            // Check cooldown
            if (Time.time - lastToolUseTime < toolCooldown)
                return;

            // Left click to remove voxel (dig/mine)
            if (Input.GetMouseButton(0))
            {
                if (RaycastVoxel(out Vector3 hitPosition, out bool hitVoxel))
                {
                    if (hitVoxel)
                    {
                        RemoveVoxel(hitPosition);
                        lastToolUseTime = Time.time;
                    }
                }
            }

            // Right click to place voxel
            if (Input.GetMouseButton(1))
            {
                if (RaycastVoxel(out Vector3 hitPosition, out bool hitVoxel))
                {
                    if (hitVoxel)
                    {
                        PlaceVoxel(hitPosition);
                        lastToolUseTime = Time.time;
                    }
                }
            }

            // Number keys to change voxel type
            if (Input.GetKeyDown(KeyCode.Alpha1)) placeType = VoxelType.Dirt;
            if (Input.GetKeyDown(KeyCode.Alpha2)) placeType = VoxelType.Grass;
            if (Input.GetKeyDown(KeyCode.Alpha3)) placeType = VoxelType.Stone;
            if (Input.GetKeyDown(KeyCode.Alpha4)) placeType = VoxelType.Sand;
        }

        /// <summary>
        /// Raycast to find voxel position
        /// </summary>
        private bool RaycastVoxel(out Vector3 hitPosition, out bool hitVoxel)
        {
            hitPosition = Vector3.zero;
            hitVoxel = false;

            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxReachDistance))
            {
                hitPosition = hit.point - hit.normal * 0.5f; // Position of hit voxel
                hitVoxel = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove a voxel at the specified world position
        /// </summary>
        private void RemoveVoxel(Vector3 worldPosition)
        {
            worldGenerator.SetVoxel(worldPosition, VoxelType.Air);
            Debug.Log($"[Terraforming] Removed voxel at {worldPosition}");
        }

        /// <summary>
        /// Place a voxel at the specified world position
        /// </summary>
        private void PlaceVoxel(Vector3 worldPosition)
        {
            // Calculate position where voxel should be placed (adjacent to hit surface)
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxReachDistance))
            {
                Vector3 placePosition = hit.point + hit.normal * 0.5f;
                worldGenerator.SetVoxel(placePosition, placeType);
                Debug.Log($"[Terraforming] Placed {placeType} voxel at {placePosition}");
            }
        }

        /// <summary>
        /// Set the voxel type to place
        /// </summary>
        public void SetPlaceType(VoxelType type)
        {
            placeType = type;
        }

        /// <summary>
        /// Get current place type
        /// </summary>
        public VoxelType GetPlaceType()
        {
            return placeType;
        }

        private void OnGUI()
        {
            if (!showTargetIndicator) return;

            // Draw crosshair
            float size = 10f;
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
            
            GUI.color = Color.white;
            GUI.Box(new Rect(center.x - size / 2, center.y - 1, size, 2), "");
            GUI.Box(new Rect(center.x - 1, center.y - size / 2, 2, size), "");

            // Show current tool info
            GUI.color = Color.yellow;
            string toolInfo = $"Terraforming Tool\n" +
                            $"Left Click: Remove | Right Click: Place\n" +
                            $"Current Block: {placeType}\n" +
                            $"1-4: Change block type";
            GUI.Label(new Rect(10, Screen.height - 100, 400, 100), toolInfo);
        }
    }
}
