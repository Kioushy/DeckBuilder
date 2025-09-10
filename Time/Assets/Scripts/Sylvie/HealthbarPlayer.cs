using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using JetBrains.Annotations;
using System.Collections;

public class HealthbarPlayer : MonoBehaviour
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
    int currentHealth;
    bool death;
    private Coroutine healthDrainCoroutine;
    private Slider healthSlider;

    // --- TODO : mettre à jour pour que le temps s'écoule automatiquement 
    // Full bar : 1h (60f * 60f) ou 45 min

    private void Awake()
    {
        // On récupère directement la référence du Slider via l'UIManager
        if (UIManager.Instance != null)
        {
            healthSlider = UIManager.Instance.GetPlayerHealthSlider();
        }
        
        // Si le slider n'est pas assigné dans l'Inspecteur, nous le trouvons automatiquement
        // if (healthSlider == null)
        // {
        //     // 1) On cherche un composant Slider sur cet objet ou ses enfants
        //     healthSlider = GetComponentInChildren<Slider>();
        // }

        // Si on n'a toujours pas de slider (soit il n'est pas assigné,
        // soit il n'est pas un enfant de cet objet), on arrête
        // ici pour éviter de faire des recharches coûteuses
        if (healthSlider == null)
        {
            Debug.LogError("HealthBarPlayer : Aucun Slider n'a été assigné ou trouvé comme ");
            // Pour éviter les NullReferenceException plus tard dans le code
            // vus pouvez désactiver le script si le composant est manquant
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        FullHeal();
        // Démarrez la coroutine au début du jeu
        healthDrainCoroutine = StartCoroutine(DrainHealthOverTime());
    }


    private void OnEnable()
    {
        // Heal action: prefer assigned InputActionReference, otherwise create a keyboard fallback (H)
        if (healAction != null && healAction.action != null)
        {
            healAction.action.performed += OnHealPerformed;
            healAction.action.Enable();
        }
        else
        {
            runtimeHealAction = new InputAction("Heal", InputActionType.Button, "<Keyboard>/h");
            runtimeHealAction.performed += OnHealPerformed;
            runtimeHealAction.Enable();
            createdRuntimeHeal = true;
        }

        // Damage action: prefer assigned InputActionReference, otherwise create a keyboard fallback (K)
        if (damageAction != null && damageAction.action != null)
        {
            damageAction.action.performed += OnDamagePerformed;
            damageAction.action.Enable();
        }
        else
        {
            runtimeDamageAction = new InputAction("Damage", InputActionType.Button, "<Keyboard>/k");
            runtimeDamageAction.performed += OnDamagePerformed;
            runtimeDamageAction.Enable();
            createdRuntimeDamage = true;
        }
    }

    private void OnDisable()
    {
        if (healAction != null && healAction.action != null)
        {
            healAction.action.performed -= OnHealPerformed;
            healAction.action.Disable();
        }

        if (damageAction != null && damageAction.action != null)
        {
            damageAction.action.performed -= OnDamagePerformed;
            damageAction.action.Disable();
        }

        if (runtimeHealAction != null)
        {
            runtimeHealAction.performed -= OnHealPerformed;
            runtimeHealAction.Disable();
            runtimeHealAction.Dispose();
            runtimeHealAction = null;
            createdRuntimeHeal = false;
        }

        if (runtimeDamageAction != null)
        {
            runtimeDamageAction.performed -= OnDamagePerformed;
            runtimeDamageAction.Disable();
            runtimeDamageAction.Dispose();
            runtimeDamageAction = null;
            createdRuntimeDamage = false;
        }
    }

    private void OnHealPerformed(InputAction.CallbackContext context)
    {
        if (context.performed) UpdateHealth(+1);
    }

    private void OnDamagePerformed(InputAction.CallbackContext context)
    {
        if (context.performed) UpdateHealth(-1);
    }

    private void UpdateHealth(int healthToChange) 
    {
        // ajoute healthToChange
        currentHealth += healthToChange;

        //clamp max health
        if (currentHealth > MaxHealth) 
        {
            currentHealth = MaxHealth;
        }

        if (death)
        {
            // diePanel.SetActive(true);
            Debug.Log("Le joueur est mort !");
        }

        // clamp min health
        if (currentHealth <= MinHealth)
        {
            currentHealth = MinHealth;
            death = true;
            Debug.Log("Le joueur est mort !");
            // Arrêter le drain de vie une fois que le joueur est mort
            if (healthDrainCoroutine != null)
            {
                StopCoroutine(healthDrainCoroutine);
            }
        }

        UpdateSlider();
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
        if (healthSlider == null)
        {
            // Nothing to update
            return;
        }

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = currentHealth;
    }

    public void FullHeal() 
    {
        currentHealth = MaxHealth;
        UpdateSlider();
    }


}