using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // important pour les coroutines

public class MainMenuController : MonoBehaviour
{
    private Button startButton;
    private Button settingButton;
    private Button quitButton;

    private GameObject transitionPanel;
    private float transitionTime = 5f;

    private AudioSource audioSource; // le splash
    private AudioSource musicAudioSource; // musique de fond

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

        // Musique de fond
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        AudioClip bgMusic = Resources.Load<AudioClip>("MainSound");
        if (bgMusic != null)
        {
            musicAudioSource.clip = bgMusic;
            musicAudioSource.loop = true; // musique en boucle
            musicAudioSource.playOnAwake = false;
            musicAudioSource.volume = 1f; // volume de départ
            musicAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Pas de fichier 'MainSound' trouvé dans Resources !");
        }

        // Audio transition le splash
        audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("Splash");
        audioSource.clip = clip;

        // Bubble
        bubblePrefab = transitionPanel != null ? transitionPanel.transform.Find("BubblePrefab")?.gameObject : null;
        if (bubblePrefab != null)
            bubblePrefab.SetActive(false);
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

        // Lancer le fade de la musique
        StartCoroutine(FadeOutMusic(transitionTime));

        // Charger la scène après le temps de transition
        Invoke("LoadCombat", transitionTime);
    }

    private void LoadCombat()
    {
        SceneManager.LoadScene("Combat");
    }

    private void OnSettingClicked()
    {
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

    // Coroutine pour réduire le volume progressivement
    private IEnumerator FadeOutMusic(float duration)
    {
        if (musicAudioSource == null) yield break;

        float startVolume = musicAudioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();
    }
}
