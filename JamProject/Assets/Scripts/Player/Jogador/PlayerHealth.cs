using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthRegenPerSecond = 0f;

    void Start()
    {
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (currentHealth < maxHealth && healthRegenPerSecond > 0)
        {
            currentHealth += healthRegenPerSecond * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("Player Morreu!");
        }
    }
}