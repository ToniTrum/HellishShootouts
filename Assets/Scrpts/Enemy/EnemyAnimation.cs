using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private EnemyMovement _enemyMovement;

    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        bool isWalking = _enemyMovement.GetVelocity().magnitude > 0.1f;
        _animator.SetBool(_isWalkingHash, isWalking);

        if (isWalking)
        {
            Vector2 velocity = _enemyMovement.GetVelocity();
            _spriteRenderer.flipX = velocity.x < 0;
        }
    }
}
