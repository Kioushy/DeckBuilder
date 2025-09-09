using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Time;
public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    private int currentIndex = 0;

    void Start()
    {
        //Load all card assets from the Ressources folder
        Card[] cards = Resources.LoadAll<Card>("Cards");
        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);
        HandManager hand = FindObjectOfType<HandManager>();
        for (int i = 0; i < 3; i++)
        {
            DrawCard(hand);
        }
    }
    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
            return;

        Card nextCard = allCards[currentIndex];
        handManager.AddCardToHand(nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }
}