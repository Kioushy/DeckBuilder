using UnityEngine;
using UnityEngine.UI;

// Ce script s'occupe de la santé de l'ennemi et instancie sa barre de vie
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3f;

    [Header("UI Settings")]
    [Tooltip("Prefab de la barre de vie qui contient un Canvas en mode 'World Space")]

    //Références privées
    public float currentHealth;
    private bool isDead = false;   
    // private Animator animator;

    // Référence pour la barre de vie instanciée
    private Slider healthbarSlider;

    // Valeur optionnelle provenant des données de l'ennemi
    private int attackDamage = 0;

    private static EnemyHealth instance;
    public static EnemyHealth Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /*
    private void Awake()
    {
        // animator = GetComponent<Animator>(); // on récupère l’Animator
    }
    */
    private void Start()
    {
        healthbarSlider = transform.GetChild(0).transform.GetChild(0).GetComponent<Slider>();
        currentHealth = maxHealth;

        // On instancie la barre de vie au démarrage

        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = maxHealth;
            healthbarSlider.value = currentHealth;
        }
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
        attackDamage = data.damage;
    }
    
    public void TakeDamage(float damageAmount)
    {
        Debug.Log($"DAMAGE APPELÉ sur {gameObject.name}: {damageAmount} dégâts"); 


        if (isDead)
        {
            Debug.Log("Ennemi déjà mort, dégâts ignorés"); 
            return;
        }

        currentHealth -= damageAmount;

        // Mettre à jour ici la valeur du slider
        if (healthbarSlider != null)
        {
            healthbarSlider.value = currentHealth;
        }

        Debug.Log($"Nouvelle vie de {gameObject.name}: {currentHealth}/{maxHealth}"); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // animator.SetTrigger("Die"); // Lance anim de mort
        // GetComponent<Collider2D>().enabled = false; // optionnel : désactive collisions

        // On notifie le GameFlowManager que l'ennemi est mort
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.EnemyDied();
        }
        // On détruit l'ennemi après un délai, la barre de vie sera détruite via OnDestroy()
        Destroy(gameObject); // ou Animation Event pour caler pile la durée
    }
}
