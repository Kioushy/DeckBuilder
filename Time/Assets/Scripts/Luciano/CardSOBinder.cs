using UnityEngine;

public class CardSOBinder : MonoBehaviour
{
    [Header("Ta carte (ScriptableObject)")]
    [SerializeField] private Card defaultCard;   // ← glisse ici le SO attendu pour ce prefab

    [Header("Références")]
    [SerializeField] private CardContainer container; // ton script existant

    void Awake()
    {
        if (container == null) container = GetComponent<CardContainer>();

        // Si rien n’est mis dans CardContainer, on le remplit depuis le prefab
        if (container != null && container.card == null && defaultCard != null)
        {
            container.card = defaultCard;
        }
    }

    // (optionnel) Permet de (re)définir la carte par code si tu pioches une carte dynamique
    public void Assign(Card newCard)
    {
        if (container == null) container = GetComponent<CardContainer>();
        container.card = newCard;
#if UNITY_EDITOR
        Debug.Log($"[CardSOBinder] Carte assignée: {(newCard ? newCard.name : "NULL")}");
#endif
    }
}
