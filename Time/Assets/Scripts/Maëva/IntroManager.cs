using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public DeckManager deckManager;
    public HandManager handManager;
    private GameObject coralLeft;
    private GameObject coralRight;
    private TextMeshProUGUI oxygenText; // dans l'UI du jeu

    private float moveDuration = 5f;
    private Vector3 leftTarget = new Vector3(-18f, 0f, 0f);
    private Vector3 rightTarget = new Vector3(18f, 0f, 0f);

    public bool waitingForClick = false;

    private void Start()
    {
        // Trouve les objets dynamiquement
        // coralLeft = GameObject.Find("CoralLeft");
        // coralRight = GameObject.Find("CoralRight");

        // GameObject oxygenGO = GameObject.FindWithTag("oxygenText");
        // if (oxygenGO != null)
        // {
        //     oxygenText = oxygenGO.GetComponent<TextMeshProUGUI>();
        //     oxygenText.gameObject.SetActive(false);
        // }
        // else
        // {
        //     Debug.LogWarning("IntroManager: Impossible de trouver oxygenText avec le tag !");
        // }

        // // Pause totale du jeu
        // Time.timeScale = 1f;

        // // Lancer la s�quence d�intro
        // Debug.Log("intro anim lancer");
        // StartCoroutine(IntroSequence());
        StartLevelIntro();
    }

    private void Update()
    {
        // Si on attend un clic pour reprendre
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            if (oxygenText != null) oxygenText.gameObject.SetActive(false);
            waitingForClick = false;

            // Reprise du jeu
            Time.timeScale = 1f;
        }
    }

    IEnumerator IntroSequence()
    {
        if (coralLeft == null || coralRight == null)
            yield break;

        // Reset positions au début du niveau
        coralLeft.transform.position = new Vector3(0f, 0f, 0f);
        coralRight.transform.position = new Vector3(0f, 0f, 0f);
        if (oxygenText != null) oxygenText.gameObject.SetActive(false);

        float elapsed = 0f;
        Vector3 startLeft = coralLeft.transform.position;
        Vector3 startRight = coralRight.transform.position;

        // Animation des coraux
        while (elapsed < moveDuration)
        {
            elapsed += Time.unscaledDeltaTime; // On utilise le temps "r�el" car le jeu est en pause
            float t = elapsed / moveDuration;

            coralLeft.transform.position = Vector3.Lerp(startLeft, leftTarget, t);
            coralRight.transform.position = Vector3.Lerp(startRight, rightTarget, t);

            yield return null;
        }

        // Quand les coraux ont fini -> afficher le texte
        if (oxygenText != null) oxygenText.gameObject.SetActive(true);

        // Attendre le clic du joueur
        waitingForClick = true;

        TurnManager.Instance.BattleSetup();
    }

    public void StartLevelIntro()
    {
         // Trouve les objets dynamiquement
        coralLeft = GameObject.Find("CoralLeft");
        coralRight = GameObject.Find("CoralRight");
        GameFlowManager.Instance.defeatPanel.SetActive(false);
        GameFlowManager.Instance.victoryPanel.SetActive(false);
        HandManager.Instance.Hand.Clear();
       if (HandManager.Instance.handTransform.childCount > 0) 
       {
            for (int i = 0; i < HandManager.Instance.handTransform.childCount; i++) 
            {
                Destroy(HandManager.Instance.handTransform.GetChild(i).gameObject);
            }
       }

        deckManager.Deck.Clear();
        deckManager.Deck.AddRange(deckManager.cards);
        GameFlowManager.Instance.healthE.FullHeal();
        GameFlowManager.Instance.healthP.FullHeal();

        GameObject oxygenGO = GameObject.FindWithTag("oxygenText");
        if (oxygenGO != null)
        {
            oxygenText = oxygenGO.GetComponent<TextMeshProUGUI>();
            oxygenText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("IntroManager: Impossible de trouver oxygenText avec le tag !");
        }

        // Pause totale du jeu
        Time.timeScale = 1f;

        // Lancer la s�quence d�intro
        Debug.Log("intro anim lancer");
        StartCoroutine(IntroSequence());
    }
}
