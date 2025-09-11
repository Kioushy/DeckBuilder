using UnityEngine;

public class DialogueFlowManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public EnemyDialogue medusa;
    public EnemyDialogue shark;
    public EnemyDialogue snake;

    private EnemyDialogue currentEnemy;

    void Start()
    {
        // On commence avec la Méduse
        StartEnemyEncounter(medusa);
    }

    void StartEnemyEncounter(EnemyDialogue enemy)
    {
        currentEnemy = enemy;

        // Intro de l’ennemi
        dialogueManager.StartDialogue(enemy.introLines, StartCombat);
    }

    void StartCombat()
    {
        Debug.Log("Simulation de combat contre " + currentEnemy.enemyName);

        // -----------------------------
        // TEST SANS COMBAT : victoire automatique
        dialogueManager.StartDialogue(currentEnemy.victoryLines, NextEnemy);
        // -----------------------------

        /* PLUS TARD : remplacer par ton vrai combat
        bool playerWin = true; // ou résultat réel du combat

        if(playerWin)
            dialogueManager.StartDialogue(currentEnemy.victoryLines, NextEnemy);
        else
            dialogueManager.StartDialogue(currentEnemy.defeatLines, () =>
            {
                Debug.Log("Game Over ? retour menu principal");
            });
        */
    }

    void NextEnemy()
    {
        if (currentEnemy == medusa) StartEnemyEncounter(shark);
        else if (currentEnemy == shark) StartEnemyEncounter(snake);
        else Debug.Log("Tous les ennemis vaincus !");
    }
}
