using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using JetBrains.Annotations;

public class Health : MonoBehaviour
{
    [SerializeField] int MaxHealth;
    [SerializeField] int MinHealth;
    [SerializeField] Slider healthSlider;
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
    int currentHealth;
    bool death;
    // public GameObject diePanel;

    // --- TODO : mettre à jour pour que le temps s'écoule automatiquement 
    // Full bar : 1h (60f * 60f) ou 45 min

    private void Awake()
    {
        // Si le slider n'est pas assigné dans l'Inspecteur, nous le trouvons automatiquement
        if (healthSlider == null)
        {
            // 1) On cherche un composant Slider sur cet objet ou ses enfants
            healthSlider = GetComponentInChildren<Slider>();
        }

        if (healthSlider == null)
        {
            // 2) On essaie de trouver un GameObject nommé "HealthSlider" dans la scène
            GameObject go = GameObject.Find("HealthSlider");
            if (go != null) healthSlider = go.GetComponent<Slider>();
        }

        if (healthSlider == null)
        {
            // 3) En dernier recours, on cherche n'importe quel Slider dans la scène
            healthSlider = UnityEngine.Object.FindAnyObjectByType<Slider>();
        }

        if (healthSlider == null)
        {
            // 4) Fallback to a Resources asset if you keep a prefab there
            healthSlider = Resources.Load<Slider>("Healthbar/HealthSlider");
        }

        if (healthSlider == null)
        {
            Debug.LogWarning("Health: Aucun Slider n'a été assigné ou trouvé. Assurez-vous qu'il y a un Slider dans la scène.");
        }
    }

    private void Start()
    {
        FullHeal();
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
        }

        UpdateSlider();
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