using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();

    public int startingHandSize = 3;

    public int maxHandSize = 3;
    public int currentHandSize;
    public HandManager handManager;
    public DrawPileManager drawPileManager;
    public bool startBattleRun = true;

    void Start()
    {
        //Load all card assets from the Ressources folder
        Card[] cards = Resources.LoadAll<Card>("Cards");
        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);
    }

    void Awake()
    {
        if (drawPileManager == null)
        {
            drawPileManager = FindFirstObjectByType<DrawPileManager>();
        }

        if (handManager == null)
        {
            handManager = FindFirstObjectByType<HandManager>();
        }
    }

    private void Update()
    {
        if (startBattleRun)
        {
            BattleSetup();
        }
    }

    public void BattleSetup()
    {
        handManager.BattleSetup();
        drawPileManager.MakeDrawPile(allCards);
        drawPileManager.BattleSetup(startingHandSize, maxHandSize);
        startBattleRun = false;
    }

}