using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    public Card card;
    public void LaunchEffect()
    {
        if (card.type == Card.TypeCard.Attack)
        {
            EnemyHealth.Instance.TakeDamage(card.damage);
        }

        if (card.type == Card.TypeCard.Defense)
        {
            HealthBarPlayer.Instance.UpdateHealth(card.protect);
        }

        if (card.type == Card.TypeCard.Special)
        {
            HealthBarPlayer.Instance.UpdateHealth(card.heal);
        }
        Debug.Log("Effect apply");
    }
}


