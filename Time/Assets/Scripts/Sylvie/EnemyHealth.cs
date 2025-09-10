using UnityEngine;
using UnityEngine.UI;

// Ce script s'occupe de la santé de l'ennemi et instancie sa barre de vie
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3f;

    [Header("UI Settings")]
    [Tooltip("Prefab de la barre de vie qui contient un Canvas en mode 'World Space")]
    [SerializeField] private string healthBarPrefabPath = "UI/HealthBarEnemy";

    //Références privées
    private float currentHealth;
    private bool isDead = false;   
    // private Animator animator;

    // Référence pour la barre de vie instanciée
    private Slider healthbarSlider;
    private GameObject healthbarInstance;

    private void Awake()
    {
        // animator = GetComponent<Animator>(); // on récupère l’Animator
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // On instancie la barre de vie au démarrage
        SetupHealthBar();

        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = maxHealth;
            healthbarSlider.value = currentHealth;
        }
    }

    private void SetupHealthBar()
    {

        // 1. Charger le prefab depuis le dossier Resources
        GameObject healthBarPrefab = Resources.Load<GameObject>(healthBarPrefabPath);
        if (healthBarPrefab == null)
        {
            Debug.LogError("Prefab de la barre de vie non assigné. Assurez-vous de le lier dans l'Inspector de l'ennemi.");
            enabled = false;
            return;
        }

        // 1. Instancier le prefab comme enfant du Canvas
        // Le deuxième argument (transform) le rend enfant du GameObject actuel
        healthbarInstance = Instantiate(healthBarPrefab, transform);

        // 2. Décaler la position de la barre de vie
        // La position est relative au parent (l'ennemi)
        // C'est beaucoup plus simple et performant
        healthbarInstance.transform.localPosition = new Vector2(0, 1.2f);

        // 3. Récupérer le composant Slider
        healthbarSlider = healthbarInstance.GetComponentInChildren<Slider>();

        if (healthbarSlider == null)
        {
            Debug.LogError("Le prefab de la barre de vie ne contient pas de composant Slider ou son parent.");
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
