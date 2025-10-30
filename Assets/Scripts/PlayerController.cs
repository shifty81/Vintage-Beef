using UnityEngine;

namespace VintageBeef
{
    /// <summary>
    /// Simple player controller for character movement
    /// Optimized for lower-end hardware with Palia-style performance
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float turnSmoothTime = 0.1f;

        [Header("Camera Settings")]
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float mouseSensitivity = 2f;
        [SerializeField] private float minVerticalAngle = -60f;
        [SerializeField] private float maxVerticalAngle = 60f;

        private CharacterController characterController;
        private Vector3 velocity;
        private float turnSmoothVelocity;
        private float verticalRotation = 0f;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            
            // Lock cursor for gameplay
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Find camera if not assigned
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main?.transform;
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleCamera();
        }

        private void HandleMovement()
        {
            // Ground check
            bool isGrounded = characterController.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small negative value to keep grounded
            }

            // Get input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            // Movement
            if (direction.magnitude >= 0.1f)
            {
                // Calculate target angle based on camera direction
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                
                // Rotate player to face movement direction
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Move in the direction the player is facing
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                // Check for sprint
                float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
                
                characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
            }

            // Jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private void HandleCamera()
        {
            if (cameraTransform == null) return;

            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Rotate player horizontally
            transform.Rotate(Vector3.up * mouseX);

            // Rotate camera vertically
            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }

        public void EnableControls(bool enable)
        {
            enabled = enable;
            Cursor.lockState = enable ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enable;
        }
    }
}
