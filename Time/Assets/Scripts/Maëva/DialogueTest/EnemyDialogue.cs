using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker; // Qui parle (Enemy ou Player)
    [TextArea(2, 5)]
    public string text;   // La phrase
}

[CreateAssetMenu(fileName = "NewEnemyDialogue", menuName = "Dialogue/Enemy Dialogue")]
public class EnemyDialogue : ScriptableObject
{
    public string enemyName;

    [Header("Intro Dialogue")]
    public DialogueLine[] introLines;

    [Header("Victory Dialogue (Player gagne)")]
    public DialogueLine[] victoryLines;

    [Header("Defeat Dialogue (Enemy gagne)")]
    public DialogueLine[] defeatLines;
}
