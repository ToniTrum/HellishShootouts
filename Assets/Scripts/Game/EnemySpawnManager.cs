using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        [Range(0f, 100f)] public float spawnChance;
        public RuntimeAnimatorController spawnAnimator;
    }

    [SerializeField] private GameObject _stateAnimatorPrefab;
    [SerializeField] private EnemySpawnData[] enemyTypes;
    [SerializeField] private Vector2 spawnAreaSize = new(23f, 22f);
    [SerializeField] private float spawnInterval = 5f;

    private float nextSpawnTime;

    private void Start()
    {
        SelectRandomEnemy();
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SelectRandomEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SelectRandomEnemy()
    {
        float totalChance = 0f;
        foreach (var enemy in enemyTypes)
        {
            totalChance += enemy.spawnChance;
        }

        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        foreach (var enemy in enemyTypes)
        {
            if (enemy.enemyPrefab != null)
            {
                cumulativeChance += enemy.spawnChance;
                if (randomValue <= cumulativeChance)
                {
                    Vector3 randomPoint = new(
                        Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                        Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                        -1
                    );
                    Vector3 spawnPosition = transform.position + randomPoint;

                    SpawnStateAnimator(enemy.enemyPrefab, spawnPosition, enemy.spawnAnimator);
                    break;
                }
            }
        }
    }

    private void SpawnStateAnimator(GameObject enemyPrefab, Vector3 position, RuntimeAnimatorController overrideController = null)
    {
        GameObject animationInstance = Instantiate(_stateAnimatorPrefab, position, Quaternion.identity);
        Animator animator = animationInstance.GetComponent<Animator>();
        if (animator != null && overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController;
        }

        StateAnimation spawnAnimation = animationInstance.GetComponent<StateAnimation>();
        float animationDuration = spawnAnimation.GetAnimationDuration();
        if (animationDuration <= 0)
        {
            Debug.LogWarning("Animation duration not detected, using default 1 second.");
            animationDuration = 1f;
        }

        StartCoroutine(SpawnEnemy(enemyPrefab, position, animationDuration));
    }

    private IEnumerator SpawnEnemy(GameObject enemyPrefab, Vector3 position, float animationDuration)
    {
        yield return new WaitForSeconds(animationDuration);
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1));
    }
}
