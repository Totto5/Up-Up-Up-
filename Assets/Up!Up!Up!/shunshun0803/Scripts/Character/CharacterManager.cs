using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;

    [Header("Flags")]
    public bool canMove = true;

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    protected virtual void Update(){

    }
    protected virtual void LateUpdate(){

    }
}
