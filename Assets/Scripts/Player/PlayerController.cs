using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [Header("UI Elements")]
    [SerializeField] private Image healthBar;    // Image типа Filled
    [SerializeField] private Image staminaBar;   // Image типа Filled

    private void Start()
    {
        // Подписываемся на события изменения статов
        playerStats.OnHealthChanged += UpdateHealthBar;
        playerStats.OnStaminaChanged += UpdateStaminaBar;
        playerStats.OnDeath += OnPlayerDeath;

        // Устанавливаем начальные значения
        UpdateHealthBar(playerStats.CurrentHealth, playerStats.MaxHealth);
        UpdateStaminaBar(playerStats.CurrentStamina, playerStats.MaxStamina);
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (healthBar != null)
            healthBar.fillAmount = current / max;
    }

    private void UpdateStaminaBar(float current, float max)
    {
        if (staminaBar != null)
            staminaBar.fillAmount = current / max;
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
    }
}
