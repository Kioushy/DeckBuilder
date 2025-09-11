using UnityEngine;
using System.Collections;

public class SeaSerpentEnemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private Color poisonColor = new Color(11f / 255f, 70f / 255f, 152f);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            baseColor = spriteRenderer.color;
        }
    }
    public void OnAttacked()
    {
        // Riposte immédiate
        HealthBarPlayer.Instance.TakeDamage(1);

        // Applique un poison (dégâts sur 3 secondes)
        StartCoroutine(PoisonEffect());

        // Aura bleue pulsante
        StartCoroutine(PoisonAuraFlash());
    }

    private IEnumerator PoisonEffect()
    {
        int ticks = 3;
        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(1f);
            HealthBarPlayer.Instance.TakeDamage(1);
        }
    }

    private IEnumerator PoisonAuraFlash()
    {
        if (spriteRenderer == null) yield break;

        // 2 pulsations
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = poisonColor;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
