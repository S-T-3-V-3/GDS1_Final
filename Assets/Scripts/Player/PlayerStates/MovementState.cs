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

    CharacterController characterController;
    Transform cameraTransform;
    Vector3 currentMovementInput;
    Animator animationController;

    Vector3 lookAtPos;
    Vector3 prevLookAtPos;
    bool isShooting = false;

    public override void BeginState()
    {
        player = this.GetComponent<BasicPlayer>();
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        cameraTransform = GameManager.Instance.mainCamera.transform;
        playerController = this.GetComponent<PlayerController>();
        characterController = this.GetComponent<CharacterController>();
        animationController = player.animationController;
        currentMovementInput = Vector3.zero;
    }

    public void Update()
    {
        Vector3 newPosition = currentMovementInput * player.statHandler.MoveSpeed * Time.fixedDeltaTime;
        characterController.Move(newPosition);
        characterController.Move(player.velocity * Time.deltaTime);

        ////// Handle Audio //////
        AudioManager.Instance.PlayFootstep();
        // This should be doable via animation events?
        // Trigger an event every frame the foot gets placed down, listen for those events, play sounds

        animationController.SetFloat("Forward", Vector3.Dot(transform.forward.normalized, currentMovementInput));
        animationController.SetFloat("Right", Vector3.Dot(transform.right.normalized, currentMovementInput));
        animationController.SetBool("IsInAir", !player.isGrounded);

        if (lookAtPos != prevLookAtPos) {
            this.transform.LookAt(lookAtPos);
            prevLookAtPos = lookAtPos;
        }

        /////// HANDLE WEAPONS ///////
        //player.equippedWeapon.RenderAim();

        if (isShooting)
            player.equippedWeapon.Shoot();
        else
        {
            if (player.equippedWeapon.weaponType == WeaponType.LASER)
                player.equippedWeapon.DisableLaser();
        }


        // There's got to be a better way to do this
        // Start by putting these kinds of things on the relevant objects
        // IE; a laser beam shouldn't be controlled by the players movement state
        /*
        else
            player.equippedWeapon.DisableLaser();
        */

        if (player.statHandler.CurrentHealth < player.statHandler.MaxHealth) {
            player.statHandler.CurrentHealth += player.statHandler.HealthRegen * Time.deltaTime;
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
