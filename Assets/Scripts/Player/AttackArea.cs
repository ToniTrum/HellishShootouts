using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f; // Радиус обнаружения врагов
    [SerializeField] private float damage = 10f;
    
    private List<EnemyStats> enemiesInArea = new List<EnemyStats>();
    private List<EnemyStats> damagedEnemies = new List<EnemyStats>();
    
    private void OnEnable()
    {
        ClearDamagedList();
        DamageEnemiesInArea();
    }
    
    private void Update()
    {
        RotateTowardsNearestEnemyInRadius();
    }
    
    private void RotateTowardsNearestEnemyInRadius()
    {

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Enemy"));
        if (hits.Length == 0) return;

        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;
        foreach (var hit in hits)
        {
            EnemyStats enemy = hit.GetComponent<EnemyStats>();
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }
        if (nearestEnemy is not null)
        {
            Vector2 direction = nearestEnemy.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
        
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyStats enemy = c.GetComponentInParent<EnemyStats>();
            if (enemy != null && !enemiesInArea.Contains(enemy))
            {
                enemiesInArea.Add(enemy);
                TryDamageEnemy(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyStats enemy = other.GetComponentInParent<EnemyStats>();
            if (enemy != null && enemiesInArea.Contains(enemy))
            {
                enemiesInArea.Remove(enemy);
            }
        }
    }

    private void DamageEnemiesInArea()
    {
        foreach (EnemyStats enemy in enemiesInArea)
        {
            if (enemy != null)
            {
                TryDamageEnemy(enemy);
            }
        }
    }

    private void TryDamageEnemy(EnemyStats enemy)
    {
        if (!damagedEnemies.Contains(enemy))
        {
            enemy.TakeDamage(damage);
            damagedEnemies.Add(enemy);
        }
    }

    private void ClearDamagedList()
    {
        damagedEnemies.Clear();
        
        for (int i = enemiesInArea.Count - 1; i >= 0; i--)
        {
            if (enemiesInArea[i] == null)
            {
                enemiesInArea.RemoveAt(i);
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }
}