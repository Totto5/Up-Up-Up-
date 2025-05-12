using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimator(float moveAmount, bool isGrounded)
    {
        animator.SetFloat("Speed", moveAmount, 0.1f, Time.deltaTime);
        animator.SetBool("IsGround", isGrounded);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
    public void TriggerClimb()
    {
        animator.SetTrigger("Climb");
    }
}
