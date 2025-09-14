using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropOnEnemy2D : MonoBehaviour, IEndDragHandler
{
    [Header("Ciblage")]
    [SerializeField] private LayerMask enemyLayer;       // Doit pointer vers la layer "turnM"
    [SerializeField] private Camera worldCamera;         // Ta Main Camera (celle qui voit la scène 2D)
    [SerializeField] private float aimAssistRadius = 0.35f; // Tolérance si tu lâches "près" de l'ennemi

    [Header("Référence effet")]
    [SerializeField] private CardContainer cardContainer; // Ton script existant

    private void Awake()
    {
        if (worldCamera == null) worldCamera = Camera.main;
        if (cardContainer == null) cardContainer = GetComponent<CardContainer>();

        // Si l’inspector n’a pas la layer, on essaie de l’auto-déduire
        if (enemyLayer.value == 0)
        {
            int idx = LayerMask.NameToLayer("turnM");
            if (idx != -1) enemyLayer = 1 << idx;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (worldCamera == null)
        {
            Debug.LogError("[CardDropOnEnemy2D] Aucune worldCamera (tag MainCamera ?).");
            return;
        }

        // 1) Ray caméra -> souris (fiable même si les colliders sont petits)
        Ray ray = worldCamera.ScreenPointToRay(eventData.position);
        RaycastHit2D hitRay = Physics2D.GetRayIntersection(ray, Mathf.Infinity, enemyLayer);
        Collider2D targetCol = hitRay.collider;

        // 2) Si rien touché, on essaie une zone de tolérance au point lâché (OverlapCircle)
        if (targetCol == null)
        {
            Vector3 worldPoint = worldCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0f));
            worldPoint.z = 0f;
            targetCol = Physics2D.OverlapCircle(worldPoint, aimAssistRadius, enemyLayer);
        }

        if (targetCol != null)
        {
            Debug.Log($"[CardDropOnEnemy2D] Ennemi détecté: {targetCol.name} → LaunchEffect()");
            if (cardContainer != null)
            {
                cardContainer.LaunchEffect(); // ← Ton code existant applique dégâts/bouclier/soin
                // Si tu veux retirer la carte après usage, défausse-la/détruis-la ici.
                // Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("[CardDropOnEnemy2D] CardContainer manquant sur la carte.");
            }
        }
        else
        {
            Debug.Log("[CardDropOnEnemy2D] Aucune cible sous le curseur au lâcher.");
        }
    }
}
