using UnityEngine;
using UnityEngine.UI;

// Ce script gère l'affichage de la barre de vie d'un ennemi dans le jeu.
public class HealthBarEnemy : MonoBehaviour
{
    // Référence à la barre de vie (le composant Slider dans Unity)
    private Slider _healthBar;

    // Cette méthode est appelée automatiquement au début du jeu
    private void Start()
    {
        // On récupère le composant Slider attaché à l'objet
        _healthBar = GetComponent<Slider>();
    }

    // Cette méthode met à jour l'affichage de la barre de vie
    // maxHealth : la vie maximale de l'ennemi
    // currentHealth : la vie actuelle de l'ennemi
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        // On calcule le pourcentage de vie restant et on l'affiche sur la barre
        _healthBar.value = currentHealth / maxHealth;
    }


}
