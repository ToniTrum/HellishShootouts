using UnityEngine;

public class EnemyStalking : MonoBehaviour
{
    [SerializeField] public float attackDistance = 1f;

    public Transform player;

    public bool StalkingOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer {  get; private set; }

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerMovement>().transform;
    }
    private void Update()
    {
        Vector2 enemyToPlayer = player.position - transform.position;
        DirectionToPlayer = enemyToPlayer.normalized;

        if (enemyToPlayer.magnitude >= attackDistance)
        {
            StalkingOfPlayer = true;
        }
        else
        {
            StalkingOfPlayer = false;
        }
    }
}