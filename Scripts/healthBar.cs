using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarForeground;
    public float currentHealth;
    public float maxHealth = 100;

    private void Update()
    {
        // Calculate the current health percentage
        float healthPercentage = currentHealth / maxHealth;

        // Set the health bar's width based on the current health percentage
        healthBarForeground.rectTransform.localScale = new Vector3(healthPercentage, 1, 1);
    }

    // Call this method to update the health from other scripts
    public void SetHealth(float health)
    {
        currentHealth = health;
    }
}
