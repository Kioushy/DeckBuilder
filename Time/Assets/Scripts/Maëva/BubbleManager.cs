using System.Collections;
using UnityEngine;

/// <summary>
/// Spawner dynamique de bulles basé sur un prefab chargé depuis Resources.
/// Même logique que dans MainMenuController, mais avec comportement animé.
/// </summary>
public class DynamicBubbleSpawner : MonoBehaviour
{
    [Header("Prefab (Resources)")]
    public string bubblePrefabPath = "Bubulle_0"; // chemin du prefab dans Resources

    [Header("Paramètres de spawn")]
    public float spawnInterval = 0.5f;      // intervalle entre deux bulles
    public Vector2 spawnArea = new Vector2(8f, 5f); // zone autour du spawner
    public int maxSimultaneousBubbles = 50; // nombre max de bulles

    [Header("Paramètres des bulles")]
    public float minScale = 0.5f;
    public float maxScale = 1.2f;
    public float minRiseSpeed = 0.6f;
    public float maxRiseSpeed = 1.6f;
    public float driftAmount = 0.3f;
    public float bubbleLifetime = 5f;

    private GameObject bubblePrefab;
    private int currentCount = 0;

    private void Awake()
    {
        // Chargement du prefab depuis Resources
        bubblePrefab = Resources.Load<GameObject>(bubblePrefabPath);
        if (bubblePrefab == null)
        {
            Debug.LogError($" Prefab '{bubblePrefabPath}' introuvable dans Resources/");
            enabled = false;
        }
    }

    private void Start()
    {
        // Lancement du spawn continu
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentCount < maxSimultaneousBubbles)
            {
                SpawnBubble();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBubble()
    {
        Vector3 spawnPos = transform.position + new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            Random.Range(-spawnArea.y, spawnArea.y),
            0f);

        GameObject bubble = Instantiate(bubblePrefab, spawnPos, Quaternion.identity, transform);

        // Scale aléatoire
        float scale = Random.Range(minScale, maxScale);
        bubble.transform.localScale = Vector3.one * scale;

        // Lancer le comportement animé
        StartCoroutine(BubbleBehavior(bubble));

        currentCount++;
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(bubbleLifetime + 0.1f);
        currentCount = Mathf.Max(0, currentCount - 1);
    }

    private IEnumerator BubbleBehavior(GameObject bubble)
    {
        float age = 0f;
        float riseSpeed = Random.Range(minRiseSpeed, maxRiseSpeed);
        float rotSpeed = Random.Range(-30f, 30f);
        SpriteRenderer sr = bubble.GetComponent<SpriteRenderer>();
        Color startColor = sr != null ? sr.color : Color.white;

        while (age < bubbleLifetime)
        {
            float dt = Time.deltaTime;
            age += dt;

            // Montée et drift latéral
            Vector3 move = new Vector3(
                Mathf.Sin(Time.time * 1.5f) * driftAmount * dt,
                riseSpeed * dt,
                0f);
            bubble.transform.Translate(move, Space.World);

            // Rotation aléatoire
            bubble.transform.Rotate(Vector3.forward * rotSpeed * dt);

            // Fade alpha
            if (sr != null)
            {
                float t = age / bubbleLifetime;
                Color c = startColor;
                c.a = Mathf.Lerp(startColor.a, 0f, t);
                sr.color = c;
            }

            yield return null;
        }

        Destroy(bubble);
    }
}
