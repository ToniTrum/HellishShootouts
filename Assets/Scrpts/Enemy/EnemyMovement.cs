using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;

    private Rigidbody2D _rigidbody;
    private EnemyStalking _enemyStalking;

    public Vector2 targetDirection;
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
            targetDirection = _enemyStalking.DirectionToPlayer;
        }
        else
        {
            targetDirection = Vector2.zero;
        }
    }

    private void SetVelocity()
    {
        _rigidbody.velocity = targetDirection * _speed;
    }
}