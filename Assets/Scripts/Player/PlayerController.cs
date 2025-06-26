using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;

    [Header("UI Elements")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _staminaBar;

    [Header("Death")]
    [SerializeField] private GameObject _stateAnimatorPrefab;
    [SerializeField] private RuntimeAnimatorController _deathAnimator;
    [Tooltip("Name of the scene to load after death delay")]
    [SerializeField] private string _menuSceneName = "Menu";
    [Tooltip("Delay before loading menu scene after death (seconds)")]
    [SerializeField] private float _deathLoadDelay = 1.5f;

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
        Vector3 deathPosition = transform.position;

        if (_stateAnimatorPrefab != null)
        {
            GameObject animGO = Instantiate(_stateAnimatorPrefab, deathPosition, Quaternion.identity);
            var animator = animGO.GetComponent<Animator>();
            if (animator != null && _deathAnimator != null)
            {
                animator.runtimeAnimatorController = _deathAnimator;
            }
        }

        StartCoroutine(LoadMenuAfterDelay(_deathLoadDelay));

        Destroy(gameObject);
        DestroySpawnManagerAndEnemies();
    }

    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!string.IsNullOrEmpty(_menuSceneName))
        {
            SceneManager.LoadScene(_menuSceneName);
        }
    }

    private void DestroySpawnManagerAndEnemies()
    {
        var spawnMgr = GameObject.FindWithTag("SpawnManager");
        if (spawnMgr != null) Destroy(spawnMgr);

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
