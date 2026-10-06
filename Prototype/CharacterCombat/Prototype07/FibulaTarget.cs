using UnityEngine;

public class FibulaTarget : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log(
            $"{gameObject.name} recebeu {damage} de dano. " +
            $"Vida: {currentHealth}/{maxHealth}"
        );

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} foi derrotado.");
        Destroy(gameObject);
    }
}