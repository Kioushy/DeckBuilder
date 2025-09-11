using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class DeckManager : MonoBehaviour
{
    private static DeckManager instance;
    public static DeckManager Instance
    {
        get { return instance; }
    }

    public List<Card> Deck = new List<Card>();
    public List<Card> DeckTemp = new List<Card>();
    public int cardInDeck;
    public List<Card> Cemetery  = new List<Card>();
    public int CardInCemetery;

    private HandManager _handManager;

    void Awake()
    {

        //Load all card assets from the Ressources folder
        Card[] cards = Resources.LoadAll<Card>("Cards");
        //Add the loaded cards to the Deck list
        Deck.AddRange(cards);

    }

    void Start()
    {
        _handManager = HandManager.Instance;
    }

    public void DrawCard(int cardToDraw)
    {
        for (int cardDrawed = 0; cardDrawed < cardToDraw; cardDrawed++)
        {
            if (_handManager.currentCardInHand <= _handManager.maxHandSize)
            {
                _handManager.GenerateCard();
                DeckCounter();
            }
        } 
    }

    public void SendToCemetery(Card card) 
    {
        Cemetery.Add(card);
        CemeteryCounter();
    }

    public void ResetDeck() 
    {
        if (Deck.Count == 0 && _handManager.currentCardInHand == 0)
        {
            Deck.AddRange(Cemetery);
            Cemetery.Clear();
            Shuffle();
            DeckCounter();
            CemeteryCounter();
        }
    }

    /*
    private void RefillDeckFromDiscard()
    {
        if (discardManager == null)
        {
            discardManager = FindFirstObjectByType<DiscardManager>();
        }

        if (discardManager != null && discardManager.discardCardsCount > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utility.Shuffle(drawPile);
            currentIndex = 0;
        }
    }

    public void AddCardsFromDiscard(List<Card> _cards)
    {
        /*foreach (Card _card in _cards)
        {
            drawPile.Add(_card);
        }

        for (int i = 0; i < _cards.Count; i++)
        {
            drawPile.Add(_cards[i]);
            Debug.Log("Card Name Add = " + drawPile[0].cardName);
        }
    }
    */
    public void Shuffle()
    {
        DeckTemp.AddRange(Deck);
        Deck.Clear();
        Deck = DeckTemp.OrderBy(c => Random.value).ToList();
        DeckTemp.Clear();
    }

    private void DeckCounter()
    {
        cardInDeck = Deck.Count;
        //Text
    }

    private void CemeteryCounter()
    {
        CardInCemetery = Cemetery.Count;
        //Text
    }

}