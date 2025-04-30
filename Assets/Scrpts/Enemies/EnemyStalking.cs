using UnityEngine;

public class EnemyStalking : MonoBehaviour
{
    [SerializeField] private float _playerDistance;

    private Transform _player;

    public bool StalkingOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer {  get; private set; }

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
    }
    private void Update()
    {
        Vector2 enemyToPlayer = _player.position - transform.position;
        DirectionToPlayer = enemyToPlayer.normalized;

        if (enemyToPlayer.magnitude >= _playerDistance)
        {
            StalkingOfPlayer = true;
        }
        else
        {
            StalkingOfPlayer = false;
        }
    }
}