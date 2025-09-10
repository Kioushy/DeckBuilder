using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    public Card card;
     public void LaunchEffect()
    {
        if (card.damage != 0)
        {
            EnemyHealth.currentHealth -= card.damage;
        }

        if (card.protect != 0)
        {
            HealthBarPlayer.shield += card.protect;
        }

        if (card.heal != 0)
        {
            HealthBarPlayer.currentHealth += card.heal;
        }
    }
}
