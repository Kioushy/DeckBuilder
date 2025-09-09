using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PauseMenuController : MonoBehaviour
{
    public static bool isPaused = false;

    private GameObject pausePanel;
    private GameObject settingsPanel;

    private Button resumeButton;
    private Button settingButton;
    private Button menuButton;
    private Button settingExitButton;

    private Dropdown resolutionDropdown;
    private Slider volumeSlider;
    private Toggle fullscreenToggle;

    private void Awake()
    {
        // Persiste entre les scènes
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) OpenPauseMenu();
            else ResumeGame();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    // Callback optionnel pour actions après chargement
    public void OpenPauseMenu(Action onReady = null)
    {
        if (!SceneManager.GetSceneByName("PauseMenu").isLoaded)
        {
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive).completed += (op) =>
            {
                SetupPauseMenu();
                onReady?.Invoke();
            };
        }
        else
        {
            SetupPauseMenu();
            onReady?.Invoke();
        }
    }

    public void SetupPauseMenu()
    {
        Scene pauseScene = SceneManager.GetSceneByName("PauseMenu");
        if (!pauseScene.isLoaded) return;

        GameObject[] roots = pauseScene.GetRootGameObjects();
        foreach (GameObject root in roots)
        {
            // Panels
            pausePanel = root.transform.Find("MenuPause Panel")?.gameObject;
            settingsPanel = root.transform.Find("SettingScreen")?.gameObject;

            if (pausePanel != null) pausePanel.SetActive(true);
            if (settingsPanel != null) settingsPanel.SetActive(false);

            // Boutons MenuPause
            resumeButton = root.transform.Find("MenuPause Panel/Resume")?.GetComponent<Button>();
            settingButton = root.transform.Find("MenuPause Panel/Setting")?.GetComponent<Button>();
            menuButton = root.transform.Find("MenuPause Panel/Menu")?.GetComponent<Button>();

            resumeButton?.onClick.RemoveAllListeners();
            resumeButton?.onClick.AddListener(ResumeGame);

            settingButton?.onClick.RemoveAllListeners();
            settingButton?.onClick.AddListener(OpenSettings);

            menuButton?.onClick.RemoveAllListeners();
            menuButton?.onClick.AddListener(ReturnToMainMenu);

            // Bouton Setting Exit
            settingExitButton = root.transform.Find("SettingScreen/Exit")?.GetComponent<Button>();
            settingExitButton?.onClick.RemoveAllListeners();
            settingExitButton?.onClick.AddListener(BackToPauseMenu);

            // Dropdown / Slider / Toggle
            resolutionDropdown = root.transform.Find("SettingScreen/Resolution/Dropdown")?.GetComponent<Dropdown>();
            if (resolutionDropdown != null)
            {
                resolutionDropdown.onValueChanged.RemoveAllListeners();
                resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
            }

            volumeSlider = root.transform.Find("SettingScreen/volume/Slider")?.GetComponent<Slider>();
            if (volumeSlider != null)
            {
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(ChangeVolume);
            }

            fullscreenToggle = root.transform.Find("SettingScreen/fullscreen/Toggle")?.GetComponent<Toggle>();
            if (fullscreenToggle != null)
            {
                fullscreenToggle.onValueChanged.RemoveAllListeners();
                fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
            }
        }

        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (SceneManager.GetSceneByName("PauseMenu").isLoaded)
            SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void OpenSettings()
    {
        if (pausePanel == null || settingsPanel == null) SetupPauseMenu();
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    // ---------- Paramètres ----------
    private void ChangeResolution(int index) { Debug.Log("Changer résolution : " + index); }
    private void ChangeVolume(float value) { AudioListener.volume = value; }
    private void SetFullscreen(bool isFullscreen) { Screen.fullScreen = isFullscreen; }
}
