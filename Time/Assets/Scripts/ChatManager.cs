using System.Collections;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public TurnManager enemies;
    private TextMeshProUGUI textMesh;

    public void Start()
    {
       textMesh = GetComponent<TextMeshProUGUI>();
    }


    public IEnumerator Chat(string message)
    {
        yield return new WaitForSeconds(2f);
        textMesh.text = message;
        yield return new WaitForSeconds(2f);
        textMesh.text = "";

    }

    public void UpdateChat(string message)
    {
        StartCoroutine(Chat(message));
    }

}
