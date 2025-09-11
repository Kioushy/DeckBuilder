using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private int currentHealth;
    [SerializeField] private int counterAttackDamage = 3; // Dégâts de la riposte

    private HealthBarPlayer healthBarPlayer; // Référent au joueur
    private AudioSource audioSource;

    private void Awake()
    {
        // Audio
        audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("sharkbite");
        audioSource.clip = clip;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        // Récupère une instance existante de HealthBarPlayer dans la scène (API moderne)
        healthBarPlayer = UnityEngine.Object.FindAnyObjectByType<HealthBarPlayer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Requin reçoit " + damage + " dégâts. PV restants : " + currentHealth);

        // Riposte immédiate
        CounterAttack();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void CounterAttack()
    {
        // Jouer le son
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        
        if (healthBarPlayer != null)
        {
            healthBarPlayer.TakeDamage(counterAttackDamage);
            Debug.Log("Le requin riposte et inflige " + counterAttackDamage + " dégâts au joueur !");
        }
    }

    private void Die()
    {
        Debug.Log("Requin est vaincu !");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
