using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static bool open = false;
    public Button start;
    public Button settings;
    public Button quit;
    private static MenuController instance = null;
    public static MenuController Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (start != null) start.onClick.AddListener(StartGame);
        if (settings != null) settings.onClick.AddListener(SceneLoad);
        if (quit != null) quit.onClick.AddListener(Quit);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoad();
        }
    }

    void SceneLoad()
    {
        open = !open;

        if (open)
        {
            Time.timeScale = 0f;
            SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync("PauseMenu");
            Time.timeScale = 1f;
        }
    }
    void StartGame()
    {
        SceneManager.LoadSceneAsync("Intro");
    }

    public void Resume()
    {
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1f;
    }

    public void Menu()
    {
        SceneManager.LoadSceneAsync("MainMenu");

    }

    void Quit()
    {
        Application.Quit();
    }
}