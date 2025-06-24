using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private EnemyWeaponConfig _config;
    [SerializeField] private float damageAmount = 10f;

    private EnemyStalking _enemyStalking;
    private EnemyAnimation _enemyAnimation;
    private SpriteRenderer _spriteRenderer;

    private Animator _enemyAnimator;
    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");

    private Vector3 _initialPosition;
    private float _attackStartTime;
    private bool _isAttacking;
    private Vector2 _attackDirection;
    private Collider2D _weaponCollider;
    private bool _isDamageDealtThisAttack;

    private void Awake()
    {
        _enemyStalking = GetComponentInParent<EnemyStalking>();
        _enemyAnimation = GetComponentInParent<EnemyAnimation>();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _weaponCollider = GetComponent<Collider2D>();

        _enemyAnimator = _enemyAnimation.GetComponent<Animator>();

        _weaponCollider.isTrigger = true;
        _weaponCollider.enabled = false;

        _initialPosition = transform.localPosition;
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
            _weaponCollider.enabled = true;
            _isDamageDealtThisAttack = false;
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
                _weaponCollider.enabled = false;
                _isDamageDealtThisAttack = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            _config.behavior.Idle(transform, isFlipped);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isAttacking && !_isDamageDealtThisAttack)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damageAmount);
                _isDamageDealtThisAttack = true;
            }
        }
    }
}
