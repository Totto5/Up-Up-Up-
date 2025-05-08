using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // 移動速度
    [SerializeField] private float jumpForce = 5f; // ジャンプ力
    [SerializeField] private float mouseSensitivity = 2f; // マウス感度
    [SerializeField] private Transform playerCamera; // プレイヤーのカメラ参照
    private Rigidbody rb; // Rigidbody コンポーネントを参照

    private float xRotation = 0f; // x 軸の回転角度（上下の視点）

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbody を取得
        Cursor.lockState = CursorLockMode.Locked; // マウスカーソルをロック
        Cursor.visible = false; // マウスカーソルを非表示にする
    }

    private void Update()
    {
        // WASDキーまたは矢印キーでの移動
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // カメラの向きに基づいて移動ベクトルを計算
        Vector3 forward = playerCamera.forward; // カメラの前方方向
        Vector3 right = playerCamera.right; // カメラの右方向

        // カメラの水平方向（y軸の成分を無視して移動方向を決定）
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // カメラの向きに基づく移動
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // スペースキーでジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f) // 地面にいる場合のみジャンプ
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 上方向に力を加える
        }

        // マウスによる視点回転
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // プレイヤーの左右回転
        transform.Rotate(Vector3.up * mouseX);

        // カメラの上下回転（x 軸の回転）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 視点が上下に90度以上回らないように制限
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
