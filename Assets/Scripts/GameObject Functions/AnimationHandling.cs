using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandling : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();
    }

    public void setAnimationParameters(bool right, bool left, bool up, bool down, bool idle)
    {
        animator.SetBool("isMovingRight", right);
        animator.SetBool("isMovingLeft", left);
        animator.SetBool("isMovingUp", up);
        animator.SetBool("isMovingDown", down);
        animator.SetBool("isIdling", idle);
    }
}
