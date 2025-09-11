using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    private GameObject coralLeft;
    private GameObject coralRight;
    private GameObject oxygenText;

    private float moveDuration = 5f;
    private Vector3 leftTarget = new Vector3(-18f, 0f, 0f);
    private Vector3 rightTarget = new Vector3(18f, 0f, 0f);

    private bool waitingForClick = false;

    private void Start()
    {
        // Trouve les objets dynamiquement
        coralLeft = GameObject.Find("CoralLeft");
        coralRight = GameObject.Find("CoralRight");
        oxygenText = GameObject.Find("OxygenTutorialText");

        if (oxygenText != null) oxygenText.SetActive(false);

        // Pause totale du jeu
        Time.timeScale = 0f;

        // Lancer la séquence d’intro
        StartCoroutine(IntroSequence());
    }

    private void Update()
    {
        // Si on attend un clic pour reprendre
        if (waitingForClick && Input.GetMouseButtonDown(0))
        {
            if (oxygenText != null) oxygenText.SetActive(false);
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
            elapsed += Time.unscaledDeltaTime; // On utilise le temps "réel" car le jeu est en pause
            float t = elapsed / moveDuration;

            coralLeft.transform.position = Vector3.Lerp(startLeft, leftTarget, t);
            coralRight.transform.position = Vector3.Lerp(startRight, rightTarget, t);

            yield return null;
        }

        // Quand les coraux ont fini -> afficher le texte
        if (oxygenText != null) oxygenText.SetActive(true);

        // Attendre le clic du joueur
        waitingForClick = true;
    }
}
