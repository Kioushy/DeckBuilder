using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Time;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject cardPrefab; //assign card Prefab in the inspector
    public Transform handTransform; //Là où se trouvera la position de la main
    public float fanSpread = 5f;
    public float cardSpacing = 5f;
    public float verticalSpacing = 100f;
    public List<GameObject> cardsInHand = new List<GameObject>(); //tenir la liste des cartes object dans la main
    void Start()
    {
        AddCardToHand();
        AddCardToHand();
        AddCardToHand();
    }

    // Update is called once per frame
 
    public void AddCardToHand()
    {
        //instancier la carte
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        UpdateHandVisuals();
    }

    void Update()
    {
        UpdateHandVisuals();
    }
    private void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }
        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));
            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //normalise la position des cartes entre -1 et 1
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            //set card position
            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }
}
