using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameFlowManagerOld : MonoBehaviour
{
    // public event System.Action OnLevelChanged;
    public static GameFlowManagerOld Instance { get; private set; }

    // Références privées pour les panneaux UI
    // Elles seront trouvées dynamiquement, pas besoin de les assigner dans l'Inspecteur
    // private GameObject victoryPanel;
    // private GameObject defeatPanel;
    // private GameObject victoryFinalText;

    // Rendre les références publiques ou sérialisées pour l'éditeur
    [Header("Panels")]
    [Tooltip("Le panneau de victoire à assigner dans l'éditeur")]
    public GameObject victoryPanel;
    [Tooltip("Le panneau de défaite à assigner dans l'éditeur")]
    public GameObject defeatPanel;
    [Tooltip("Le texte final de victoire à assigner dans l'éditeur")]
    public GameObject victoryFinalText;

    // Un tableau pour stocker les références des scènes des niveaux
    [Header("Spawn Points")]
    [Tooltip("Position où les ennemis doivent apparaître dans la scène")]
    public Transform enemySpawnPoint;
    [Tooltip("Position où les décors doivent apparaître dans la scène")]
    [SerializeField] private Transform decorSpawnPoint;

    private int currentLevelIndex = 0;
    private bool gamePaused = false; // arr�te la barre de vie et les actions

    // Evénement pour notifier les autres scripts quand un niveau change
    public event System.Action OnLevelChanged;

    private GameObject currentEnemy; // Référence de l'ennemi actif
    private GameObject currentDecor; // Décor actif
    private void Awake()
    {
        // Implémentation du Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        // Référencement dynamique des GameObjects de l'UI et des niveaux
        FindUiPanels();
        // FindLevels();

        // -- CODE MAEVA --

        // // Cherche panels dynamiquement
        // victoryPanel = GameObject.Find("VictoryPanel");
        // defeatPanel = GameObject.Find("DefeatPanel");
        // victoryFinalText = GameObject.Find("VictoryFinalText");

        // if (victoryPanel != null) victoryPanel.SetActive(false);
        // if (defeatPanel != null) defeatPanel.SetActive(false);
        // if (victoryFinalText != null) victoryFinalText.SetActive(false);

        // // R�cup�re les boutons et lie les fonctions
        // if (victoryPanel != null)
        // {
        //     Button next = victoryPanel.transform.Find("Next")?.GetComponent<Button>();
        //     Button restart = victoryPanel.transform.Find("Restart")?.GetComponent<Button>();
        //     Button menu = victoryPanel.transform.Find("Menu")?.GetComponent<Button>();

        //     if (next != null) next.onClick.AddListener(NextLevel);
        //     if (restart != null) restart.onClick.AddListener(RestartLevel);
        //     if (menu != null) menu.onClick.AddListener(ReturnToMainMenu);
        // }

        // if (defeatPanel != null)
        // {
        //     Button restart = defeatPanel.transform.Find("Restart")?.GetComponent<Button>();
        //     Button menu = defeatPanel.transform.Find("Menu")?.GetComponent<Button>();

        //     if (restart != null) restart.onClick.AddListener(RestartLevel);
        //     if (menu != null) menu.onClick.AddListener(ReturnToMainMenu);
        // }

        // // Cherche les niveaux dans LevelContainer
        // GameObject container = GameObject.Find("LevelContainer");
        // if (container != null)
        // {
        //     int childCount = container.transform.childCount;
        //     levels = new GameObject[childCount];
        //     for (int i = 0; i < childCount; i++)
        //     {
        //         levels[i] = container.transform.GetChild(i).gameObject;
        //     }
        // }

        // // Active seulement le premier niveau
        // ShowLevel(0);

        // --- FIN CODE MAEVA ---

        // On s'assure que les panneaux sont désactivés au démarrage
        SetPanelState(false);

        // On assigne les listeners aux boutons
        AssignButtonListeners();
    }
    
    private void Start()
    {
        // On s'assure que les panels sont désactivés au démarrage
        SetPanelState(false);
        ShowLevel(0);
    }

    /// <summary>
    /// Recherche et stocke les références des panneaux d'UI dans la scène.
    /// </summary>
    private void FindUiPanels()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("GameFlowManager: Aucun Canvas trouvé dans la scène.");
            return;
        }

        // Recherche récursive dans tous les enfants du Canvas
        foreach (Transform t in canvas.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "VictoryPanel") victoryPanel = t.gameObject;
            if (t.name == "DefeatPanel") defeatPanel = t.gameObject;
            if (t.name == "VictoryFinalText") victoryFinalText = t.gameObject;
        }

        if (victoryPanel == null || defeatPanel == null)
        {
            Debug.LogWarning("GameFlowManager: Impossible de trouver VictoryPanel ou DefeatPanel dans la scène.");
        }
    }


    /// <summary>
    /// Recherche tous les niveaux sous l'objet parent 'LevelContainer'.
    /// </summary>
    // private void FindLevels()
    // {
    //     GameObject container = GameObject.Find("LevelContainer");
    //     if (container != null)
    //     {
    //         levels = new GameObject[container.transform.childCount];
    //         for (int i = 0; i < container.transform.childCount; i++)
    //         {
    //             levels[i] = container.transform.GetChild(i).gameObject;
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("GameFlowManager: L'objet 'LevelContainer' est introuvable. Les niveaux ne peuvent pas être chargés.");
    //     }
    // }

    /// <summary>
    /// Lie les fonctions de gestion aux boutons des panneaux.
    /// </summary>
    private void AssignButtonListeners()
    {
        // Boutons du VictoryPanel
        if (victoryPanel != null)
        {
            victoryPanel.transform.Find("Next")?.GetComponent<Button>()?.onClick.AddListener(NextLevel);
            victoryPanel.transform.Find("Restart")?.GetComponent<Button>()?.onClick.AddListener(RestartLevel);
            victoryPanel.transform.Find("Menu")?.GetComponent<Button>()?.onClick.AddListener(ReturnToMainMenu);
        }

        // Boutons du DefeatPanel
        if (defeatPanel != null)
        {
            defeatPanel.transform.Find("Restart")?.GetComponent<Button>()?.onClick.AddListener(RestartLevel);
            defeatPanel.transform.Find("Menu")?.GetComponent<Button>()?.onClick.AddListener(ReturnToMainMenu);
        }
    }

    /// <summary>
    /// Active ou désactive l'ensemble des panneaux UI de fin de partie.
    /// </summary>
    private void SetPanelState(bool state)
    {
        if (victoryPanel != null) victoryPanel.SetActive(state);
        if (defeatPanel != null) defeatPanel.SetActive(state);
        if (victoryFinalText != null) victoryFinalText.SetActive(state);
    }


    /// <summary>
    /// Gère la mort du joueur et l'affichage du panneau de défaite.
    /// </summary>
    public void PlayerDied()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        if (defeatPanel != null) defeatPanel.SetActive(true);
    }

    /// <summary>
    /// Gère la mort de l'ennemi et l'affichage du panneau de victoire.
    /// </summary>
    public void EnemyDied()
    {
        gamePaused = true;
        Time.timeScale = 0f;

        if (currentLevelIndex >= 2) // Dernier niveau (3e, index = 2)
        {
            // Fin du jeu
            if (victoryFinalText != null) victoryFinalText.SetActive(true);
            // if (victoryPanel != null) victoryPanel.SetActive(false);
        }
        else
        {
            // Niveau suivant
            if (victoryPanel != null) victoryPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Passe au niveau suivant du tableau 'levels'.
    /// </summary>
    public void NextLevel()
    {
        // if (victoryPanel != null) victoryPanel.SetActive(false);
        // if (victoryFinalText != null) victoryFinalText.SetActive(false);

        // int nextIndex = currentLevelIndex + 1;
        // if (nextIndex < levels.Length) ShowLevel(nextIndex);
        SetPanelState(false);

        int nextIndex = currentLevelIndex + 1;
        if (nextIndex <= 2) // car 3 niveaux : 0, 1, 2
        {
            ShowLevel(nextIndex);
        }
    }

    /// <summary>
    /// Recharge la scène actuelle pour un redémarrage.
    /// </summary>
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Charge la scène du menu principal.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Affiche le niveau spécifié et notifie les autres scripts du changement.
    /// </summary>
    private void ShowLevel(int index)
    {

        currentLevelIndex = index;

        // Charger décor
        string decorName = $"Prefabs/Level{index + 1}";
        GameObject decorPrefab = Resources.Load<GameObject>(decorName);
        if (decorPrefab != null)
        {
            Vector3 pos = decorSpawnPoint != null ? decorSpawnPoint.position : Vector3.zero;
            currentDecor = Instantiate(decorPrefab, pos, Quaternion.identity); 
        }

        // Supprime le décor précédent
        if (currentDecor != null)
        {
            Destroy(currentDecor);
        }

        // Supprime l'ancien ennemi si encore présent
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
        }
        

        // Choisit le prefab  de l'ennemi en fonction du niveau
        string prefabName = "";
        switch (index)
        {
            case 0: prefabName = "Prefabs/MedusaEnemy"; break;
            case 1: prefabName = "Prefabs/SharkEnemy"; break;
            case 2: prefabName = "Prefabs/SeaSerpentEnemy"; break;
        }

        GameObject enemyPrefab = Resources.Load<GameObject>(prefabName);
        if (enemyPrefab != null)
        {
            Vector3 spawnPos = enemySpawnPoint != null ? enemySpawnPoint.position : Vector3.zero;
            currentEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Impossible de charger le prefab : " + prefabName);
        }

        gamePaused = false;
        Time.timeScale = 1f;

        // On notifie les autres scripts que le niveau a changé
        OnLevelChanged?.Invoke();
    }

    // public void ActionDamage(InputAction.CallbackContext context) 
    // {
    //     if (context.performed)
    //     {
    //         Health.Instance.TakeDamage(1);
    //     }
    // }

    /// <summary>
    /// Indique si le jeu est en pause.
    /// </summary>
    public bool IsGamePaused() => gamePaused;
}
