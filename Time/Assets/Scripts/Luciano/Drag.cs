using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private UnityEngine.Canvas canvas;   // ← force le bon type Canvas
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    public TurnManager turnM;

    /*
    // Mémo pour pouvoir revenir en arrière si besoin
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector2 originalAnchoredPosition;
    private bool droppedThisDrag;
    */

    private void Awake()
    {
        
        turnM = GameManager.instance.gameObject.GetComponent<TurnManager>();
        rectTransform = GetComponent<RectTransform>();
        if (canvas == null) canvas = transform.parent.transform.parent.transform.parent.GetComponent<Canvas>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (turnM.currentState == TurnManager.State.PlayerTurn)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            GameManager.instance.currentSelectedObject = eventData.selectedObject.gameObject;
        }
    
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (turnM.currentState == TurnManager.State.PlayerTurn)
        {

            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {


    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
