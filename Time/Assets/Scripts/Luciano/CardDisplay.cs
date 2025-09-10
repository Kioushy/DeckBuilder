using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardDisplay : MonoBehaviour
{
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public TMP_Text damageText;
    public TMP_Text protectText;
    public TMP_Text healText;
    public TMP_Text descriptionText;
    public Image[] typeImages;

    private Color[] cardColors =
    {
        new Color(1f, 1f, 1f), //carte attaque 
        new Color(0.04313726f, 0.2745098f, 0.5960785f),// carte défense
        new Color(0.972549f, 0.7254902f, 0.227451f) // carte spécial
    };

       private Color[] typeColors =
    {
        new Color(0.04313726f, 0.2745098f, 0.5960785f), //icône mono 
        new Color(1f, 1f, 1f),// icône multi
        new Color(0f, 0f, 0f) // icône overtime
    };
    void Start()
    {
        UpdateCardDisplay();
    }
    public void UpdateCardDisplay()
    {
        //Update the main card image color based on the first card type
        cardImage.color = cardColors[(int)cardData.cardType[0]];

        nameText.text = cardData.cardName;
        damageText.text = $"{cardData.damage}";
        protectText.text = $"{cardData.protect}";
        healText.text = $"{cardData.heal}";
        descriptionText.text = $"{cardData.descriptionCard}";

        //Update type images
        for (int i = 0; i < typeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                typeImages[i].gameObject.SetActive(true);
                typeImages[i].color = typeColors[(int)cardData.cardType[i]];
            }
            else
            {
                typeImages[i].gameObject.SetActive(false);
            }
        }
    }
}
