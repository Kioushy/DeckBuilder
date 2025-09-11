using UnityEngine;

/// <summary>
/// Ce script est attaché à chaque ennemi en jeu.
/// Il lit les données d'un Scriptable Object (EnemyData) pour s'initialiser
/// et gère l'état de santé actuel de l'ennemi.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    // Référence aux données de l'ennemi. Sera assignée au moment de l'instanciation.
    public EnemyData enemyData; 
    
    // La santé ACTUELLE de cette instance d'ennemi.
    private float currentHealth;

    // Référence à la barre de vie de la scène.
    private HealthBarEnemy healthBar;

    private static EnemyHealth instance;
    public static EnemyHealth Instance
    {
        get { return instance; }
    }

    public void Initialize(EnemyData data)
    {
        // On reçoit les données depuis le GameFlowManager
        enemyData = data;

        // On initialise la vie de l'ennemi avec la valeur max définie dans le SO
        currentHealth = enemyData.maxHealth;

        // On trouve la barre de vie dans la scène
        healthBar = FindObjectOfType<HealthBarEnemy>();
        if (healthBar != null)
        {
            // On met à jour la barre de vie une première fois pour l'afficher pleine.
            healthBar.UpdateHealthBar(enemyData.maxHealth, currentHealth);
        }
        else
        {
            Debug.LogError("Aucune HealthBarEnemy trouvée dans la scène !");
        }
    }

    public void TakeDamage(float amount)
    {
        if (enemyData == null) return; // Sécurité

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, enemyData.maxHealth);

        // Mettre à jour l'affichage de la barre de vie
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(enemyData.maxHealth, currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Logique de mort (lancer une animation, un son, un effet, etc.)
        Debug.Log($"{enemyData.enemyName} est vaincu !");

        // Notifier le GameFlowManager
        GameFlowManager.Instance.EnemyDied();
        
        // Détruire l'objet de l'ennemi
        Destroy(gameObject);
    }
}