using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLocomotionManager : MonoBehaviour
{
    CharacterController controller;
    PlayerInputManager input;
    PlayerAnimatorManager animator;

    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    private Vector3 moveDirection;
    private Vector3 velocity;
    private bool isGrounded;

    [SerializeField] private float climbHeight = 1.0f;
    [SerializeField] private float climbRayOriginY = 1.5f;
    [SerializeField] private float climbRayDistance = 1.0f;
    private bool justClimbed = false;

    [SerializeField] private float slowTimeScale = 0.2f;
    [SerializeField] private float normalTimeScale = 1.0f;
    [SerializeField] private float slowTimeTransitionSpeed = 5f;

    private bool isClimbing = false;



    private void Start()
    {
        controller = GetComponent<CharacterController>();
        input = PlayerInputManager.instance;
        animator = GetComponent<PlayerAnimatorManager>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleClimb();
        ApplyGravity();
        ApplyFinalMovement();
        HandleTimeSlow();


        if (justClimbed)
        {
            controller.Move(Vector3.down * 0.05f);
            justClimbed = false;
        }
    }

    void HandleMovement()
    {
        isGrounded = controller.isGrounded;

        if (!controller.enabled) return; // 無効なら移動をスキップ

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = forward * input.verticalInput + right * input.horizontalInput;
        moveDirection.Normalize();

        float speed = input.moveAmount > 0.5f ? runSpeed : walkSpeed;

        controller.Move(moveDirection * speed * Time.deltaTime);
        animator.UpdateAnimator(input.moveAmount, isGrounded);
        }

    void HandleRotation()
    {
        if (moveDirection == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (input.jumpInput && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.TriggerJump();
            input.ResetJumpInput(); // 入力直後にリセット
        }
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 安定化
        }
        velocity.y += gravity * Time.deltaTime;
    }

    void ApplyFinalMovement()
    {
        if (!controller.enabled) return;
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);
    }
    void HandleClimb()
    {
        if (input.climbInput)
        {
            input.ResetClimbInput();

            if (CheckForClimbableWall(out RaycastHit hit))
            {
                StartCoroutine(ClimbRoutine(hit));
            }
        }
    }

    bool CheckForClimbableWall(out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * climbRayOriginY;
        return Physics.Raycast(origin, transform.forward, out hit, climbRayDistance);
    }

    IEnumerator ClimbRoutine(RaycastHit hit)
    {
        isClimbing = true;
        controller.enabled = false;
        animator.TriggerClimb(); // AnimatorManager に作る

        Vector3 target = hit.point + Vector3.up * climbHeight;
        Vector3 start = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        controller.enabled = true;
        justClimbed = true;
        isClimbing = false;
    }
    void HandleTimeSlow()
    {
        // 空中でスロウ入力中のみ slowTimeScale に向かって補間
        if (!isGrounded && input.slowInput && !isClimbing)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowTimeScale, Time.unscaledDeltaTime * slowTimeTransitionSpeed);
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // 物理も合わせる
        }
        else if (isGrounded && input.slowInput && !isClimbing)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, slowTimeScale, Time.unscaledDeltaTime * slowTimeTransitionSpeed);
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // 物理も合わせる
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, normalTimeScale, Time.unscaledDeltaTime * slowTimeTransitionSpeed);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

}
