using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MedusaEnemy : MonoBehaviour
{

    [Header("Stats")]
    public int health = 50;
    public int damage = 10;

    [Header("Shockwave")]
    private GameObject shockwavePrefab;
    public float shockwaveDelay = 1.5f; // délai après l'attaque
    public bool retaliateOnHit = true; // true = riposte après coup reçu

    private bool isAlive = true;

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

     private void OnCheatRiposte(InputAction.CallbackContext context)
    {
        if (isAlive)
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

        health -= amount;

        if (health <= 0)
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
        GameFlowManager.Instance.EnemyDied();
    }

    // Appelée quand le joueur inflige des dégâts avec une carte 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
