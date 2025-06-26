using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private EnemyStats _enemyStats;

    [SerializeField] private GameObject _stateAnimatorPrefab;
    [SerializeField] private RuntimeAnimatorController _deathAnimator;

    private void Awake()
    {
        _enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        _enemyStats.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        Vector3 position = transform.position;
        Destroy(gameObject);

        GameObject animationInstance = Instantiate(_stateAnimatorPrefab, position, Quaternion.identity);

        Animator animator = animationInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = _deathAnimator;
    }
}
