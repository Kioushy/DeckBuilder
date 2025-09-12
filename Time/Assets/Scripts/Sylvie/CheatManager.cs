using UnityEngine;
using UnityEngine.InputSystem;

public class CheatManager : MonoBehaviour
{
    private Health currentEnemy;
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
        currentEnemy = FindObjectOfType<Health>();
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
                currentEnemy.TakeDamageEnemy(100); // Dégâts suffisants pour tuer n'importe quel ennemi
            }
            else
            {
                Debug.LogWarning("CHEAT: Pas d'ennemi à attaquer.");
            }
        }
    }
    
    // Cheat : inflige quelques dégâts quand on appuie sur Espace
    public void OnCheatRiposte(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (HealthBarPlayer.Instance != null)
            {
                Debug.Log("CHEAT: Riposte simulée -> l'ennemi inflige 5 dégâts au joueur !");
                HealthBarPlayer.Instance.TakeDamage(5); // inflige 5 dégâts au joueur
            }
            else
            {
                Debug.LogWarning("CHEAT: Aucun HealthBarPlayer trouvé dans la scène !");
            }
        }
    }
}