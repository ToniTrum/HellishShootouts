using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;
    private EnemyStalking _enemyStalking;

    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");
    private float _lastAttackTime;
    private bool _isAttacking = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyStalking = GetComponent<EnemyStalking>();
    }

    private void Update()
    {
        bool isWalking = _enemyMovement.GetVelocity().magnitude > 0.1f;
        _animator.SetBool(_isWalkingHash, isWalking);

        float distanceToPlayer = (_enemyStalking.player.position - transform.position).magnitude;
        if (!_enemyStalking.StalkingOfPlayer 
            && distanceToPlayer <= _enemyStalking.attackDistance 
            && Time.time >= _lastAttackTime + attackCooldown)
        {
            _animator.SetBool(_isAttackingHash, true);
            _lastAttackTime = Time.time;
            _isAttacking = true;
        }

        if (!_isAttacking)
        {
            _spriteRenderer.flipX = _enemyStalking.DirectionToPlayer.x < 0;
        }
        _enemyMovement.IsAttacking = _isAttacking;
    }

    public void EndAttack()
    {
        _animator.SetBool(_isAttackingHash, false);
        _isAttacking = false;
    }
}
