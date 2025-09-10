using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Button startButton;
    private Button settingButton;
    private Button quitButton;

    private void Awake()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null) { Debug.LogError("Canvas introuvable !"); return; }

        startButton = canvas.transform.Find("HomeScreen/Start")?.GetComponent<Button>();
        settingButton = canvas.transform.Find("HomeScreen/Setting")?.GetComponent<Button>();
        quitButton = canvas.transform.Find("HomeScreen/Quit")?.GetComponent<Button>();

        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);
        if (settingButton != null) settingButton.onClick.AddListener(OnSettingClicked);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnStartClicked()
    {
        SceneManager.LoadScene("Level1");
    }

    private void OnSettingClicked()
    {
        // Ouvre directement le SettingScreen
        PauseMenuController pauseController = GameObject.FindFirstObjectByType<PauseMenuController>();
        if (pauseController != null)
        {
            pauseController.OpenPauseMenu(() => pauseController.OpenSettings());
        }
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
