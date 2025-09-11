using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using Abyss;

public class DrawPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();

    public int startingHandSize = 3;
    private int currentIndex = 0;

    public int maxHandSize;
    public int currentHandSize;
    public HandManager handManager;
    public DiscardManager discardManager;
    public TextMeshProUGUI drawPileCounter;

    void Start()
    {
        handManager = FindFirstObjectByType<HandManager>();
    }

    void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }

    public void MakeDrawPile(List<Card> cardsToAdd)
    {
        drawPile.AddRange(cardsToAdd);
        Utility.Shuffle(drawPile);
        UpdateDrawPileCount();
    }

    public void BattleSetup(int numberOfCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            DrawCard(handManager);
        }
    }
    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            RefillDeckFromDiscard();
        }

        if (currentHandSize < maxHandSize)
        {
            int rand = Random.Range(0, drawPile.Count - 1);
            Card nextCard = drawPile[rand];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);
            UpdateDrawPileCount();
            if (drawPile.Count > 0) currentIndex %= drawPile.Count;
            drawPile.Remove(drawPile[rand]);

        }
    }

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

    private void UpdateDrawPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }
}
