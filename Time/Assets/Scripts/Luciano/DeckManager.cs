using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;


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
    public GameManager _Gm;
    public Card[] cards;

    void Awake()
    {

        //Load all card assets from the Ressources folder
       // cards = Resources.LoadAll<Card>("Cards");
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
        _handManager.currentCardInHand--;
        _handManager.Hand.Remove(_Gm.currentSelectedObject);
        Cemetery.Add(card);
        CemeteryCounter();
        if (_handManager.currentCardInHand == 0 && Deck.Count == 0)
        {
            ResetDeck();
        }

    }

    public void ResetDeck() 
    {
      foreach (GameObject card in _handManager.Hand)
      {
          Destroy(card);
      }
      _handManager.Hand.Clear();
      Deck.AddRange(cards);
      Cemetery.Clear();
      Shuffle();
      DeckCounter();
      CemeteryCounter();
    }

    public void Shuffle()
    {
        for (int i = 0; i < Deck.Count; i++)
        {
            int randomIndex = Random.Range(i, Deck.Count);
            Card temp = Deck[i];
            Deck[i] = Deck[randomIndex];
            Deck[randomIndex] = temp;
        }
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