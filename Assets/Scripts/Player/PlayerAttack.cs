using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float timeToAttack = 0.25f;
    [SerializeField] private float attackCooldown = 0.5f;
    private SwordMovement sword;
    
    private GameObject attackArea;
    private bool attacking = false;
    private bool canAttack = true;
    private float timer = 0f;
    private readonly int _isAttackHash = Animator.StringToHash("isAttack");

    
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackArea.GetComponent<AttackArea>().SetDamage(attackDamage);
        attackArea.SetActive(false);
        sword = GetComponentInChildren<SwordMovement>();
    }
    
    void Update()
    {
        HandleAttackInput();
        HandleAttackState();
    }

    private void HandleAttackInput()
    {
        if(Input.GetKeyDown(KeyCode.J) && !attacking)
        {
            Attack();
        }
    }

    private void HandleAttackState()
    {
        if(attacking)
        {
            timer += Time.deltaTime;
            
            if(timer >= timeToAttack)
            {
                attackArea.SetActive(false);
            }
            
            if(timer >= attackCooldown)
            {
                timer = 0f;
                attacking = false;
                canAttack = true;
            }
        }
    }
    
    private void Attack()
    {
        attacking = true;
        canAttack = false;
        
        Debug.Log(sword);
        attackArea.SetActive(true);
        sword.ChangeAnimation("PlayerSwordAttack");
        
        timer = 0f;
    }

    public void SetAttackDamage(float newDamage)
    {
        attackDamage = newDamage;
        attackArea.GetComponent<AttackArea>().SetDamage(newDamage);
    }
}