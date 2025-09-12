using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject currentSelectedObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<PauseMenuController>(); // ajoute automatiquement le script pause
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
