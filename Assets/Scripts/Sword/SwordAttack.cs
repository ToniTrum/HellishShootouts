using UnityEngine;


public class SwordAttack : MonoBehaviour
{
    private Animator _animator;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Attack()
    {
        _animator.SetTrigger("AttackTrigger");
    }
}