using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3;
    [SerializeField] private float jumpPower = 6;
    [SerializeField] private Transform cameraTransform; // 🎥 カメラ参照（CinemachineのFollowターゲット）

    private CharacterController _characterController;
    private Vector3 _moveVelocity;
    private InputAction _move;
    private InputAction _jump;
    private PlayerInput _playerInput;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();

        var map = _playerInput.currentActionMap;
        _move = map.FindAction("Move");
        _jump = map.FindAction("Jump");

        _move.Enable();
        _jump.Enable();
    }

    void Update()
    {
        Vector2 input = _move.ReadValue<Vector2>();

        // 🎯 カメラの向きに合わせた移動ベクトルを作成
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * input.y + camRight * input.x;
        move *= moveSpeed;

        // y方向を維持（ジャンプや重力の影響）
        move.y = _moveVelocity.y;

        // 🎯 キャラの向きを移動方向に合わせる
        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 lookDirection = new Vector3(move.x, 0f, move.z);
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10f * Time.deltaTime);
            }
        }

        // ジャンプ処理
        if (_characterController.isGrounded)
        {
            if (_jump.WasPressedThisFrame())
            {
                move.y = jumpPower;
                Debug.Log("Jump");
            }
            else
            {
                move.y = -1f; // 接地維持
            }
        }
        else
        {
            move.y += Physics.gravity.y * Time.deltaTime;
        }

        // 速度を保存しておく（重力用）
        _moveVelocity = move;

        // 実際に動かす
        _characterController.Move(move * Time.deltaTime);
    }
    
}
