using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Gère le flux principal du jeu : chargement des niveaux, état de victoire/défaite
/// et persistance de la progression entre les scènes
/// Utilise un modèle de Singleton persistant
/// </summary>
public class GameFlowManager : MonoBehaviour
{
    // --- Singleton Instance ---
    public static GameFlowManager Instance { get; private set; }

    // --- State Management --
    // Rendre l'index de niveau 'static' le fait persister à travers les recharges
    // Il appartient maintenant à la classe, pas à une instance de l'objet
    private static int currentLevelIndex = 0;
    private bool gamePaused = false;


    // --- Scene References (trouvées dynamiquement)
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    private GameObject victoryFinalText;
    private Transform enemySpawnPoint;
    private Transform decorSpawnPoint;


    // --- Runtime Objects ---
    private GameObject currentEnemy;
    private GameObject currentDecor;

    // --- Events ---
    public event System.Action OnLevelChanged;

    public Health healthP;
    public Health healthE;

    public IntroManager introManager;

    public Enemies enemies;

    #region Unity Lifecycle Methods
    private void Awake()
    {
        // Logique du Singleton Persistant
        if (Instance != null && Instance != this)
        {
            // S'il existe déjà une instance (celle qui persiste), on détruit ce nouvel objet.
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Empêche l'objet d'être détruit lors du chargement d'une scène
        DontDestroyOnLoad(gameObject);

        // S'abonner à l'événement de chargement de scène pour initialiser chaque niveau.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Toujours se désabonner des événements pour éviter les fuites de mémoire.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    #endregion

    #region Scene and Level Management

    /// <summary>
    /// Méthode appelée automatiquement par Unity après le chargement d'une scène.
    /// C'est le point d'entrée pour toute l'initialisation d'un niveau.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // On ne veut exécuter la logique que dans la scène de jeu principale.
        if (scene.name != "MainMenu")
        {
            InitializeLevel();
        }

    }

    /// <summary>
    /// Orchestre toutes les étapes d'initialisation d'un niveau.
    /// </summary>
    private void InitializeLevel()
    {
        // La recherche des objets de la scène doit se faire ici, car ils n'existent qu'après le chargement.
        FindSceneReferences();
        AssignButtonListeners();
        SetPanelState(false);
        //   ShowLevel(currentLevelIndex);
    }

    /// <summary>
    /// Charge les prefabs du décor et de l'ennemi pour le niveau spécifié.
    /// </summary>
    private void ShowLevel(int index)
    {
        currentLevelIndex = index;


        // --- Charger le décor ---
        string decorName = $"Prefabs/Level{index + 1}";
        GameObject decorPrefab = Resources.Load<GameObject>(decorName);
        if (decorPrefab != null)
        {
            currentDecor = Instantiate(decorPrefab, decorSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Décor introuvable : " + decorName);
        }

        // Réinitialiser la vie du joueur
        HealthBarPlayer healthBar = Object.FindFirstObjectByType<HealthBarPlayer>();
        if (healthBar != null)
        {
            healthBar.ResetHealth();
        }

        // Recentrer la caméra sur le nouvel ennemi ou décor
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null && currentEnemy != null)
        {
            camFollow.SetTarget();
        }

        // Lancer l’intro à chaque nouveau niveau


        /*
            // --- NOUVELLE Logique de chargement de l'ennemi ---
            string enemyPrefabPath = "";
            string enemyDataPath = "";

            switch (index)
            {
                case 0:
                    enemyPrefabPath = "Prefabs/MedusaEnemy";
                    enemyDataPath = "Data/MedusaData"; // Assurez-vous que vos SO sont dans "Resources/Data"
                    break;
                case 1:
                    enemyPrefabPath = "Prefabs/SharkEnemy";
                    enemyDataPath = "Data/SharkData";
                    break;
                case 2:
                    enemyPrefabPath = "Prefabs/SeaSerpentEnemy";
                    enemyDataPath = "Data/SeaSerpentData";
                    break;
            }

            if (!string.IsNullOrEmpty(enemyPrefabPath) && !string.IsNullOrEmpty(enemyDataPath))
            {
                // 1. Charger les données depuis le Scriptable Object
                EnemyData data = Resources.Load<EnemyData>(enemyDataPath);

                // 2. Charger le préfabriqué
                GameObject enemyPrefab = Resources.Load<GameObject>(enemyPrefabPath);

                if (data != null && enemyPrefab != null)
                {
                    // 3. Instancier l'ennemi
                    currentEnemy = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity);

                    // 4. Récupérer son composant Health
                    Health healthComponent = currentEnemy.GetComponent<Health>();
                    if (healthComponent != null)
                    {
                        health = healthComponent; // stocker la référence pour le cheat code
                    }

                }
                else
                {
                    Debug.LogError($"Impossible de charger les données ({enemyDataPath}) ou le préfabriqué ({enemyPrefabPath})");
                }
            }
            */

        // --- Finaliser l'initialisation ---
        gamePaused = false;
        Time.timeScale = 1f;
        OnLevelChanged?.Invoke();
    }

    #endregion

    #region GameFlow

    // Unifie la logique de passage au niveau suivant
    public void NextLevel()
    {
        Time.timeScale = 1f;
        introManager.StartLevelIntro();
    }

    // Redémarre la scène
    public void RestartLevel()
    {
     
        // SetPanelState(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        introManager.StartLevelIntro();
    }

    // Retour au menu principal
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        currentLevelIndex = 0; // Réinitialiser le niveau quand on retourne au menu.
        SceneManager.LoadScene("MainMenu");
    }

    // Quand le joueur meurt
    public void DefeatPanel()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        if (defeatPanel != null) defeatPanel.SetActive(true);
    }

    public void VictoryPanel() 
    {
        Time.timeScale = 0f;
        victoryPanel.SetActive(true);
    }


    public bool IsGamePaused() => gamePaused;

    #endregion

    #region Initialization Helpers

    /// <summary>
    /// Trouve tous les objets nécessaires dans la scène chargée.
    /// Crée les points de spawn s'ils n'existent pas.
    /// </summary>
    private void FindSceneReferences()
    {
        Canvas canvas = UnityEngine.Object.FindAnyObjectByType<Canvas>();
        if (canvas != null)
        {
            //  victoryPanel = canvas.transform.Find("VictoryPanel")?.gameObject;
            //  defeatPanel = canvas.transform.Find("DefeatPanel")?.gameObject;
            //  victoryFinalText = canvas.transform.Find("VictoryFinalText")?.gameObject;
        }
        else
        {
            Debug.LogError("Canvas introuvable dans la scène !");
        }

        // Utilisation d'une fonction locale pour éviter la répétition de code
        Transform FindOrCreateSpawnPoint(string name)
        {
            GameObject sp = GameObject.Find(name);
            if (sp == null)
            {
                sp = new GameObject(name);
                sp.transform.position = Vector3.zero;
            }
            return sp.transform;
        }

        enemySpawnPoint = FindOrCreateSpawnPoint("EnemySpawnPoint");
        decorSpawnPoint = FindOrCreateSpawnPoint("DecorSpawnPoint");

    }

    /// <summary>
    /// Assigne les listeners aux boutons de l'UI.
    /// </summary>
    private void AssignButtonListeners()
    {
        if (victoryPanel != null)
        {
            victoryPanel.transform.Find("Next")?.GetComponent<Button>()?.onClick.AddListener(NextLevel);
            victoryPanel.transform.Find("Restart")?.GetComponent<Button>()?.onClick.AddListener(RestartLevel);
            victoryPanel.transform.Find("Menu")?.GetComponent<Button>()?.onClick.AddListener(ReturnToMainMenu);
        }

        if (defeatPanel != null)
        {
            defeatPanel.transform.Find("Restart")?.GetComponent<Button>()?.onClick.AddListener(RestartLevel);
            defeatPanel.transform.Find("Menu")?.GetComponent<Button>()?.onClick.AddListener(ReturnToMainMenu);
        }
    }

    /// <summary>
    /// Active ou désactive tous les panneaux de l'UI.
    /// </summary>
    private void SetPanelState(bool state)
    {
        if (victoryPanel != null) victoryPanel.SetActive(state);
        if (defeatPanel != null) defeatPanel.SetActive(state);
        if (victoryFinalText != null) victoryFinalText.SetActive(state);
    }

    #endregion

    #region Debug

    // Debug damage avec la touche input
    // public void ActionDamage(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         health.TakeDamage(-1); // inflige 1 dégât
    //         Debug.Log("Cheat Code: dégâts appliqués à l'ennemi !");
    //     }
    // }

    // Pour charger le niveau suivant
    // public void LoadNextLevel()
    // {
    //     currentLevelIndex++;
    //     // Now you load the same scene, and the Start() method will handle loading the correct enemy
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

    #endregion


    // Change camera position
}