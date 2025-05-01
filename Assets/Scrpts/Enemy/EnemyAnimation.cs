using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private EnemyMovement _enemyMovement;

    private readonly int _isWalkingHash = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        bool isWalking = _enemyMovement.GetVelocity().magnitude > 0.1f;
        _animator.SetBool(_isWalkingHash, isWalking);
    }
}
