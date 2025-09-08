using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Time
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public List<CardType> cardType;
        public int damage;
        public int protect;
        public int heal;
        public string descriptionCard;
        public List<DamageType> damageType;
        public List<ProtectType> protectType;
        public List<HealType> healType;

        public enum CardType
        {
            Attack,
            Defense,
            Special,
        }

        public enum DamageType
        {
            Mono,
            Multi,
            Overtime,
        }

        public enum ProtectType
        {
            Mono,
            Multi,
            Overtime,
        }

        public enum HealType
        {
            Mono,
            Multi,
            Overtime,
        }
    }
}