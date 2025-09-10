using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour , IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.selectedObject.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x / 2, transform.GetComponent<RectTransform>().sizeDelta.y / -2);
        //    eventData.selectedObject.GetComponent<CardContainer>().LaunchEffect();
            Debug.Log("Drop");
        }
    }


}