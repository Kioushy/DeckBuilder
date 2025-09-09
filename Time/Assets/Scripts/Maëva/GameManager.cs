using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
