using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementState : PlayerState
{
    PlayerController playerController;
    InputSystem playerInput;
    PlayerSettings playerSettings;

    Rigidbody playerRb;
    Vector3 newPosition;
    Vector2 moveInput;
    Transform cameraTransform;
    float deltaX;

    public override void BeginState()
    {
        playerController = GetComponent<PlayerController>();
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        playerInput = playerController.playerInput;

        playerInput.Player.Movement.performed += move => moveInput = move.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += move => moveInput = Vector2.zero;

        playerInput.Player.RotationY.performed += delta => deltaX = delta.ReadValue<float>();
        playerInput.Player.RotationY.canceled += delta => deltaX = 0f;

        cameraTransform = GameManager.Instance.mainCamera.transform;
        playerRb = this.GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        //////////// Debug Input ///////////////////
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        if (Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }
        ////////////////////////////////////////////
        
        newPosition = Vector3.zero;
        newPosition += cameraTransform.right * moveInput.x;
        newPosition += cameraTransform.forward * moveInput.y;
        newPosition.y = 0;
        newPosition = Vector3.Normalize(newPosition) * playerSettings.baseStats.moveSpeed * Time.fixedDeltaTime;

        playerRb.MovePosition(this.gameObject.transform.position + newPosition);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Vector3.Magnitude(GameManager.Instance.mainCamera.transform.position - this.gameObject.transform.position);
        Vector3 lookAtPos = GameManager.Instance.mainCamera.ScreenToWorldPoint(mousePos);
        
        lookAtPos.y = this.transform.position.y;
        this.transform.LookAt(lookAtPos);
    }
}
