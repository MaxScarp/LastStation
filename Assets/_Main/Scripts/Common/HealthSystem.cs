using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler<float> OnHealthChanged;

    [SerializeField] private float maxHealth = 1.0f;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        OnHealthChanged?.Invoke(this, currentHealth);
    }

    public float GetCurrentHealth() => currentHealth;
}
