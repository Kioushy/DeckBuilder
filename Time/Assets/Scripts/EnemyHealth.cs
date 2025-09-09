using UnityEngine;
using UnityEngine.UI;

// Ce script s'occupe de la santé de l'ennemi
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 3f;

    [Header("UI Settings")]
    [SerializeField] private string healthBarPrefabPath = "UI/HealthBarEnemy";
    [SerializeField] private Vector2 healthBarOffset = new Vector2(0, 1.2f);

    //Références privées
    private float currentHealth;
    private bool isDead = false;   
    private Animator animator;

    // Référence pour la barre de vie dynamique
    private Slider healthbarSlider;
    private GameObject healthbarInstance;
    private Canvas mainCanvas;
    private void Awake()
    {
        animator = GetComponent<Animator>(); // on récupère l’Animator
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // On instancie la barre de vie au démarrage
        SetupHealthBar();
    }

    // Pro-tip: Utiliser LateUpdate pour la UI qui suit un objet du monde
    // Cela garantit que la position est mise à jour PARES que tous les mouvements
    // de l'ennemi (dans Update ou FixedUpdate) ont été calculées pour la frame
    // Ca évite un effet de "sautillement" (jitter)
    private void LateUpdate()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        Debug.Log($"DAMAGE APPELÉ sur {gameObject.name}: {damageAmount} dégâts"); // ⭐ AJOUTÉ


        if (isDead)
        {
            Debug.Log("Ennemi déjà mort, dégâts ignorés"); // ⭐ AJOUTÉ
            return;
        }

        currentHealth -= damageAmount;
        // healthBar.value = currentHealth;

        Debug.Log($"Nouvelle vie de {gameObject.name}: {currentHealth}/{maxHealth}"); // ⭐ AJOUTÉ

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void SetupHealthBar()
    {
        
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die"); // Lance anim de mort
        GetComponent<Collider2D>().enabled = false; // optionnel : désactive collisions
        GetComponent<Rigidbody2D>().simulated = false; // optionnel : stop physique

        Destroy(gameObject, 1f); // ou Animation Event pour caler pile la durée
    }
}
