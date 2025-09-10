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
    public static float currentHealth;
    private bool isDead = false;   
    // private Animator animator;

    // Référence pour la barre de vie instanciée
    private Slider healthbarSlider;


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
