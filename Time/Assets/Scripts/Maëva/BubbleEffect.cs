using UnityEngine;
using UnityEngine.UI;

public class BubbleEffect : MonoBehaviour
{
    public GameObject bubblePrefab; // ton UI Image existant
    public int bubbleCount = 20;
    public float areaWidth = 800f;
    public float areaHeight = 600f;
    public float speedMin = 50f;
    public float speedMax = 150f;

    void Start()
    {
        if (bubblePrefab == null) return;

        for (int i = 0; i < bubbleCount; i++)
        {
            // Instancie la bulle depuis le prefab
            GameObject bubble = Instantiate(bubblePrefab, transform);
            bubble.SetActive(true); // important car le prefab était désactivé

            // Position aléatoire dans le panel
            bubble.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Random.Range(-areaWidth / 2f, areaWidth / 2f),
                Random.Range(-areaHeight / 2f, areaHeight / 2f)
            );

            // vitesse aléatoire
            float speed = Random.Range(speedMin, speedMax);
            bubble.AddComponent<Bubble>().Init(speed);
        }
    }
}

public class Bubble : MonoBehaviour
{
    private float speed;

    public void Init(float s)
    {
        speed = s;
    }

    void Update()
    {
        // fait monter la bulle
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // détruit la bulle hors écran
        if (transform.localPosition.y > 400f)
            Destroy(gameObject);
    }
}
