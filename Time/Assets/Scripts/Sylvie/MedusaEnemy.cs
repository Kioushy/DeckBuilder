using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class MedusaEnemy : MonoBehaviour
{

    public EnemyData enemyData; // Référence aux données de l'ennemi

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

    // Appelée quand le joueur inflige des dégâts avec une carte 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Stocker le damage si nécessaire pour la logique future
        damage = enemyData.damage;

        // Appliquer le sprite si possible
        if (enemyData.enemySprite != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.sprite = enemyData.enemySprite;
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

    private IEnumerator RiposteShockwave()
    {
        yield return new WaitForSeconds(shockwaveDelay);

        if (isAlive) // si toujours vivant après le délai
        {
            Instantiate(shockwavePrefab, transform.position, Quaternion.identity);
          
        }
    }

 

    // Update is called once per frame
    void Update()
    {
        
    }
}
