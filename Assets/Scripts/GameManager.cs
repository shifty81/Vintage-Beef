using UnityEngine;
using UnityEngine.SceneManagement;

namespace VintageBeef
{
    /// <summary>
    /// Main game manager that handles game state and coordination
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Player Settings")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Vector3 spawnPosition = new Vector3(0, 2, 0);

        private GameObject currentPlayer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Subscribe to scene loaded events
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Scene loaded: {scene.name}");

            // Spawn player in game world
            if (scene.name == "GameWorld")
            {
                SpawnPlayer();
            }
        }

        private void SpawnPlayer()
        {
            if (currentPlayer != null)
            {
                Destroy(currentPlayer);
            }

            if (playerPrefab != null)
            {
                currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                currentPlayer.tag = "Player";
                Debug.Log("Player spawned!");
            }
            else
            {
                // Create default player if no prefab
                CreateDefaultPlayer();
            }
        }

        private void CreateDefaultPlayer()
        {
            currentPlayer = new GameObject("Player");
            currentPlayer.tag = "Player";
            currentPlayer.transform.position = spawnPosition;

            // Add character controller
            CharacterController controller = currentPlayer.AddComponent<CharacterController>();
            controller.height = 2f;
            controller.radius = 0.5f;

            // Add player controller script
            currentPlayer.AddComponent<PlayerController>();

            // Add camera
            GameObject cameraObj = new GameObject("PlayerCamera");
            cameraObj.transform.SetParent(currentPlayer.transform);
            cameraObj.transform.localPosition = new Vector3(0, 1.6f, 0);
            Camera cam = cameraObj.AddComponent<Camera>();
            cam.tag = "MainCamera";

            // Add audio listener
            cameraObj.AddComponent<AudioListener>();

            Debug.Log("Default player created!");
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void Update()
        {
            // ESC to return to menu (for testing)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
