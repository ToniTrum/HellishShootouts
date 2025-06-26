using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float timeToAttack = 0.25f;
    [SerializeField] private float attackCooldown = 0.5f;
    
    [Header("Sword References")]
    [SerializeField] private Animator swordAnimator;
    
    private GameObject attackArea;
    private bool attacking = false;
    private bool canAttack = true;
    private float timer = 0f;
    private readonly int _isAttackHash = Animator.StringToHash("isAttack");

    private void Awake()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            // Если аниматор не на корневом объекте (т.е. на дочернем)
            if (anim.transform != transform)
            {
                swordAnimator = anim;
                break;
            }
        }
        if (swordAnimator == null)
        {
            Debug.LogError("Sword animator not found in children!");
        }
    }
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        attackArea.GetComponent<AttackArea>().SetDamage(attackDamage);
        attackArea.SetActive(false);
        
        if(swordAnimator == null)
        {
            swordAnimator = GetComponentInChildren<Animator>();

            if(swordAnimator == null)
            {
                Debug.LogError("Sword Animator not found! Forgor to assign?");
            }
        }
        foreach (AnimatorControllerParameter param in swordAnimator.parameters)
        {
            Debug.Log($"Found animator on object: {swordAnimator.gameObject.name}");
        }
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
                swordAnimator.SetBool("isAttack", false);
            }
        }
    }

    private void Attack()
    {
        attacking = true;
        canAttack = false;
        
        attackArea.SetActive(true);
        
        if(swordAnimator != null)
        {
            swordAnimator.SetBool("isAttack", true);
        }
        else
        {
            Debug.LogWarning("Sword animator missing!");
        }
        
        timer = 0f;
    }

    public void SetAttackDamage(float newDamage)
    {
        attackDamage = newDamage;
        attackArea.GetComponent<AttackArea>().SetDamage(newDamage);
    }
}