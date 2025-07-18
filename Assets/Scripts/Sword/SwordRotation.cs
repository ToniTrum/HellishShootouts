using UnityEngine;

public class SwordRotation : MonoBehaviour
{
    public Transform nearestEnemy;
    public Vector3 directionToEnemy;

    private void Update()
    {
        UpdateNearestEnemy();
        RotateToEnemy();
    }

    private void UpdateNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            float shortestDistance = float.MaxValue;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestEnemy = enemy.transform;
                    }
                }
            }

            nearestEnemy = closestEnemy;
        }
        else
        {
            nearestEnemy = null;
            directionToEnemy = Vector3.zero;
        }
    }

    private void RotateToEnemy()
    {
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            directionToEnemy = direction;

            Vector3 localScale = transform.localScale;
            if (angle > 90 || angle < -90)
            {
                localScale.y = -1f;
            }
            else
            {
                localScale.y = 1f;
            }
            transform.localScale = localScale;
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
