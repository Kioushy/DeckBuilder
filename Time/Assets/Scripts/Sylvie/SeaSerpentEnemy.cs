using UnityEngine;
using System.Collections;

public class SeaSerpentEnemy : MonoBehaviour
{
    public void OnAttacked()
    {
        // Riposte immédiate
        HealthBarPlayer.Instance.TakeDamage(1);

        // Applique un poison (dégâts sur 3 secondes)
        StartCoroutine(PoisonEffect());
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
}
