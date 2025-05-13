using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    private PlayerControls inputActions;

    [Header("Movement Input")]
    public Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;

    [Header("Camera Input")]
    public Vector2 cameraInput;
    public float cameraX;
    public float cameraY;

    [Header("Actions")]
    public bool jumpInput;
    public bool climbInput;
    public bool slowInput;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        inputActions = new PlayerControls();

        inputActions.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputActions.PlayerMovement.Movement.canceled += ctx => movementInput = Vector2.zero;

        inputActions.CameraMovement.Movement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        inputActions.CameraMovement.Movement.canceled += ctx => cameraInput = Vector2.zero;

        inputActions.PlayerAction.Jump.performed += ctx => jumpInput = true;
        inputActions.PlayerAction.Climb.performed += ctx => climbInput = true;

        inputActions.PlayerAction.Slow.performed += ctx => slowInput = true;
        inputActions.PlayerAction.Slow.canceled += ctx => slowInput = false;
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();
    public void ResetClimbInput() => climbInput = false;

    private void Update()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        cameraX = cameraInput.x;
        cameraY = cameraInput.y;
    }

    public void ResetJumpInput() => jumpInput = false;
}
