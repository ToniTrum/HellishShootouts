using System.Collections;
using UnityEngine;


public class SwordMovement : MonoBehaviour
{
    private string currentAnimation = "";
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
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