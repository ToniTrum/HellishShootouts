using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackArea : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    
    private List<EnemyStats> enemiesInArea = new List<EnemyStats>();
    private List<EnemyStats> damagedEnemies = new List<EnemyStats>();
    
    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    
    private void OnEnable()
    {
        ClearDamagedList();
        DamageEnemiesInArea();
    }

    private void OnTriggerEnter(Collider c)
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

    private void OnTriggerExit(Collider other)
    {
        Console.WriteLine("trigger exit");
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
            Console.WriteLine("damaged!!!!!!");
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