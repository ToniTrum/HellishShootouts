using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Slider manaSlider;

    private void Start()
    {
        // UI initialization
        healthSlider.maxValue = playerStats.MaxHealth;
        staminaSlider.maxValue = playerStats.MaxStamina;
        manaSlider.maxValue = playerStats.MaxMana;

        // Event subscribes
        playerStats.OnHealthChanged += (current, max) => healthSlider.value = current;
        playerStats.OnStaminaChanged += (current, max) => staminaSlider.value = current;
        playerStats.OnManaChanged += (current, max) => manaSlider.value = current;
        playerStats.OnDeath += OnPlayerDeath;

        // Start values
        healthSlider.value = playerStats.CurrentHealth;
        staminaSlider.value = playerStats.CurrentStamina;
        manaSlider.value = playerStats.CurrentMana;
    }

    private void Update()
    {
        // TEST BEGIN
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerStats.TakeDamage(10f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.Heal(20f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerStats.UseStamina(20f))
        {
            Debug.Log("Used Stamina for sprint!");
        }
        if (Input.GetKeyDown(KeyCode.Q) && playerStats.UseMana(15f))
        {
            Debug.Log("Cast a spell!");
        }
        // TEST END
    }

    private void OnPlayerDeath()
    {
        Debug.Log("Player has died!");
    }
}
