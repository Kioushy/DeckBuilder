using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Instance unique pour le pattern Singleton
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("Assigner la barre de vie du joueur ici")]
    [SerializeField] private Slider playerHealthSlider;

    private void Awake()
    {
        // Implémentation du Singleton pour s'assurer qu'il n'y a qu'une seule instance
        if (Instance != null & Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            // Assure que l'objet n'est pas détruit au chargement d'une nouvelle scène
            DontDestroyOnLoad(this.gameObject);
        }

        // Vérification des références essentielles
        if (playerHealthSlider == null)
        {
            Debug.LogError("UIManager: Le Slider de la barre de vie n'est pas assigné dans l'inspecteur");
        }
    }

    /// <summary>
    /// Fournit la référence de la barre de vie du joueur
    /// </summary>
    public Slider GetPlayerHealthSlider()
    {
        return playerHealthSlider;
    }
}