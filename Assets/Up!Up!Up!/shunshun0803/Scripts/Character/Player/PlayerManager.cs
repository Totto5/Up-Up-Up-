using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }
    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();
    }
    public void OnSpwawnPlayer()
    {
        playerLocomotionManager.enabled = true;
        PlayerCamera.instance.playerManager = this;
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
        PlayerCamera.instance.HandleAllCameraActions();
    }
}
