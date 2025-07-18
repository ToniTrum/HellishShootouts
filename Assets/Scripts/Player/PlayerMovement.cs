using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SwordRotation _swordRotation;

    public Vector2 movementInput;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _swordRotation = GetComponentInChildren<SwordRotation>();
    }

    private void FixedUpdate()
    {
        _smoothedMovementInput = Vector2.SmoothDamp
        (
            _smoothedMovementInput,
            movementInput,
            ref _movementInputSmoothVelocity,
            0.1f
        );

        _rigidbody.linearVelocity = _smoothedMovementInput * _speed;
    }

    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
        bool isWalk = movementInput.magnitude > 0;
        _animator.SetBool("isWalk", isWalk);

        if (isWalk)
        {
            if (_swordRotation.nearestEnemy != null)
            {
                UpdatePlayerDirection(Mathf.Sign(_swordRotation.directionToEnemy.x));
            }
            else
            {
                UpdatePlayerDirection(Mathf.Sign(movementInput.x));
                _swordRotation.transform.localScale = new Vector3(Mathf.Sign(movementInput.x), 1, 1);
            }
        }
    }

    private void UpdatePlayerDirection(float moveX)
    {
        if (moveX < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (moveX > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}