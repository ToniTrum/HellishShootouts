using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] private float spawnDelay = 0f;

    private float nextSpawnTime;

    public void SpawnEnemy(Vector3 position)
    {
        if (Time.time >= nextSpawnTime && enemyPrefab != null)
        {
            Instantiate(enemyPrefab, position, Quaternion.identity);
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
