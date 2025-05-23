using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private EnemyWeaponConfig _config;
    [SerializeField] private EnemyStalking _enemyStalking;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Animator _enemyAnimator;
    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");

    private Vector3 _initialPosition;
    private float _attackStartTime;
    private bool _isAttacking;
    private Vector2 _attackDirection;

    private void Awake()
    {
        _enemyStalking = GetComponentInParent<EnemyStalking>();
        _enemyAnimation = GetComponentInParent<EnemyAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyAnimator = _enemyAnimation.GetComponent<Animator>();

        _initialPosition = transform.localPosition;
        ApplyConfig();
    }

    private void ApplyConfig()
    {
        _spriteRenderer.sprite = _config.sprite;
    }

    private void Update()
    {
        bool isFlipped = _enemyStalking.DirectionToPlayer.x < 0;
        bool isWalking = _enemyAnimator.GetBool(_isWalkingHash);
        bool isAttacking = _enemyAnimator.GetBool(_isAttackingHash);

        _spriteRenderer.flipX = isFlipped;

        if (isAttacking && !_isAttacking)
        {
            _attackStartTime = Time.time;
            _isAttacking = true;
            _attackDirection = _enemyStalking.DirectionToPlayer;
        }

        if (isWalking)
        {
            float angle = isFlipped ? 180 : 0;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _config.behavior.Walk(transform, isFlipped);
        }
        else if (isAttacking && _isAttacking)
        {
            transform.rotation = Quaternion.identity;
            _config.behavior.Attack
            (
                transform, 
                isFlipped, 
                _initialPosition, 
                _attackStartTime, 
                ref _isAttacking, 
                _attackDirection
            );
            if (!_isAttacking)
            {
                _enemyAnimation.EndAttack();
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            _config.behavior.Idle(transform, isFlipped);
        }
    }
}
