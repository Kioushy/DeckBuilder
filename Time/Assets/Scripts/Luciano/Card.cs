using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string cardName;
    public List<CardType> cardType;

     public enum TypeCard { Defense, Attack, Special };
    public TypeCard type;
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
}
