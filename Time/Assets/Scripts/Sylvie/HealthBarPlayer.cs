using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using JetBrains.Annotations;
using System.Collections;

public class HealthBarPlayer : MonoBehaviour
{
    [SerializeField] int MaxHealth;
    [SerializeField] int MinHealth;
    // [SerializeField] Slider healthSlider;
    [Header("Input System")]
    [Tooltip("Optional: assign an InputActionReference from your Input Actions for Heal (performed)")]
    [SerializeField] InputActionReference healAction;
    [Tooltip("Optional: assign an InputActionReference from your Input Actions for Damage (performed)")]
    [SerializeField] InputActionReference damageAction;
    // runtime-created actions used when no references are provided
    InputAction runtimeHealAction;
    InputAction runtimeDamageAction;
    bool createdRuntimeHeal;
    bool createdRuntimeDamage;

    [Header("Health Drain")]
    [Tooltip("La durée en secondes pour que la barre de vie se vide complétement")]
    [SerializeField] float drainDuration = 60f;
    public static int currentHealth;

    public static int shield = 0;
    bool death;
    private bool isDead = false;  
    private Coroutine healthDrainCoroutine;
    private Slider playerHealthSlider;

    private static HealthBarPlayer instance;

    public static HealthBarPlayer Instance
    {
        get { return instance; }
    }

    // --- TODO : mettre à jour pour que le temps s'écoule automatiquement 
    // Full bar : 1h (60f * 60f) ou 45 min

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Si le slider n'est pas assigné dans l'Inspecteur, nous le trouvons automatiquement
        // if (healthSlider == null)
        // {
        //     // 1) On cherche un composant Slider sur cet objet ou ses enfants
        //     healthSlider = GetComponentInChildren<Slider>();
        // }

    }

    private void Start()
    {
        // On récupère directement la référence du Slider sur le GameObject
    
        playerHealthSlider  = GetComponent<Slider>();
        

        // Si on n'a toujours pas de slider (soit il n'est pas assigné,
        // soit il n'est pas un enfant de cet objet), on arrête
        // ici pour éviter de faire des recharches coûteuses
        if (playerHealthSlider == null)
        {
            Debug.LogError("HealthBarPlayer : Aucun Slider n'a été assigné ou trouvé ");
            enabled = false;
            return;
        }

        StartCoroutine(WaitAnimation());

        FullHeal();

        //FullHeal();
        // Démarrez la coroutine au début du jeu
        //healthDrainCoroutine = StartCoroutine(DrainHealthOverTime());
    }


    private void OnEnable()
    {
        // S'abonne aux événements des actions
        if (healAction != null) healAction.action.performed += OnHeal;
        if (damageAction != null) damageAction.action.performed += OnDamage;
    }

    private void OnDisable()
    {
        // Se désabonne pour éviter les fuites de mémoire
        if (healAction != null) healAction.action.performed -= OnHeal;
        if (damageAction != null) damageAction.action.performed -= OnDamage;
    }

    // Méthodes de callback pour le nouvel Input System
    public void OnHeal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpdateHealth(+1);
            Debug.Log("Soin reçu !");
        }
    }

    public void OnDamage(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            UpdateHealth(-1);
            Debug.Log("Dégâts reçus !");
        }
    }
    public void UpdateHealth(int healthToChange)
    {

        if (isDead) return; // déjà mort, ignore

        // ajoute healthToChange
        currentHealth += healthToChange;

        // S'assurer que la santé ne dépasse pas la limite
        currentHealth = Mathf.Clamp(currentHealth, MinHealth, MaxHealth);

        //clamp max health
        // if (currentHealth > MaxHealth)
        // {
        //     currentHealth = MaxHealth;
        // }

        // if (death)
        // {
        //     // diePanel.SetActive(true);
        //     Debug.Log("Le joueur est mort !");
        // }

        // // clamp min health
        // if (currentHealth <= MinHealth)
        // {
        //     currentHealth = MinHealth;
        //     death = true;
        //     Debug.Log("Le joueur est mort !");
        //     // Arrêter le drain de vie une fois que le joueur est mort
        //     if (healthDrainCoroutine != null)
        //     {
        //         StopCoroutine(healthDrainCoroutine);
        //     }
        // }

        UpdateSlider();
        
         if (currentHealth <= MinHealth)
        {
            Die();
        }

        // if (currentHealth <= MinHealth && !death)
        // {
        //     death = true;
        //     Debug.Log("Le joueur est mort !");

        //     // Stop drain
        //     if (healthDrainCoroutine != null)
        //     {
        //         StopCoroutine(healthDrainCoroutine);
        //     }

        //     // Notifie le GameFlowManager
        //     if (GameFlowManager.Instance != null)
        //     {
        //         GameFlowManager.Instance.PlayerDied();
        //     }
        // }
    }
    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine(DrainHealthOverTime());
    }


    // Coroutine pour le drain de vie automatique
    private IEnumerator DrainHealthOverTime()
    {
        // Calcule de ltemps entre chaque point de vie perdu
        float timePerHealthPoint = drainDuration / (MaxHealth - MinHealth);
        float timer = 0f;

        while (currentHealth > MinHealth)
        {
            timer += Time.deltaTime; // Time.deltaTime est le temps écoulé depuis la dernière frame

            if (timer >= timePerHealthPoint)
            {
                UpdateHealth(-1); // Réduit la vie d'un point
                timer = 0f;
            }
            yield return null; // Attend la prochaine frame
        }
    }

    /*
    public void Heal(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            UpdateHealth(+1);
        }
    }


    public void Damage(InputAction.CallbackContext context) 
    {
        if (context.performed)
        {
            UpdateHealth(-1);
        }
    }*/

    private void UpdateSlider()
    {
        if (playerHealthSlider == null)
        {
            // Nothing to update
            return;
        }

        playerHealthSlider.maxValue = MaxHealth;
        playerHealthSlider.value = currentHealth;
    }

    public void FullHeal()
    {
        currentHealth = MaxHealth;
        UpdateSlider();
    }

    public void Initialize(Slider slider)
    {
        playerHealthSlider = slider;
    }
    
    public void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Le joueur est mort !");
        // Animation optionnelle
        // if (animator != null)
        // {
        //     animator.SetTrigger("Die");
        // }

        // Stop drain
        if (healthDrainCoroutine != null)
        {
            StopCoroutine(healthDrainCoroutine);
        }

        // Très important : appeler le GameFlowManager
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.PlayerDied();
        }

        // Destroy(gameObject);
    }
}