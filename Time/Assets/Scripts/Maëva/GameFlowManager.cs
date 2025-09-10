using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    private GameObject victoryPanel;
    private GameObject defeatPanel;
    private GameObject victoryFinalText;

    private GameObject[] levels;
    private int currentLevelIndex = 0;

    private bool gamePaused = false; // arrête la barre de vie et les actions

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cherche panels dynamiquement
        victoryPanel = GameObject.Find("VictoryPanel");
        defeatPanel = GameObject.Find("DefeatPanel");
        victoryFinalText = GameObject.Find("VictoryFinalText");

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryFinalText != null) victoryFinalText.SetActive(false);

        // Récupère les boutons et lie les fonctions
        if (victoryPanel != null)
        {
            Button next = victoryPanel.transform.Find("Next")?.GetComponent<Button>();
            Button restart = victoryPanel.transform.Find("Restart")?.GetComponent<Button>();
            Button menu = victoryPanel.transform.Find("Menu")?.GetComponent<Button>();

            if (next != null) next.onClick.AddListener(NextLevel);
            if (restart != null) restart.onClick.AddListener(RestartLevel);
            if (menu != null) menu.onClick.AddListener(ReturnToMainMenu);
        }

        if (defeatPanel != null)
        {
            Button restart = defeatPanel.transform.Find("Restart")?.GetComponent<Button>();
            Button menu = defeatPanel.transform.Find("Menu")?.GetComponent<Button>();

            if (restart != null) restart.onClick.AddListener(RestartLevel);
            if (menu != null) menu.onClick.AddListener(ReturnToMainMenu);
        }

        // Cherche les niveaux dans LevelContainer
        GameObject container = GameObject.Find("LevelContainer");
        if (container != null)
        {
            int childCount = container.transform.childCount;
            levels = new GameObject[childCount];
            for (int i = 0; i < childCount; i++)
            {
                levels[i] = container.transform.GetChild(i).gameObject;
            }
        }

        // Active seulement le premier niveau
        ShowLevel(0);
    }

    private void ShowLevel(int index)
    {
        currentLevelIndex = index;

        for (int i = 0; i < levels.Length; i++)
            levels[i].SetActive(i == currentLevelIndex);

        gamePaused = false;
        Time.timeScale = 1f;
    }

    public void PlayerDied()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        if (defeatPanel != null) defeatPanel.SetActive(true);
    }

    public void EnemyDied()
    {
        gamePaused = true;
        Time.timeScale = 0f;

        if (currentLevelIndex == levels.Length - 1)
        {
            if (victoryFinalText != null) victoryFinalText.SetActive(true);
            if (victoryPanel != null) victoryPanel.SetActive(false);
        }
        else
        {
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (victoryFinalText != null) victoryFinalText.SetActive(false);

        int nextIndex = currentLevelIndex + 1;
        if (nextIndex < levels.Length) ShowLevel(nextIndex);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsGamePaused() => gamePaused;
}
