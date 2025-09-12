using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class HandManager : MonoBehaviour
{

    private static HandManager instance;
    public static HandManager Instance
    {
        get { return instance; }
    }

    public DeckManager deckManager;

    public GameObject cardPrefab; //assign card Prefab in the inspector
    public Transform handTransform; //Là où se trouvera la position de la main
    public float fanSpread = 5f;
    public float cardSpacing = 5f;
    public float verticalSpacing = 100f;
    public List<GameObject> Hand = new List<GameObject>(); //tenir la liste des cartes object dans la main
    public int startingHandSize = 3;

    public int maxHandSize = 3;
    public int currentCardInHand;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        HandCounter();
    }



    private void UpdateHandVisuals()
    {
        int cardCount = Hand.Count;

        if (cardCount == 1)
        {
            Hand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            Hand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            Hand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //normalise la position des cartes entre -1 et 1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            //set card position
            Hand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }


    public void GenerateCard() 
    {
        //instancier la carte
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity,handTransform);
        Hand.Add(newCard);
        //Set the CardData of the instantiated card 
        newCard.GetComponent<CardDisplay>().cardData = deckManager.Deck[0];
        newCard.GetComponent<CardContainer>().card = deckManager.Deck[0];
        deckManager.Deck.RemoveAt(0);
        HandCounter();
        UpdateHandVisuals();
    }

    public void HandCounter()
    {
        currentCardInHand = Hand.Count;
        //Text
    }

}
