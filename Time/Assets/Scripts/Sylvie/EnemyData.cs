using UnityEngine;

/// <summary>
/// Scriptable Object contenant les données de base pour un type d'ennemi.
/// C'est un conteneur de données pur, il n'exécute aucune logique de jeu.
/// </summary>
// L'attribut [CreateAssetMenu] vous permet de créer des instances de ce script 
// directement depuis le menu "Assets -> Create" de l'éditeur Unity.
[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Informations Générales")]
    public string enemyName = "Nouvel Ennemi";

    [Header("Statistiques de combat")]
    public int maxHealth = 100;

    public int damage = 10;

    public Sprite enemySprite;
}
