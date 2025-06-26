using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _staminaBar;

    [Header("Death")]
    [SerializeField] private GameObject _stateAnimatorPrefab;
    [SerializeField] private RuntimeAnimatorController _deathAnimator;

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
        if (_healthBar != null)
            _healthBar.fillAmount = current / max;
    }

    private void UpdateStaminaBar(float current, float max)
    {
        if (_staminaBar != null)
            _staminaBar.fillAmount = current / max;
    }

    private void OnPlayerDeath()
    {
        // Где-нибудь в этой функции напиши уничтожение мобов, спавнеров и спавн менеджера

        Vector3 position = transform.position;
        Destroy(gameObject);

        GameObject animationInstance = Instantiate(_stateAnimatorPrefab, position, Quaternion.identity);

        Animator animator = animationInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = _deathAnimator;

        StateAnimation spawnAnimation = animationInstance.GetComponent<StateAnimation>();
        float animationDuration = spawnAnimation.GetAnimationDuration();

        StartCoroutine(EndGame(animationDuration));
    }

    private IEnumerator EndGame(float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration);
        // Пиши конец игры здесь
    }
}
