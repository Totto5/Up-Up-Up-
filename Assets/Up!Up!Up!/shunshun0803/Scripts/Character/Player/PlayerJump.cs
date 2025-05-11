using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    private CharacterController controller;
    private PlayerControls inputActions;
    private bool isJumpPressed;
    private Vector3 velocity;
    private bool isGrounded;

    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    private Animator animator;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.PlayerAction.Jump.performed += ctx => isJumpPressed = true;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        animator.SetBool("IsGround", isGrounded);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 接地を安定させる
        }

        // ジャンプ処理
        if (isJumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumpPressed = false;
            animator.SetTrigger("Jump"); // アニメーション制御
        }

        // 重力処理
        velocity.y += gravity * Time.deltaTime;

        // 実際のジャンプ移動
        controller.Move(Vector3.up * velocity.y * Time.deltaTime);
    }
}
