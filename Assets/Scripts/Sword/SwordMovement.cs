using System.Collections;
using UnityEngine;


public class SwordMovement : MonoBehaviour
{
    private string currentAnimation = "";
    private Animator animator;
    private Transform nearestEnemy;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateNearestEnemy();
        RotateToEnemy();
    }

    private void UpdateNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            float shortestDistance = float.MaxValue;
            Transform closestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestEnemy = enemy.transform;
                    }
                }
            }

            nearestEnemy = closestEnemy;
        }
        else
        {
            nearestEnemy = null;
        }
    }

    private void RotateToEnemy()
    {
        // Debug.Log(nearestEnemy);
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            // Debug.Log(angle);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }
    
    public void ChangeAnimation(string animation, float time = 0)
    {
        if (time > 0) StartCoroutine(Wait());
        else Validate();

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time);
            Validate();
        } 

        void Validate()
        {
            if (currentAnimation != animation)
            {
                currentAnimation = animation;
                animator.Play(animation);
            }
        }
    }
}