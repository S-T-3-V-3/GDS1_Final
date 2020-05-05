using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : PlayerState
{
    PlayerController playerController;
    InputSystem playerInput;
    PlayerSettings playerSettings;

    Vector3 movementVelocity;
    Vector2 moveInput;
    float deltaX;

    public override void BeginState()
    {
        playerController = GetComponent<PlayerController>();
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        playerInput = playerController.playerInput;

        playerInput.Player.Movement.performed += move => moveInput = move.ReadValue<Vector2>();
        //playerInput.Player.Movement.performed += CheckInput; Use this for debugging only
        playerInput.Player.Movement.canceled += move => moveInput = Vector2.zero;

        playerInput.Player.RotationY.performed += delta => deltaX = delta.ReadValue<float>();
        playerInput.Player.RotationY.canceled += delta => deltaX = 0f;
    }

    public void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    public void MovePlayer()
    {
        movementVelocity = Vector3.zero;
        //movementVelocity = new Vector3(moveInput.x, 0, moveInput.y) * playerSettings.baseStats.moveSpeed * Time.fixedDeltaTime;

        //Below is relative movement towards forward direction
        if(moveInput.x != 0)
        {
            movementVelocity += transform.right * moveInput.x * playerSettings.baseStats.moveSpeed * playerSettings.movementDelta;
        }

        if(moveInput.y != 0)
        {
            movementVelocity += transform.forward * moveInput.y * playerSettings.baseStats.moveSpeed * playerSettings.movementDelta;
        }

        transform.position += movementVelocity;
    }

    //Use this for debugging only
    /*public void CheckInput(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }*/

    public void RotatePlayer()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Vector3.Magnitude(GameManager.Instance.mainCamera.transform.position - this.gameObject.transform.position);
        Vector3 lookAtPos = GameManager.Instance.mainCamera.ScreenToWorldPoint(mousePos);
        
        lookAtPos.y = transform.position.y;
        transform.LookAt(lookAtPos);
        // Maybe we want to put a bit of rotation speed/smooth damp towards look at pos?
        //transform.Rotate(0, deltaX * playerSettings.rotationSpeed, 0);
    }
}
