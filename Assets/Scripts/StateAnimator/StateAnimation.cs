using UnityEngine;

public class StateAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    public bool IsFlipped { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator.SetBool("IsAnimate", true);
        IsFlipped = false;
    }

    private void Update()
    {
        _spriteRenderer.flipX = IsFlipped;  

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
