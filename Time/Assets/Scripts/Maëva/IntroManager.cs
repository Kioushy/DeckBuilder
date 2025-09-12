using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class IntroManager : MonoBehaviour
{
    private GameObject coralLeft;
    private GameObject coralRight;
    private TextMeshProUGUI oxygenText; // dans l'UI du jeu

    private float moveDuration = 5f;
    private Vector3 leftTarget = new Vector3(-18f, 0f, 0f);
    private Vector3 rightTarget = new Vector3(18f, 0f, 0f);

    private bool waitingForClick = false;

    private void Start()
    {
        // Trouve les objets dynamiquement
        coralLeft = GameObject.Find("CoralLeft");
        coralRight = GameObject.Find("CoralRight");

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
    }
}
