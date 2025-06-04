using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float maxMana = 100f;

    [Header("Regeneration Rates")]
    [SerializeField] private float healthRegenRate = 0f; // HP/sec
    [SerializeField] private float staminaRegenRate = 5f; // Stamina/sec
    [SerializeField] private float manaRegenRate = 2f; // Mana/sec
    [SerializeField] private float regenDelay = 2f;

    private float currentHealth;
    private float currentStamina;
    private float currentMana;
    private float lastDamageTime;
    private float lastStaminaUseTime;
    private float lastManaUseTime;

    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action<float, float> OnStaminaChanged; // (current, max)
    public event Action<float, float> OnManaChanged; // (current, max)
    public event Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
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

    public bool UseStamina(float amount)
    {
        if (amount <= 0 || currentStamina < amount) return false;

        currentStamina = Mathf.Clamp(currentStamina - amount, 0, maxStamina);
        lastStaminaUseTime = Time.time;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        return true;
    }

    public void RestoreStamina(float amount)
    {
        if (amount <= 0) return;

        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public bool UseMana(float amount)
    {
        if (amount <= 0 || currentMana < amount) return false;

        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
        lastManaUseTime = Time.time;
        OnManaChanged?.Invoke(currentMana, maxMana);
        return true;
    }

    public void RestoreMana(float amount)
    {
        if (amount <= 0) return;

        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    private void RegenerateStats()
    {
        float timeSinceLastDamage = Time.time - lastDamageTime;
        float timeSinceLastStaminaUse = Time.time - lastStaminaUseTime;
        float timeSinceLastManaUse = Time.time - lastManaUseTime;

        if (timeSinceLastDamage > regenDelay && currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        if (timeSinceLastStaminaUse > regenDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        }

        if (timeSinceLastManaUse > regenDelay && currentMana < maxMana)
        {
            currentMana += manaRegenRate * Time.deltaTime;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }
    }

    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;
    public float CurrentMana => currentMana;
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;
    public float MaxMana => maxMana;
}
