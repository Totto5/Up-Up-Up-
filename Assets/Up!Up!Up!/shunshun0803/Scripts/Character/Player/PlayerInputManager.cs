using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    public PlayerInputManager player;
    PlayerControls playerControls;

    [Header("PLAYER MOVEMENT INPUT")]
    [SerializeField] Vector2 movementInput;
    public float horizontalInput;
    public float verticalInput;
    public float moveAmount;
    [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] Vector2 cameraMovementInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        HandleAllInput();
    }
    private void HandleAllInput()
    {
        // Handle all input here
        // For example, you can call methods to handle movement, jumping, etc.
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }
    private void HandlePlayerMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if(moveAmount <= 0.5 && moveAmount > 0) {
            moveAmount = 0.5f;
        }else if(moveAmount > 0.5 && moveAmount < 1) {
            moveAmount = 1f;
        }
    }
    private void HandleCameraMovementInput(){
        cameraVerticalInput = cameraMovementInput.y;
        cameraHorizontalInput = cameraMovementInput.x;
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
            playerControls.CameraMovement.Movement.performed += ctx => cameraMovementInput = ctx.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }
}
