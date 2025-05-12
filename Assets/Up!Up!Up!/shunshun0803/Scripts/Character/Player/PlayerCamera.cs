using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager playerManager;
    public Camera cameraObject;
    [SerializeField] private Transform cameraPivotTransform;


    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;
    [SerializeField]private float leftAndRightRotationSpeed = 220;
    [SerializeField]private float upAndDownRotationSpeed = 220;
    [SerializeField]private float minimumPivot = -30;
    [SerializeField]private float maximumPivot = 60;
    [SerializeField] private float cameraCollisionRadius = 0.2f;
    [SerializeField] private LayerMask cameraCollisionLayer;


    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] private float leftAndRightLookAngle;
    [SerializeField] private float upAndDownLookAngle;

    private float cameraZPosition;
    private float targetCameraZPosition;

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
    private void Start()
    {
        cameraZPosition = cameraObject.transform.localPosition.z;
    }
    public void HandleAllCameraActions(){
        if(playerManager != null){
            HandleFollowTarget();
            HandleRotation();
            HandleCollision();
        }
    }
    private void HandleFollowTarget(){
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetPosition;
    }
    private void HandleRotation(){
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraX * leftAndRightRotationSpeed) * Time.deltaTime;

        upAndDownLookAngle -= (PlayerInputManager.instance.cameraY * upAndDownRotationSpeed) * Time.deltaTime;

        upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollision()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), cameraCollisionLayer))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if(Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraZPosition;

        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

}
