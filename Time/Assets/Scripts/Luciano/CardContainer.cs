using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    public Card card;
    public void LaunchEffect()
    {
        
        if (card.Type == Card.TypeCard.Attack)
        {
            EnemyHealth.Instance.TakeDamage(-card.damage);
        }

        if (card.Type == Card.TypeCard.Defense)
        {
            HealthBarPlayer.Instance.UpdateHealth(card.protect);
        }

        if (card.Type == Card.TypeCard.Special)
        {
            HealthBarPlayer.Instance.UpdateHealth(card.heal);
        }
        Debug.Log("Effect apply");
    }
}


