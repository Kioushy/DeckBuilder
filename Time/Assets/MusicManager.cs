using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip victoryClip;
    private AudioClip defeatClip;

    [Header("Volumes réglables dans l'Inspector")]
    [Range(0f, 2f)] public float victoryVolume = 1f;
    [Range(0f, 2f)] public float defeatVolume = 1f;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        // Charge automatiquement les sons depuis Resources/Audio/
        victoryClip = Resources.Load<AudioClip>("Victory");
        defeatClip = Resources.Load<AudioClip>("Defeat");

        DontDestroyOnLoad(gameObject); // Persiste entre les scènes
    }

    private void Update()
    {
        if (GameFlowManager.Instance != null && GameFlowManager.Instance.IsGamePaused())
        {
            // Victoire
            if (GameFlowManager.Instance.victoryPanel != null &&
                GameFlowManager.Instance.victoryPanel.activeSelf &&
                !audioSource.isPlaying)
            {
                PlaySound(victoryClip, victoryVolume);
            }

            // Défaite
            if (GameFlowManager.Instance.defeatPanel != null &&
                GameFlowManager.Instance.defeatPanel.activeSelf &&
                !audioSource.isPlaying)
            {
                PlaySound(defeatClip, defeatVolume);
            }
        }
    }

    private void PlaySound(AudioClip clip, float volumeScale)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}
