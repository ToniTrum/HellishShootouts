using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private EnemyWeaponConfig _config;
    [SerializeField] private EnemyStalking _enemyStalking;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;

    private Animator _animator;
    private readonly int _isAttackingHash = Animator.StringToHash("IsAttacking");
    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _enemyStalking = GetComponentInParent<EnemyStalking>();
        _enemyAnimation = GetComponentInParent<EnemyAnimation>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemySpriteRenderer = GetComponentInParent<SpriteRenderer>();
        _animator = _enemyAnimation.GetComponent<Animator>();

        ApplyConfig();
    }

    private void ApplyConfig()
    {
        _spriteRenderer.sprite = _config.sprite;
        transform.localPosition = _config.positionOffset;
    }

    private void Update()
    {
        bool isFlipped = _enemyStalking.DirectionToPlayer.x < 0;
        bool isWalking = _animator.GetBool(_isWalkingHash);

        _spriteRenderer.flipX = isFlipped;

        if (isWalking)
        {
            if (_enemyStalking.DirectionToPlayer != Vector2.zero)
            {
                Vector2 direction = _enemyStalking.DirectionToPlayer;
                float angleOffset = _config.spriteFaceUp ? -90f : 0f;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + angleOffset;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            _config.behavior.Walk(transform, isFlipped);
        }
        else
        {
            transform.rotation = Quaternion.identity;
            _config.behavior.Idle(transform, isFlipped);
        }
    }
}
