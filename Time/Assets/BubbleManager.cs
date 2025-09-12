using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Prefab de la bulle (dans Resources/Bubble)")]
    public string bubblePrefabPath = "BubblePrefab"; // ton prefab doit �tre dans Resources/

    [Header("Param�tres des bulles")]
    public float spawnInterval = 1.5f;   // temps entre chaque bulle
    public Vector2 spawnArea = new Vector2(10f, 5f); // zone al�atoire (X,Y)
    public float minScale = 0.5f;
    public float maxScale = 1.5f;

    private GameObject bubblePrefab;
    private float timer;

    private void Awake()
    {
        // Charger ton prefab automatiquement
        bubblePrefab = Resources.Load<GameObject>(bubblePrefabPath);
        if (bubblePrefab == null)
        {
            Debug.LogError("Prefab Bubble introuvable dans Resources/" + bubblePrefabPath);
        }
    }

    private void Update()
    {
        if (bubblePrefab == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        // Position al�atoire autour du spawner
        Vector3 spawnPos = transform.position +
            new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
                        Random.Range(-spawnArea.y, spawnArea.y),
                        0f);

        GameObject bubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity);

        // Taille al�atoire
        float scale = Random.Range(minScale, maxScale);
        bubble.transform.localScale = Vector3.one * scale;

        // D�truire la bulle apr�s un certain temps
        Destroy(bubble, 5f);
    }
}
