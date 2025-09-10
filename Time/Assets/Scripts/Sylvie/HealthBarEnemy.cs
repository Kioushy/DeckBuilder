using UnityEngine;
using UnityEngine.UI;

public class HealthBarEnemy : MonoBehaviour
{

    private Slider _healthBar;

    private void Start()
    {
        _healthBar = GetComponent<Slider>();
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _healthBar.value = currentHealth / maxHealth;
    }


}
