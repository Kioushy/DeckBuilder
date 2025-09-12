using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DropZone : MonoBehaviour, IDropHandler
{
    public DeckManager _DeckM;
    public TurnManager _TurnM;
    public HandManager _HandM;
    public Health healthE;

    public static event Action<Card> Discard;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject cardPlayed = eventData.selectedObject;


            cardPlayed.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x / 2, transform.GetComponent<RectTransform>().sizeDelta.y / -2);
            cardPlayed.GetComponent<CardContainer>().LaunchEffect();
            Debug.Log("Drop : " + cardPlayed.name);
            _DeckM.SendToCemetery(cardPlayed.GetComponent<CardContainer>().card);

            if (healthE.currentHealth > 0)
            {
                if (_HandM.currentCardInHand == 0)
                {
                    _TurnM.EnemyTurn();
                }
            }
            

            Destroy(GameManager.instance.currentSelectedObject);
        }
    }


}