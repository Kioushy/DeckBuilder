using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DropZone : MonoBehaviour , IDropHandler
{

    public static event Action<Card> Discard;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.selectedObject.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x / 2, transform.GetComponent<RectTransform>().sizeDelta.y / -2);
            eventData.selectedObject.GetComponent<CardContainer>().LaunchEffect();
            Debug.Log(eventData.selectedObject.name);
            Debug.Log("Drop");
            Discard?.Invoke(eventData.selectedObject.GetComponent<CardContainer>().card);
            Destroy(eventData.selectedObject);
        }
    }


}