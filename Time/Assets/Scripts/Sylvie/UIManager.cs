using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Instance unique pour le pattern Singleton
    public static UIManager Instance { get; private set; }

    // Utilisation de la référence privée pour stocker le Slider
    private Slider playerHealthSlider;

    // Ajout d'une référence au nom du préfabriqué dans le dossier Resources
    [Header("HealthBar Prefab")]
    [Tooltip("Nom du prefab de la barre de vie dans le dossier Resources")]
    [SerializeField] private string healthBarPrefabName = "UI/HealthBarPlayer";

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

            // On appelle la méthode de chargement et d'initialisation ici
            LoadAndInstantiateHealthBar();
        }
    }

    private void LoadAndInstantiateHealthBar()
    {
        // Utilisation de string.IsNullOrEmpty pour vérifier si le nom a été assigné
        if (string.IsNullOrEmpty(healthBarPrefabName))
        {
            Debug.LogError("UIManager: Le nom du préfabriqué de la barre de vie n'est pas assigné !");
            return;
        }

        // Charger le préfabriqué depuis le dossier Resources
        GameObject healthBarPrefab = Resources.Load<GameObject>(healthBarPrefabName);

        // 3. VÉRIFIER L'OBJET APRÈS LE CHARGEMENT. C'est le point crucial qui manquait.
        if (healthBarPrefab == null)
        {
            Debug.LogError($"UIManager: Le préfabriqué '{healthBarPrefabName}' n'a pas été trouvé dans le dossier Resources. Vérifiez le nom et l'emplacement.");
            return; // Arrêter l'exécution si l'objet n'est pas trouvé
        }

        // Vérifier si l'object a bien été chargé
        if (healthBarPrefabName == null)
        {
            Debug.LogError($"UIManager: Le préfabriqué '{healthBarPrefabName}' n'a pas été dans le dossier Resources.");
            return;
        }

        // 2. Instancier le préfabriqué dans la scène
        GameObject healthBarInstance = Instantiate(healthBarPrefab);

        // 3. Récupérer le composant Slider de l'enfant
        playerHealthSlider = healthBarInstance.GetComponentInChildren<Slider>();

        if (playerHealthSlider == null)
        {
            Debug.LogError($"UIManager: Aucun composant Slider n'a été trouvé dans le préfabriqué");
        }
        
        // On donne la référence au script HealthbarPlayer s’il est dessus
        HealthbarPlayer healthScript = healthBarInstance.GetComponent<HealthbarPlayer>();
        if (healthScript != null)
        {
            healthScript.Initialize(playerHealthSlider);
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