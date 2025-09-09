using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    private static bool isPaused = false;
    private GameObject pausePanel;
    private GameObject settingsPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) OpenPauseMenu();
            else ResumeGame();
        }
    }

    void OpenPauseMenu()
    {
        isPaused = true;
        Time.timeScale = 0f;

        // Charger la scène du menu pause en Additive
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive).completed += (op) =>
        {
            Scene pauseScene = SceneManager.GetSceneByName("PauseMenu");
            if (pauseScene.isLoaded)
            {
                GameObject[] roots = pauseScene.GetRootGameObjects();

                foreach (GameObject go in roots)
                {
                    // Récupérer les panels
                    pausePanel = go.transform.Find("MenuPause Panel")?.gameObject;
                    settingsPanel = go.transform.Find("SettingScreen")?.gameObject;

                    if (pausePanel != null) pausePanel.SetActive(true);
                    if (settingsPanel != null) settingsPanel.SetActive(false);

                    // Boutons du menu principal
                    go.transform.Find("MenuPause Panel/Resume")?.GetComponent<Button>()
                        .onClick.AddListener(ResumeGame);

                    go.transform.Find("MenuPause Panel/Setting")?.GetComponent<Button>()
                        .onClick.AddListener(OpenSettings);

                    go.transform.Find("MenuPause Panel/Menu")?.GetComponent<Button>()
                        .onClick.AddListener(ReturnToMainMenu);

                    // Boutons du menu Settings
                    go.transform.Find("SettingScreen/Exit")?.GetComponent<Button>()
                        .onClick.AddListener(BackToPauseMenu);

                    // Dropdown Résolution
                    Dropdown resDropdown = go.transform.Find("SettingScreen/Resolution/Dropdown")
                        ?.GetComponent<Dropdown>();
                    if (resDropdown != null)
                    {
                        resDropdown.onValueChanged.AddListener(ChangeResolution);
                    }

                    // Slider Volume
                    Slider volumeSlider = go.transform.Find("SettingScreen/volume/Slider")
                        ?.GetComponent<Slider>();
                    if (volumeSlider != null)
                    {
                        volumeSlider.onValueChanged.AddListener(ChangeVolume);
                    }

                    // Toggle Fullscreen
                    Toggle fullscreenToggle = go.transform.Find("SettingScreen/fullscreen/Toggle")
                        ?.GetComponent<Toggle>();
                    if (fullscreenToggle != null)
                    {
                        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
                    }
                }
            }
        };
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.UnloadSceneAsync("PauseMenu");
    }

    void OpenSettings()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    void BackToPauseMenu()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu"); // quand tu auras ta scène principale
    }

    // ---------- Paramètres ----------
    void ChangeResolution(int index)
    {
        Debug.Log("Changer résolution : " + index);
        // TODO: appliquer Screen.SetResolution selon options
    }

    void ChangeVolume(float value)
    {
        Debug.Log("Changer volume : " + value);
        AudioListener.volume = value;
    }

    void SetFullscreen(bool isFullscreen)
    {
        Debug.Log("Fullscreen : " + isFullscreen);
        Screen.fullScreen = isFullscreen;
    }
}
