using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Ce script s'occupe de la santé de l'ennemi et instancie sa barre de vie
public class Health : MonoBehaviour
{
   

    [Tooltip("Prefab de la barre de vie qui contient un Canvas en mode 'World Space")]

    [Header("Stats")]
    //Références privées
    [SerializeField] int MaxHealth;
    [SerializeField] int MinHealth;
    public float currentHealth;
    private bool isDead = false;

    [SerializeField] float drainDuration = 60f;

    // private Animator animator;

    // Référence pour la barre de vie instanciée
    public Slider healthbarSlider;

    public enum HealthType { Enemy,Player}
    public HealthType htype;

    public enum EnemyType { Shark, Meduse, Serpent }
    public EnemyType eType;

    public GameFlowManager _GFm;
    private void Start()
    {
        
        currentHealth = MaxHealth;

        // On instancie la barre de vie au démarrage

        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = MaxHealth;
            healthbarSlider.value = currentHealth;
        }


        if (htype == HealthType.Player)
        {
            healthbarSlider = GetComponent<Slider>();
            InitializePlayerHealth();
      
        }
        if (htype == HealthType.Enemy)
        {
         

            switch (eType) 
            {
                case EnemyType.Shark:
                   // Initialize(transform.GetChild(1).GetComponent<SharkEnemy>().enemyData);
                    break;

                    case EnemyType.Meduse:
                    InitializeEnemy(transform.GetChild(0).GetComponent<MedusaEnemy>().enemyData);
                    break;
                    case EnemyType.Serpent:
                 //   Initialize(GetComponent<MedusaEnemy>().enemyData);
                    break;
            }
          //  healthbarSlider = GameObject.FindGameObjectWithTag("HealthBarEnemy").GetComponent<Slider>();


        }

    }


    #region Player
    public void InitializePlayerHealth()
    {
        FullHeal();
        StartCoroutine(WaitAnimation());
    }

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

    public void TakeDamagePlayer(int damageAmount)
    {
        currentHealth += damageAmount;
        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            EnemyDie();
        }

    }

    #endregion

    #region Enemy

    public void InitializeEnemy(EnemyData data)
    {
        if (data == null)
        {
            Debug.LogWarning("Health.Initialize appelé avec des données null.");
            return;
        }

        // Appliquer les données
        MaxHealth = data.maxHealth;
        currentHealth = MaxHealth;


        // Mettre à jour la barre de vie si elle est déjà liée
        if (healthbarSlider != null)
        {
            healthbarSlider.maxValue = MaxHealth;
            healthbarSlider.value = currentHealth;
        }


    }

    public void EnemyDie()
    {
        isDead = true;
        _GFm.EnemyDied();
    }



    public void TakeDamageEnemy(int damageAmount)
    {
        currentHealth += damageAmount;
        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            EnemyDie();
        }

    }

    #endregion



    private IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(DrainHealthOverTime());
    }

    public void FullHeal() 
    {
        currentHealth = MaxHealth;
        UpdateHealthSlider();
    }

    public void UpdateHealthSlider()
    {
        healthbarSlider.value = currentHealth;
    }



    public void UpdateHealth(int healthToChange)
    {


        // ajoute healthToChange
        currentHealth += healthToChange;

        // S'assurer que la santé ne dépasse pas la limite
        currentHealth = Mathf.Clamp(currentHealth, MinHealth, MaxHealth);

        UpdateHealthSlider();

        if (currentHealth <= MinHealth)
        {
            GameFlowManager.Instance.PlayerDied();
        }

 
    }




}
