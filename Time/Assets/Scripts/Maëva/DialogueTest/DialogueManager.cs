using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    private DialogueLine[] currentLines;
    private int index = 0;
    private System.Action onDialogueEnd; // callback quand le dialogue se termine

    // Méthode publique que DialogueFlowManager peut appeler
    public void StartDialogue(DialogueLine[] lines, System.Action callback = null)
    {
        currentLines = lines;
        index = 0;
        onDialogueEnd = callback;

        nextButton.gameObject.SetActive(true);

        nextButton.onClick.RemoveAllListeners(); // au cas où
        nextButton.onClick.AddListener(NextLine);

        ShowLine();
    }

    void ShowLine()
    {
        if (currentLines == null || currentLines.Length == 0) return;

        var line = currentLines[index];
        speakerText.text = line.speaker;
        dialogueText.text = line.text;
    }

    void NextLine()
    {
        index++;
        if (index < currentLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        speakerText.text = "";
        dialogueText.text = "";
        nextButton.gameObject.SetActive(false);

        onDialogueEnd?.Invoke(); //  Appel du callback (ex: StartCombat)
    }
}
