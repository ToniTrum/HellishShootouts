using System;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Regeneration Rates")]
    [SerializeField] private float healthRegenRate = 0f; // HP/sec
    [SerializeField] private float regenDelay = 2f;

    private float currentHealth;
    private float lastDamageTime;

    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        RegenerateStats();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        lastDamageTime = Time.time;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void RegenerateStats()
    {
        float timeSinceLastDamage = Time.time - lastDamageTime;

        if (timeSinceLastDamage > regenDelay && currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
}
