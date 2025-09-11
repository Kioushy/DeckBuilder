using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Button startButton;
    private Button settingButton;
    private Button quitButton;

    private GameObject transitionPanel;
    private float transitionTime = 5f;

    private AudioSource audioSource;

    private GameObject bubblePrefab;


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

        // Panel de Transition
        transitionPanel = canvas.transform.Find("HomeScreen/TransitionPanel")?.gameObject;
        if (transitionPanel != null)
            transitionPanel.SetActive(false);
        else
            Debug.LogWarning("TransitionPanel introuvable !");

        // Audio
        audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Splash");
        audioSource.clip = clip;

        // Bubble
        bubblePrefab = transitionPanel != null ? transitionPanel.transform.Find("BubblePrefab")?.gameObject : null;
        if (bubblePrefab != null)
            bubblePrefab.SetActive(false );
        else
            Debug.LogWarning("BubblePrefab introuvable !");
    }

    private void OnStartClicked()
    {
        if (transitionPanel != null)
        {  
            transitionPanel.SetActive(true);  
          
        // Jouer le son
        if (audioSource.clip != null)
            audioSource.Play();

        // Générer les bulles
            BubbleEffect bubbles = transitionPanel.AddComponent<BubbleEffect>();
            bubbles.bubblePrefab = bubblePrefab;
            bubbles.bubbleCount = 20;
        }

        Invoke("LoadLevel1", transitionTime);

    }

    private void LoadLevel1()

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
