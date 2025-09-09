using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] private UnityEngine.Canvas canvas;   // ← force le bon type Canvas
    [SerializeField] private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    // Mémo pour pouvoir revenir en arrière si besoin
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector2 originalAnchoredPosition;
    private bool droppedThisDrag;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (canvas == null) canvas = GetComponentInParent<UnityEngine.Canvas>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // On mémorise la position et l'ordre d'affichage avant de commencer le drag
        originalParent = rectTransform.parent;
        originalSiblingIndex = rectTransform.GetSiblingIndex();
        originalAnchoredPosition = rectTransform.anchoredPosition;
        droppedThisDrag = false;

        // Optionnel : mettre la carte au-dessus des autres pendant le drag
        transform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // indispensable pour que les Drop reçoivent l'événement
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Déplacement fluide en UI
        rectTransform.anchoredPosition += eventData.delta / (canvas ? canvas.scaleFactor : 1f);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Cette fonction est appelée sur la "cible" quand on lâche un autre objet dessus
        if (eventData.pointerDrag == null) return;

        var draggedGO   = eventData.pointerDrag;
        var draggedRect = draggedGO.GetComponent<RectTransform>();
        var draggedDD   = draggedGO.GetComponent<DragAndDrop>();
        if (draggedRect == null || draggedDD == null) return;

        // On note que le drop est pris en compte (pour ne pas revert dans OnEndDrag)
        draggedDD.droppedThisDrag = true;

        // Cas courant : deux cartes dans la même main → on échange l'ordre (sibling index)
        if (draggedRect.parent == rectTransform.parent)
        {
            int myIndex    = rectTransform.GetSiblingIndex();
            int otherIndex = draggedRect.GetSiblingIndex();

            rectTransform.SetSiblingIndex(otherIndex);
            draggedRect.SetSiblingIndex(myIndex);
        }
        else
        {
            // Sinon : on remet la carte au même parent que la cible, à côté d'elle
            draggedRect.SetParent(rectTransform.parent, false);
            draggedRect.SetSiblingIndex(rectTransform.GetSiblingIndex());
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Si rien n'a accepté le drop, on revient à l'état initial
        if (!droppedThisDrag)
        {
            rectTransform.SetParent(originalParent, false);
            rectTransform.SetSiblingIndex(originalSiblingIndex);
            rectTransform.anchoredPosition = originalAnchoredPosition;
        }
    }
}
