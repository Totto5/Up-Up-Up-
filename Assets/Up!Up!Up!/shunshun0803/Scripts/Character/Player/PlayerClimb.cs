using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerClimb : MonoBehaviour
{
    private PlayerControls inputActions;
    private CharacterController controller;
    private Animator animator;
    private bool isClimbPressed;

    // 落下制御用
    private bool justClimbed = false;

    private void Awake()
    {
        inputActions = new PlayerControls();
        inputActions.PlayerAction.Climb.performed += ctx => isClimbPressed = true;
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
        if (isClimbPressed)
        {
            isClimbPressed = false;
            if (CheckForClimbableWall(out RaycastHit hit))
            {
                StartCoroutine(ClimbRoutine(hit));
            }
        }

        // 落下補正：Climb直後だけ小さな下方向Moveで接地させる
        if (justClimbed)
        {
            controller.Move(Vector3.down * 0.05f); // 軽く地面へ押し付ける
            justClimbed = false;
        }
    }

    bool CheckForClimbableWall(out RaycastHit hit)
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        return Physics.Raycast(origin, transform.forward, out hit, 1f);
    }

    IEnumerator ClimbRoutine(RaycastHit hit)
    {
        controller.enabled = false;
        animator.SetTrigger("Climb");

        // 高さを控えめに（1.5f→1.0f）して浮きを防ぐ
        Vector3 climbTarget = hit.point + Vector3.up * 1.0f;
        Vector3 startPos = transform.position;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, climbTarget, t);
            yield return null;
        }

        controller.enabled = true;

        // 直後に軽く下方向へ押して isGrounded を復活させる
        justClimbed = true;
    }
}

