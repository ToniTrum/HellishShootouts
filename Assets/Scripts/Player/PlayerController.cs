using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        playerStats.OnHealthChanged += UpdateHealthBar;
        playerStats.OnStaminaChanged += UpdateStaminaBar;
        playerStats.OnDeath += OnPlayerDeath;

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
