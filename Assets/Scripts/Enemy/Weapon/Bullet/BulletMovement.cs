using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _damage = 1f;

    public Vector2 direction;

    private void Update()
    {
        transform.position += (Vector3)(direction.normalized * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
