using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 1.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthBarPlayer health = collision.GetComponent<HealthBarPlayer>();
        if (health != null)
        {
            health.UpdateHealth(-damage);
        }
    }
}
