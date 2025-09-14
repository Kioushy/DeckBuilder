using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardContainer : MonoBehaviour
{
    public Card card;

    private Health _healthE;
    private Health _healthP;

    public void Start()
    {
      _healthE = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Health>();
      _healthP = GameObject.FindGameObjectWithTag("HealthBarPlayer").GetComponent<Health>();
    }   


    public void LaunchEffect()
    {
        
        if (card.Type == Card.TypeCard.Attack)
        {
            _healthE.TakeDamageEnemy(-card.damage);
        }

        if (card.Type == Card.TypeCard.Defense)
        {
            _healthP.UpdateHealth(card.protect);
        }

        if (card.Type == Card.TypeCard.Special)
        {
            _healthP.UpdateHealth(card.heal);
        }
        Debug.Log("Effect apply");
    }
}


