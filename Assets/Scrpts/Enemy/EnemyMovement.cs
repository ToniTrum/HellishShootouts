using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Rigidbody2D _rigidbody;
    private EnemyStalking _enemyStalking;

    private Vector2 _targetDirection;

    public bool IsAttacking { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _enemyStalking = GetComponent<EnemyStalking>();
    }

    private void FixedUpdate()
    {
        UpdateTargetDirection();
        SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if (_enemyStalking.StalkingOfPlayer && !IsAttacking)
        {
            _targetDirection = _enemyStalking.DirectionToPlayer;
        }
        else
        {
            _targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        _rigidbody.velocity = _targetDirection * _speed;
    }

    public Vector2 GetVelocity() => _rigidbody.velocity;
}