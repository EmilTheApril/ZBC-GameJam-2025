using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void RemoveHealth(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Death();
        }

    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
