using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    private AudioSource sfxAudioSource;     // pour victoire/défaite
    private AudioSource musicAudioSource;   // pour la musique de fond

    private AudioClip victoryClip;
    private AudioClip defeatClip;
    private AudioClip backgroundClip;

    [Header("Volumes réglables dans l'Inspector")]
    [Range(0f, 2f)] public float victoryVolume = 1f;
    [Range(0f, 2f)] public float defeatVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 0.5f; // volume de fond réglable
    
    private bool victoryPlayed = false;
    private bool defeatPlayed = false;

    private void Awake()
    {
        // Ajoute 2 AudioSources : un pour la musique, un pour les SFX
        sfxAudioSource = gameObject.AddComponent<AudioSource>();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true; // musique en boucle
        musicAudioSource.playOnAwake = false;

        // Charge automatiquement les sons depuis Resources/Audio/
        victoryClip = Resources.Load<AudioClip>("Victory");
        defeatClip = Resources.Load<AudioClip>("Defeat");
        backgroundClip = Resources.Load<AudioClip>("BackgroundCombat"); // nom du fichier musique

        // Lance la musique de fond
        if (backgroundClip != null)
        {
            musicAudioSource.clip = backgroundClip;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Pas de fichier 'Background' trouvé dans Resources/Audio/");
        }

        DontDestroyOnLoad(gameObject); // Persiste entre les scènes
    }

    private void Update()
    {
        if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGamePaused())
        {
            // Pause la musique si victoire ou défaite
            if ((GameFlowManager.Instance.victoryPanel != null && GameFlowManager.Instance.victoryPanel.activeSelf) ||
                (GameFlowManager.Instance.defeatPanel != null && GameFlowManager.Instance.defeatPanel.activeSelf))
            {
                if (musicAudioSource.isPlaying) musicAudioSource.Pause();

                // Victoire
                if (GameFlowManager.Instance.victoryPanel != null &&
                    GameFlowManager.Instance.victoryPanel.activeSelf &&
                    !victoryPlayed)
                {
                    PlaySFX(victoryClip, victoryVolume);
                    victoryPlayed = true; // joue une fois
                }

                // Défaite
                if (GameFlowManager.Instance.defeatPanel != null &&
                    GameFlowManager.Instance.defeatPanel.activeSelf &&
                    !defeatPlayed)
                {
                    PlaySFX(defeatClip, defeatVolume);
                    defeatPlayed = true; // joue une fois
                }
            }
        }
        else
        {
            // Si le jeu reprend -> relancer la musique si elle est en pause
            if (!musicAudioSource.isPlaying && backgroundClip != null)
            {
                musicAudioSource.UnPause();
            }
        }
    }

    private void PlaySFX(AudioClip clip, float volumeScale)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip, volumeScale);
        }
    }

    // Méthode publique à appeler pour réinitialiser les flags lors d'un restart
    public void ResetFlags()
    {
        victoryPlayed = false;
        defeatPlayed = false;
    }
}

