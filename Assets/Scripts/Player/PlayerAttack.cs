using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float timeToAttack = 0.25f;
    
    private GameObject attackArea;
    private bool attacking = false;
    private float timer = 0f;
    
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackArea.GetComponent<AttackArea>().SetDamage(attackDamage);
        attackArea.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J) && !attacking)
        {
            Attack();
        }

        if(attacking)
        {
            timer += Time.deltaTime;
            if(timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(false);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        attackArea.SetActive(true);
    }

    // Other scripts can change the stat
    public void SetAttackDamage(float newDamage)
    {
        attackDamage = newDamage;
        attackArea.GetComponent<AttackArea>().SetDamage(newDamage);
    }
}