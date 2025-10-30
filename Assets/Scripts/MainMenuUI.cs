using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace VintageBeef.UI
{
    /// <summary>
    /// Manages the main menu UI
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private TMP_Text titleText;

        private void Start()
        {
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnPlayClicked);
            }

            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitClicked);
            }

            if (titleText != null)
            {
                titleText.text = "VINTAGE BEEF";
            }
        }

        private void OnPlayClicked()
        {
            // Load lobby scene
            SceneManager.LoadScene("Lobby");
        }

        private void OnQuitClicked()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
