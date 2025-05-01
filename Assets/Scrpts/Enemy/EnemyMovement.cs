using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private EnemyStalking _enemyStalking;

    private Vector2 _targetDirection;
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
        if (_enemyStalking.StalkingOfPlayer)
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
        if (_targetDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.velocity = _targetDirection * _speed;
        }
    }
}
