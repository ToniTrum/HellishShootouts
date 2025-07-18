using UnityEngine;

public class OnFinish : StateMachineBehaviour
{
    [SerializeField] private string animation;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        animator.GetComponent<SwordMovement>().ChangeAnimation(animation, stateInfo.length);
    }
    
}