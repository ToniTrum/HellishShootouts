using UnityEngine;

public class SpawnAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        
        _animator.SetBool("IsSpawning", true);
    }

    private void Update()
    {
        if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Destroy(gameObject);
        }
    }

    public float GetAnimationDuration()
    {
        if (_animator != null && _animator.runtimeAnimatorController != null)
        {
            AnimatorClipInfo[] clipInfo = _animator.GetCurrentAnimatorClipInfo(0);
            float time = clipInfo[0].clip.length;
            if (time > 0)
            {
                return time;
            }
        }
        return 0f;
    }
}
