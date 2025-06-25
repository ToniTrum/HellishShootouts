using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxStamina = 100f;

    [Header("Regeneration Rates")]
    [SerializeField] private float healthRegenRate = 0f; // HP/sec
    [SerializeField] private float staminaRegenRate = 5f; // Stamina/sec
    [SerializeField] private float regenDelay = 2f;

    private float currentHealth;
    private float currentStamina;
    private float lastDamageTime;
    private float lastStaminaUseTime;

    public event Action<float, float> OnHealthChanged; // (current, max)
    public event Action<float, float> OnStaminaChanged; // (current, max)
    public event Action OnDeath;

    // Camera shake
    [SerializeField] private float dmgShakeIntensity = 5f; // Intensity when player receives dmg
    [SerializeField] private float dmgShakeTime = 1f; // Duration

    private void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        RegenerateStats();
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;

        //Camera Shake
        CinemachineShake.Instance.ShakeCamera(dmgShakeIntensity, dmgShakeTime);

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





    private void RegenerateStats()
    {
        float timeSinceLastDamage = Time.time - lastDamageTime;
        float timeSinceLastStaminaUse = Time.time - lastStaminaUseTime;

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


    }

    public float CurrentHealth => currentHealth;
    public float CurrentStamina => currentStamina;
    public float MaxHealth => maxHealth;
    public float MaxStamina => maxStamina;
}
