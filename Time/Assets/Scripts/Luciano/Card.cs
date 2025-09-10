using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Card card;
    public string cardName;
    public List<CardType> cardType;
    public int damage;
    public int protect;
    public int heal;
    public enum Players { Joueur1, Joueur2 };
    public Players player;
    public damageType dealDamage;
    public protectType protection;
    public healType healing;
    public string descriptionCard;
    public enum damageType { Mono, Multi, Overtime };
    public enum protectType { Mono, Multi, Overtime };
    public enum healType { Mono, Multi, Overtime };

    public enum CardType
    {
        Attack,
        Defense,
        Special,
    }

    public void LaunchEffect()
    {
        if (card.damage != 0)
        {
            EnemyHealth.currentHealth -= card.damage;
        }

        if (card.protect != 0)
        {
            HealthbarPlayer.shield += card.protect;
        }

        if (card.heal != 0)
        {
            HealthbarPlayer.currentHealth += card.heal;
        }
    }
}
