using UnityEngine;


public class PlayerMovementAnimation : MonoBehaviour
{
    Animator animator;
    CharacterController controller;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        float speed = move.magnitude;

        // 入力ベクトルの大きさが0.1未満ならSpeed = 0にする処理を追加
        if (move.magnitude < 0.1f)
            speed = 0;
        animator.SetFloat("Speed", speed);

    }
}
