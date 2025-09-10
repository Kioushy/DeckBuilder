using UnityEngine;
using UnityEngine.InputSystem;

public class CheatManager : MonoBehaviour
{
    private EnemyHealth currentEnemy;
    // private GameFlowManager gameFlowManager;
    // private bool initialized = false;

    private void Start()
    {
        // On s'abonne à un événement si GameFlowManager notifie le changement de niveau
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.OnLevelChanged += FindCurrentEnemy;
        }

        FindCurrentEnemy();
        
    }

    private void OnDestroy()
    {
        // N'oublie pas de te désabonner pour éviter les fuites de mémoire
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.OnLevelChanged -= FindCurrentEnemy;
        }
    }

    // On ré-exécute cette méthode chaque fois qu'un niveau change
    public void FindCurrentEnemy()
    {
        // On cherche l'ennemi en fonction du tag "Enemy" ou de son script
        currentEnemy = FindObjectOfType<EnemyHealth>();
        if (currentEnemy == null)
        {
            Debug.LogWarning("CheatManager: Aucun ennemi trouvé dans la scène.");
        }
        else
        {
            Debug.Log("CheatManager: Cible actuelle : " + currentEnemy.gameObject.name);
        }
    }
    
    // Méthode appelée par l'Input System pour la "tricherie" d'attaque
    public void OnCheatAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentEnemy != null)
            {
                Debug.Log("CHEAT: Attaque simulée sur l'ennemi.");
                currentEnemy.TakeDamage(100f); // Dégâts suffisants pour tuer n'importe quel ennemi
            }
            else
            {
                Debug.LogWarning("CHEAT: Pas d'ennemi à attaquer.");
            }
        }
    }
}