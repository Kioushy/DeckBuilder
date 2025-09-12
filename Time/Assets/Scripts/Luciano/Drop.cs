using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DropZone : MonoBehaviour , IDropHandler
{
    public DeckManager _DeckM;

    public static event Action<Card> Discard;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
          GameObject cardPlayed = eventData.selectedObject;

            
            cardPlayed.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x / 2, transform.GetComponent<RectTransform>().sizeDelta.y / -2);
            cardPlayed.GetComponent<CardContainer>().LaunchEffect();
            Debug.Log( "Drop : " +  cardPlayed.name);
            DeckManager.Instance.SendToCemetery(cardPlayed.GetComponent<CardContainer>().card);
            Destroy(eventData.selectedObject);
        }
    }


}