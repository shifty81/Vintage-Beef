using UnityEngine;

namespace VintageBeef
{
    /// <summary>
    /// Makes an object always face the camera
    /// Used for player name displays
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (mainCamera != null)
            {
                transform.forward = mainCamera.transform.forward;
            }
        }
    }
}
