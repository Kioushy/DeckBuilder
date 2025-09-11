using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class DiscardManager : MonoBehaviour
{
    [SerializeField] public List<Card> discardCards = new List<Card>();
    public TextMeshProUGUI discardCount;
    public int discardCardsCount;

    private void Awake()
    {
        UpdateDiscardCount();
    }

    private void OnEnable()
    {
        DropZone.Discard += AddDiscard;
    }

    private void OnDisable()
    {
        DropZone.Discard -= AddDiscard;
    }

    private void AddDiscard(Card _card)
    {
        discardCards.Add(_card);
        if (discardCards.Count == 3)
        {
            DrawPileManager.Instance.AddCardsFromDiscard(discardCards);
        }
        UpdateDiscardCount();
    }
    private void UpdateDiscardCount()
    {
        discardCount.text = discardCards.Count.ToString();
        discardCardsCount = discardCards.Count;
    }

    public void AddToDiscard(Card card)
    {
        if (card != null)
        {
            discardCards.Add(card);
            UpdateDiscardCount();
        }

    }

    public Card PullFromDiscard()
    {
        if (discardCards.Count > 0)
        {
            Card cardToReturn = discardCards[discardCards.Count - 1];
            discardCards.RemoveAt(discardCards.Count - 1);
            UpdateDiscardCount();
            return cardToReturn;
        }
        else
        {
            return null;
        }
    }

    public bool PullSelectCardFromDiscard(Card card)
    {
        if (discardCards.Count > 0 && discardCards.Contains(card))
        {
            discardCards.Remove(card);
            UpdateDiscardCount();
            return true;
        }

        else
        {
            return false;
        }
    }

    public List<Card> PullAllFromDiscard()
    {
        if (discardCards.Count > 0)
        {
            List<Card> cardsToReturn = new List<Card>(discardCards);
            discardCards.Clear();
            UpdateDiscardCount();
            return cardsToReturn;
        }

        else
        {
            return new List<Card>();
        }
    }
}
