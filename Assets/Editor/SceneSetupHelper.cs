#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;
using VintageBeef;
using VintageBeef.UI;

namespace VintageBeef.Editor
{
    /// <summary>
    /// Helper tool to quickly setup scenes with required GameObjects and components
    /// </summary>
    public class SceneSetupHelper : EditorWindow
    {
        [MenuItem("Vintage Beef/Scene Setup Helper")]
        public static void ShowWindow()
        {
            GetWindow<SceneSetupHelper>("Scene Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Vintage Beef Scene Setup", EditorStyles.boldLabel);
            GUILayout.Space(10);

            EditorGUILayout.HelpBox(
                "Use these buttons to quickly setup scenes with required components. " +
                "Make sure you have the correct scene open before clicking.", 
                MessageType.Info);

            GUILayout.Space(10);

            if (GUILayout.Button("Setup MainMenu Scene", GUILayout.Height(30)))
            {
                SetupMainMenuScene();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Setup Lobby Scene", GUILayout.Height(30)))
            {
                SetupLobbyScene();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Setup GameWorld Scene", GUILayout.Height(30)))
            {
                SetupGameWorldScene();
            }

            GUILayout.Space(20);
            GUILayout.Label("Individual Components", EditorStyles.boldLabel);
            GUILayout.Space(5);

            if (GUILayout.Button("Add Canvas to Current Scene"))
            {
                CreateCanvas();
            }

            if (GUILayout.Button("Add GameManager"))
            {
                CreateGameManager();
            }

            if (GUILayout.Button("Add ProfessionManager"))
            {
                CreateProfessionManager();
            }

            if (GUILayout.Button("Add PlayerData"))
            {
                CreatePlayerData();
            }

            if (GUILayout.Button("Add TerrainManager"))
            {
                CreateTerrainManager();
            }
        }

        private void SetupMainMenuScene()
        {
            Debug.Log("Setting up MainMenu scene...");

            // Create managers
            CreateGameManager();
            CreateProfessionManager();
            CreatePlayerData();

            // Create Canvas
            GameObject canvas = CreateCanvas();
            if (canvas == null) return;

            // Create Title
            GameObject titleObj = CreateTextMeshPro(canvas.transform, "TitleText");
            TMP_Text titleText = titleObj.GetComponent<TMP_Text>();
            titleText.text = "VINTAGE BEEF";
            titleText.fontSize = 72;
            titleText.alignment = TextAlignmentOptions.Center;
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.7f);
            titleRect.anchorMax = new Vector2(0.5f, 0.7f);
            titleRect.sizeDelta = new Vector2(800, 100);

            // Create Play Button
            GameObject playBtn = CreateButton(canvas.transform, "PlayButton", "PLAY");
            RectTransform playRect = playBtn.GetComponent<RectTransform>();
            playRect.anchorMin = new Vector2(0.5f, 0.5f);
            playRect.anchorMax = new Vector2(0.5f, 0.5f);
            playRect.sizeDelta = new Vector2(200, 60);

            // Create Quit Button
            GameObject quitBtn = CreateButton(canvas.transform, "QuitButton", "QUIT");
            RectTransform quitRect = quitBtn.GetComponent<RectTransform>();
            quitRect.anchorMin = new Vector2(0.5f, 0.4f);
            quitRect.anchorMax = new Vector2(0.5f, 0.4f);
            quitRect.sizeDelta = new Vector2(200, 60);

            // Create MenuManager
            GameObject menuManager = new GameObject("MenuManager");
            menuManager.transform.SetParent(canvas.transform);
            MainMenuUI menuUI = menuManager.AddComponent<MainMenuUI>();

            // Use reflection to set private fields
            System.Type type = typeof(MainMenuUI);
            type.GetField("playButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(menuUI, playBtn.GetComponent<Button>());
            type.GetField("quitButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(menuUI, quitBtn.GetComponent<Button>());
            type.GetField("titleText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(menuUI, titleText);

            EditorUtility.SetDirty(menuManager);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("MainMenu scene setup complete!");
        }

        private void SetupLobbyScene()
        {
            Debug.Log("Setting up Lobby scene...");

            // Create Canvas
            GameObject canvas = CreateCanvas();
            if (canvas == null) return;

            // Create Title
            GameObject titleObj = CreateTextMeshPro(canvas.transform, "TitleText");
            TMP_Text titleText = titleObj.GetComponent<TMP_Text>();
            titleText.text = "SELECT YOUR PROFESSION";
            titleText.fontSize = 48;
            titleText.alignment = TextAlignmentOptions.Center;
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.sizeDelta = new Vector2(800, 60);

            // Create Profession Container
            GameObject container = new GameObject("ProfessionContainer");
            container.transform.SetParent(canvas.transform);
            RectTransform containerRect = container.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.5f, 0.5f);
            containerRect.anchorMax = new Vector2(0.5f, 0.5f);
            containerRect.sizeDelta = new Vector2(600, 400);
            
            VerticalLayoutGroup layout = container.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 10;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlHeight = false;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = true;

            ContentSizeFitter fitter = container.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Create Selected Text
            GameObject selectedObj = CreateTextMeshPro(canvas.transform, "SelectedText");
            TMP_Text selectedText = selectedObj.GetComponent<TMP_Text>();
            selectedText.text = "Select a profession...";
            selectedText.fontSize = 24;
            selectedText.alignment = TextAlignmentOptions.Center;
            RectTransform selectedRect = selectedObj.GetComponent<RectTransform>();
            selectedRect.anchorMin = new Vector2(0.5f, 0.25f);
            selectedRect.anchorMax = new Vector2(0.5f, 0.25f);
            selectedRect.sizeDelta = new Vector2(600, 80);

            // Create Start Button
            GameObject startBtn = CreateButton(canvas.transform, "StartGameButton", "START GAME");
            RectTransform startRect = startBtn.GetComponent<RectTransform>();
            startRect.anchorMin = new Vector2(0.5f, 0.1f);
            startRect.anchorMax = new Vector2(0.5f, 0.1f);
            startRect.sizeDelta = new Vector2(200, 60);
            startBtn.GetComponent<Button>().interactable = false;

            // Create LobbyManager
            GameObject lobbyManager = new GameObject("LobbyManager");
            lobbyManager.transform.SetParent(canvas.transform);
            LobbyUI lobbyUI = lobbyManager.AddComponent<LobbyUI>();

            // Use reflection to set private fields
            System.Type type = typeof(LobbyUI);
            type.GetField("titleText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(lobbyUI, titleText);
            type.GetField("professionButtonContainer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(lobbyUI, container.transform);
            type.GetField("startGameButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(lobbyUI, startBtn.GetComponent<Button>());
            type.GetField("selectedProfessionText", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(lobbyUI, selectedText);

            EditorUtility.SetDirty(lobbyManager);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("Lobby scene setup complete!");
        }

        private void SetupGameWorldScene()
        {
            Debug.Log("Setting up GameWorld scene...");

            // Create Environment
            GameObject environment = GameObject.Find("Environment");
            if (environment == null)
            {
                environment = new GameObject("Environment");
            }

            // Create Sun
            GameObject sun = new GameObject("Sun");
            sun.transform.SetParent(environment.transform);
            Light sunLight = sun.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.color = new Color(1f, 0.95f, 0.8f);
            sunLight.intensity = 1f;
            sunLight.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(50, -30, 0);

            // Create Moon
            GameObject moon = new GameObject("Moon");
            moon.transform.SetParent(environment.transform);
            Light moonLight = moon.AddComponent<Light>();
            moonLight.type = LightType.Directional;
            moonLight.color = new Color(0.6f, 0.7f, 1f);
            moonLight.intensity = 0.3f;
            moonLight.shadows = LightShadows.Soft;
            moon.transform.rotation = Quaternion.Euler(-50, -30, 0);
            moon.SetActive(false);

            // Add Day/Night Cycle
            DayNightCycle dayNight = environment.GetComponent<DayNightCycle>();
            if (dayNight == null)
            {
                dayNight = environment.AddComponent<DayNightCycle>();
            }

            // Use reflection to set sun/moon references
            System.Type dnType = typeof(DayNightCycle);
            dnType.GetField("sunLight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(dayNight, sunLight);
            dnType.GetField("moonLight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(dayNight, moonLight);

            EditorUtility.SetDirty(environment);

            // Add Weather System
            if (environment.GetComponent<WeatherSystem>() == null)
            {
                environment.AddComponent<WeatherSystem>();
            }

            // Create TerrainManager
            CreateTerrainManager();

            // Create Canvas
            GameObject canvas = CreateCanvas();
            if (canvas != null)
            {
                // Create Inventory Panel
                GameObject inventoryPanel = new GameObject("InventoryPanel");
                inventoryPanel.transform.SetParent(canvas.transform);
                RectTransform panelRect = inventoryPanel.AddComponent<RectTransform>();
                panelRect.anchorMin = new Vector2(0.5f, 0.5f);
                panelRect.anchorMax = new Vector2(0.5f, 0.5f);
                panelRect.sizeDelta = new Vector2(600, 400);
                
                Image panelImage = inventoryPanel.AddComponent<Image>();
                panelImage.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
                
                GridLayoutGroup grid = inventoryPanel.AddComponent<GridLayoutGroup>();
                grid.cellSize = new Vector2(80, 80);
                grid.spacing = new Vector2(10, 10);
                grid.padding = new RectOffset(10, 10, 10, 10);

                inventoryPanel.SetActive(false);

                // Create Inventory UI Manager
                GameObject inventoryManager = new GameObject("InventoryUIManager");
                inventoryManager.transform.SetParent(canvas.transform);
                InventoryUI invUI = inventoryManager.AddComponent<InventoryUI>();

                // Use reflection to set private fields
                System.Type invType = typeof(InventoryUI);
                invType.GetField("inventoryPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(invUI, inventoryPanel);

                EditorUtility.SetDirty(inventoryManager);
            }

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            Debug.Log("GameWorld scene setup complete!");
        }

        private GameObject CreateCanvas()
        {
            // Check if Canvas already exists
            Canvas existingCanvas = FindObjectOfType<Canvas>();
            if (existingCanvas != null)
            {
                Debug.Log("Canvas already exists in scene.");
                return existingCanvas.gameObject;
            }

            // Create Canvas
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasObj.AddComponent<GraphicRaycaster>();

            // Create EventSystem if it doesn't exist
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }

            Debug.Log("Canvas created!");
            return canvasObj;
        }

        private GameObject CreateTextMeshPro(Transform parent, string name)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent);
            
            RectTransform rect = textObj.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            
            TMP_Text text = textObj.AddComponent<TextMeshProUGUI>();
            text.color = Color.white;
            text.alignment = TextAlignmentOptions.Center;
            
            return textObj;
        }

        private GameObject CreateButton(Transform parent, string name, string buttonText)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent);
            
            RectTransform rect = btnObj.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            
            Image image = btnObj.AddComponent<Image>();
            image.color = Color.white;
            
            Button button = btnObj.AddComponent<Button>();

            // Create text child
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform);
            
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.localScale = Vector3.one;
            
            TMP_Text text = textObj.AddComponent<TextMeshProUGUI>();
            text.text = buttonText;
            text.color = Color.black;
            text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 24;

            return btnObj;
        }

        private void CreateGameManager()
        {
            if (FindObjectOfType<GameManager>() != null)
            {
                Debug.Log("GameManager already exists.");
                return;
            }

            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
            Debug.Log("GameManager created!");
        }

        private void CreateProfessionManager()
        {
            if (FindObjectOfType<ProfessionManager>() != null)
            {
                Debug.Log("ProfessionManager already exists.");
                return;
            }

            GameObject pm = new GameObject("ProfessionManager");
            pm.AddComponent<ProfessionManager>();
            Debug.Log("ProfessionManager created!");
        }

        private void CreatePlayerData()
        {
            if (FindObjectOfType<PlayerData>() != null)
            {
                Debug.Log("PlayerData already exists.");
                return;
            }

            GameObject pd = new GameObject("PlayerData");
            pd.AddComponent<PlayerData>();
            Debug.Log("PlayerData created!");
        }

        private void CreateTerrainManager()
        {
            if (FindObjectOfType<TerrainManager>() != null)
            {
                Debug.Log("TerrainManager already exists.");
                return;
            }

            GameObject tm = new GameObject("TerrainSystem");
            tm.AddComponent<TerrainManager>();
            Debug.Log("TerrainManager created! Set Terrain Type in Inspector.");
        }
    }
}
#endif
