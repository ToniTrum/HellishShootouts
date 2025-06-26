using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _staminaBar;

    [Header("Death")]
    [SerializeField] private GameObject _stateAnimatorPrefab;
    [SerializeField] private RuntimeAnimatorController _deathAnimator;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        playerMovement.enabled = false;
        spriteRenderer.enabled = false;

        DestroySpawnManagerAndEnemies();

        Vector3 position = transform.position;
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
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }

    private void DestroySpawnManagerAndEnemies()
    {
        var spawnManager = GameObject.Find("SpawnManager");
        if (spawnManager != null) Destroy(spawnManager);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
