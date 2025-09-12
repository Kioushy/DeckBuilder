using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class MedusaEnemy : MonoBehaviour
{

    [Header("Stats")]
    public int maxHealth = 50;
    public int damage = 10;

    [Header("Shockwave")]
    private GameObject shockwavePrefab;
    public float shockwaveDelay = 1.5f; // délai après l'attaque
    public bool retaliateOnHit = true; // true = riposte après coup reçu

    private int currentHealth;
    private bool isAlive = true;

    // Référence pour la barre de vie instanciée
    private Slider healthbarSlider;

    // Nouveau Input System
    private PlayerControls inputActions;

    void Awake()
    {
        inputActions = new PlayerControls();

        // Charge automatiquement le prefab
        shockwavePrefab = Resources.Load<GameObject>("Prefabs/Shockwave");
        if (shockwavePrefab == null)
        {
            Debug.LogError("Impossible de charger le Shockwave. Vérifie le chemin : Resources/Prefabs/Shockwave.prefab");
        }
        
        // Initialise les Points de Vie
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Cherche une barre de vie si enfant
        healthbarSlider = GetComponentInChildren<Slider>();
        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = maxHealth;
            healthbarSlider.value = currentHealth;
        }
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.CheatRiposte.performed += OnCheatRiposte;
    }

    void OnDisable()
    {
        inputActions.Player.CheatRiposte.performed -= OnCheatRiposte;
        inputActions.Disable();
    }

    /// <summary>
    /// Initialise l'ennemi à partir d'un EnemyData (injecté depuis GameFlowManager).
    /// Appellez cette méthode après l'instanciation pour configurer PV, sprite, et dégâts.
    /// </summary>
    public void Initialize(EnemyData data)
    {
        if (data == null)
        {
            Debug.LogWarning("EnemyHealth.Initialize appelé avec des données null.");
            return;
        }

        // Appliquer les données
        maxHealth = data.maxHealth;
        currentHealth = maxHealth;

        // Mettre à jour la barre de vie si elle est déjà liée
        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = maxHealth;
            healthbarSlider.value = currentHealth;
        }

        // Appliquer le sprite si possible
        if (data.enemySprite != null)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = data.enemySprite;
        }

        // Stocker le damage si nécessaire pour la logique future
        damage = data.damage;
    }
    

     private void OnCheatRiposte(InputAction.CallbackContext context)
    {
        if (isAlive && shockwavePrefab != null)
        {
            // Cheat code : lance directement la riposte
            StartCoroutine(RiposteShockwave());
            Debug.Log("Cheat riposte de la méduse activée !");
        }
    }

    // Appelée quand le joueur inflige des dégâts avec une carte
    public void TakeDamage(int amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;

        // Met à jour la barre de vie si elle existe
        if (healthbarSlider != null)
        {
            healthbarSlider.value = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else if (retaliateOnHit)
        {
            StartCoroutine(RiposteShockwave());
        }
    }

    private IEnumerator RiposteShockwave()
    {
        yield return new WaitForSeconds(shockwaveDelay);

        if (isAlive) // si toujours vivant après le délai
        {
            Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
        }
    }

    private void Die()
    {
        isAlive = false;
        Destroy(gameObject); // provisoire
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.EnemyDied();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Rien à faire ici pour l'instant
    }
}
