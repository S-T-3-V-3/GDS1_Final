using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : PlayerState
{
    BasicPlayer player;
    PlayerController playerController;
    PlayerSettings playerSettings;

    Rigidbody playerRb;
    Transform cameraTransform;
    Vector3 currentMovementInput;
    Animator playerAnimator;

    Vector3 lookAtPos;
    Vector3 prevLookAtPos;
    bool isShooting = false;

    public override void BeginState()
    {
        player = this.GetComponent<BasicPlayer>();
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        cameraTransform = GameManager.Instance.mainCamera.transform;
        playerController = GetComponent<PlayerController>();
        playerRb = this.GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        currentMovementInput = Vector3.zero;
    }

    public void FixedUpdate()
    {
        if (currentMovementInput.magnitude > 0) {
            Vector3 newPosition = currentMovementInput * player.playerStats.moveSpeed * Time.fixedDeltaTime;
            playerRb.MovePosition(this.gameObject.transform.position + newPosition);

            ////// Handle Audio //////
            AudioManager.audioInstance.PlayFootstep();

            if (!playerAnimator.GetBool("isWalkingForward"))
            {
                playerAnimator.SetBool("isWalkingForward", true);
            }
        }

        if (currentMovementInput.magnitude == 0 && playerAnimator.GetBool("isWalkingForward"))
        {
            playerAnimator.SetBool("isWalkingForward", false);
        }

        if (lookAtPos != prevLookAtPos) {
            this.transform.LookAt(lookAtPos);
            prevLookAtPos = lookAtPos;
        }

        /////// HANDLE WEAPONS ///////
        player.equippedWeapon.RenderAim();

        if (isShooting)
            player.equippedWeapon.Shoot();
        else
            player.equippedWeapon.DisableLaser();

        if (player.playerStats.currentHealth < player.playerStats.maxHealth) {
            player.playerStats.currentHealth += player.playerStats.healthRegenSpeed * Time.deltaTime;
            player.OnHealthChanged.Invoke();
        }
    }

    public void OnMovement(InputValue value) {
        float inputX = value.Get<Vector2>().x;
        float inputY = value.Get<Vector2>().y;
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z);

        currentMovementInput = Vector3.zero;
        currentMovementInput += cameraForward.normalized * inputY;
        currentMovementInput += cameraRight.normalized * inputX;
        Vector3.ClampMagnitude(currentMovementInput, 1);
    }

    public void OnMouseAim(InputValue value) {
        Vector3 mousePos = value.Get<Vector2>();
        mousePos.z = Vector3.Magnitude(GameManager.Instance.mainCamera.transform.position - this.gameObject.transform.position); 

        lookAtPos = GameManager.Instance.mainCamera.ScreenToWorldPoint(mousePos);
        lookAtPos.y = this.transform.position.y;      
    }

    public void OnGamepadAim(InputValue value) {
        Vector2 rightStickPos = value.Get<Vector2>();
        float inputX = rightStickPos.x;
        float inputY = rightStickPos.y;
        Vector3 cameraForward = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
        Vector3 cameraRight = new Vector3(cameraTransform.right.x, 0, cameraTransform.right.z);
        
        Vector3 newLookAtPos = Vector3.zero;
        newLookAtPos += cameraForward.normalized * inputY;
        newLookAtPos += cameraRight.normalized * inputX;
        lookAtPos = this.transform.position + newLookAtPos;
    }

    public void OnShoot(InputValue value) {
        isShooting = value.Get<float>() > 0.25f;
    }
}
