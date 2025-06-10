using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public GameObject enemyPrefab;
        [Range(0f, 100f)] public float spawnChance;
    }

    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private EnemySpawnData[] enemyTypes;
    [SerializeField] private Vector2 spawnAreaSize = new(23f, 22f);
    [SerializeField] private float spawnInterval = 5f;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime && spawner != null)
        {
            SpawnRandomEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnRandomEnemy()
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

                    spawner.enemyPrefab = enemy.enemyPrefab;
                    spawner.SpawnEnemy(spawnPosition);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1));
    }
}
