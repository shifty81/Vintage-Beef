using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using VintageBeef;

namespace VintageBeef.UI
{
    /// <summary>
    /// Manages the lobby where players select their profession
    /// </summary>
    public class LobbyUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private Transform professionButtonContainer;
        [SerializeField] private GameObject professionButtonPrefab;
        [SerializeField] private Button startGameButton;
        [SerializeField] private TMP_Text selectedProfessionText;

        private int selectedProfessionIndex = -1;
        private List<Button> professionButtons = new List<Button>();

        private void Start()
        {
            if (titleText != null)
            {
                titleText.text = "SELECT YOUR PROFESSION";
            }

            if (startGameButton != null)
            {
                startGameButton.onClick.AddListener(OnStartGameClicked);
                startGameButton.interactable = false;
            }

            CreateProfessionButtons();
        }

        private void CreateProfessionButtons()
        {
            if (ProfessionManager.Instance == null || professionButtonContainer == null)
            {
                Debug.LogError("Missing ProfessionManager or button container!");
                return;
            }

            List<Profession> professions = ProfessionManager.Instance.GetAllProfessions();

            for (int i = 0; i < professions.Count; i++)
            {
                int index = i; // Capture for closure
                Profession profession = professions[i];

                GameObject buttonObj;
                Button button;

                if (professionButtonPrefab != null)
                {
                    buttonObj = Instantiate(professionButtonPrefab, professionButtonContainer);
                    button = buttonObj.GetComponent<Button>();
                }
                else
                {
                    // Create simple button if no prefab provided
                    buttonObj = new GameObject($"Profession_{profession.professionName}");
                    buttonObj.transform.SetParent(professionButtonContainer);
                    button = buttonObj.AddComponent<Button>();
                    
                    Image image = buttonObj.AddComponent<Image>();
                    image.color = Color.white;
                }

                // Add text to button
                TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                if (buttonText == null)
                {
                    GameObject textObj = new GameObject("Text");
                    textObj.transform.SetParent(buttonObj.transform);
                    buttonText = textObj.AddComponent<TMP_Text>();
                    buttonText.alignment = TextAlignmentOptions.Center;
                    buttonText.fontSize = 18;
                    buttonText.color = Color.black;
                    
                    RectTransform rectTransform = textObj.GetComponent<RectTransform>();
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                }

                buttonText.text = profession.professionName;

                button.onClick.AddListener(() => OnProfessionSelected(index));
                professionButtons.Add(button);
            }
        }

        private void OnProfessionSelected(int index)
        {
            selectedProfessionIndex = index;

            if (PlayerData.Instance != null)
            {
                PlayerData.Instance.SetProfession(index);
            }

            // Update UI
            if (selectedProfessionText != null && ProfessionManager.Instance != null)
            {
                Profession profession = ProfessionManager.Instance.GetProfession(index);
                if (profession != null)
                {
                    selectedProfessionText.text = $"Selected: {profession.professionName}\n{profession.description}";
                }
            }

            // Enable start button
            if (startGameButton != null)
            {
                startGameButton.interactable = true;
            }

            Debug.Log($"Profession selected: {index}");
        }

        private void OnStartGameClicked()
        {
            if (selectedProfessionIndex == -1)
            {
                Debug.LogWarning("No profession selected!");
                return;
            }

            // Load game world
            SceneManager.LoadScene("GameWorld");
        }
    }
}
