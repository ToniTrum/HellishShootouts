using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    
    private List<EnemyStats> enemiesInArea = new List<EnemyStats>();
    private List<EnemyStats> damagedEnemies = new List<EnemyStats>();
    
    private void OnEnable()
    {
        ClearDamagedList();
        DamageEnemiesInArea();
    }
    
    
    private void OnTriggerEnter2D(Collider2D c)
    {
        Console.WriteLine("trigger enter");
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
        Debug.Log("trigger exit");
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
            Debug.Log($"damaged!!!!!! Enemy: {enemy.Yield()}");
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