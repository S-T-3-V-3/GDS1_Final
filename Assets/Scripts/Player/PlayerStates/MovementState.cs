using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : PlayerState
{
    BasicPlayer player;
    PlayerController playerController;
    PlayerSettings playerSettings;
    AutoLookAt playerAim;
    MouseIndicator mouseIndicator;

    CharacterController characterController;
    Transform cameraTransform;
    Vector3 currentMovementInput;
    Animator animationController;
    AimSystem aimSystem;

    Vector3 lookAtPos;
    Vector3 prevLookAtPos;
    bool isShooting = false;
    bool isAimingToPosition = false;

    public override void BeginState()
    {
        player = this.GetComponent<BasicPlayer>();
        playerAim = this.gameObject.AddComponent<AutoLookAt>();
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        cameraTransform = GameManager.Instance.mainCamera.transform;
        playerController = this.GetComponent<PlayerController>();
        characterController = this.GetComponent<CharacterController>();
        animationController = player.animationController;
        currentMovementInput = Vector3.zero;

        aimSystem = Instantiate(playerSettings.aimSystem, transform).GetComponent<AimSystem>();
    }

    public void FixedUpdate()
    {
        if (player.isSprinting) {
            player.statHandler.Energy -= 10f * Time.deltaTime;

            if (player.statHandler.Energy <= 0) {
                player.isSprinting = false;
                player.statHandler.MoveSpeed = player.statHandler.WalkSpeed;
                animationController.SetBool("isSprinting", player.isSprinting);
            }
        }
        else {
            player.statHandler.Energy += player.statHandler.EnergyRegenSpeed * Time.deltaTime;
        }

        Vector3 newPosition = currentMovementInput * player.statHandler.MoveSpeed * Time.fixedDeltaTime;
        characterController.Move(newPosition);
        characterController.Move(player.velocity * Time.deltaTime);

        ////// Handle Audio //////
        //AudioManager.Instance.PlayFootstep();
        // This should be doable via animation events?
        // Trigger an event every frame the foot gets placed down, listen for those events, play sounds

        animationController.SetFloat("Forward", Vector3.Dot(transform.forward.normalized, currentMovementInput));
        animationController.SetFloat("Right", Vector3.Dot(transform.right.normalized, currentMovementInput));
        animationController.SetBool("IsInAir", !player.isGrounded);

        if (lookAtPos != prevLookAtPos) {
            player.transform.LookAt(new Vector3(lookAtPos.x, this.transform.position.y, lookAtPos.z));
            player.equippedWeapon.weaponModel.transform.LookAt(lookAtPos);
            prevLookAtPos = lookAtPos;
        }

        //lookAtPos = playerAim.OnMouseAim(InputValue value);

        /////// HANDLE WEAPONS ///////
        //player.equippedWeapon.RenderAim();

        if (isShooting)
            player.equippedWeapon.Shoot();

        /*if (isLockedOn)
        {
            if (!playerAim.EnemyIsInFieldOfView()) isLockedOn = false;
            if (playerAim.targetedEnemy == null) return;
            lookAtPos = playerAim.LockOntoEnemy();
        }*/

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

        RunAimSystem();
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

    void RunAimSystem()
    {
        if (player.equippedWeapon == null) return;

        aimSystem.RenderAimLine(player.equippedWeapon.firePoint, lookAtPos, isAimingToPosition);
    }

    ///TODO: Jaiden to refactor this later
    public void OnMouseAim(InputValue value) {
        Vector3 mousePos = value.Get<Vector2>();
        Camera camera = GameManager.Instance.mainCamera;
        Ray ray = camera.ScreenPointToRay(mousePos);

        if (mouseIndicator == null) {
            mouseIndicator = GameManager.Instance.hud.mouseIndicator.GetComponent<MouseIndicator>();
        } else {
            mouseIndicator.SetIndicatorPosition(mousePos);
        }


        RaycastHit[] hits = Physics.RaycastAll(ray,200f).Where(x => x.collider.name.Contains("Wall") == false && x.collider.GetComponent<Tile>() == null).ToArray();
        //RaycastHit[] hits = Physics.RaycastAll(ray,200f).Where(x => x.collider.GetComponent<IDamageable>() != null).ToArray();

        if (hits.Length > 0) {

            RaycastHit[] filteredHits = hits.Where(x => x.collider.GetComponent<IDamageable>() != null).ToArray();
            //Collider[] hitColliders = Physics.OverlapSphere(hits.First().collider.transform.position, 2).Where(x => x.name.Contains("Wall") == false && x.GetComponent<Tile>() == null).ToArray();
            Collider[] hitColliders = Physics.OverlapSphere(hits.First().collider.transform.position, 0.7f).Where(x => x.gameObject.GetComponent<IDamageable>() != null).ToArray();
            isAimingToPosition = true;

            if(filteredHits.Length > 0)
            {
                if (filteredHits.First().transform != this.transform) {
                    lookAtPos = filteredHits.First().transform.position;
                    mouseIndicator.SetTransitionState(true);

                    if (isShooting) playerAim.targetedEnemy = filteredHits.First().collider.gameObject;
                }
                else
                {
                    lookAtPos = hits.First().point;
                    lookAtPos.y += 0.5f;
                }

                return;
            } else {
                if (hitColliders.Length > 0)
                {
                    //Debug.Log(hitColliders.First().name);
                    if (hitColliders.First().transform != this.transform)
                    {
                        lookAtPos = hitColliders.First().transform.position;
                        mouseIndicator.SetTransitionState(true);

                        if (isShooting) playerAim.targetedEnemy = hitColliders.First().gameObject;
                    }
                    else
                    {
                        lookAtPos = hits.First().point;
                        lookAtPos.y += 0.5f;
                    }

                    return;
                }
            }
            
            lookAtPos = hits.First().point;
            lookAtPos.y += 0.5f;
            mouseIndicator.SetTransitionState(false);

        } else
        {
            isAimingToPosition = false;
            mouseIndicator.SetTransitionState(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(lookAtPos, 1f);
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

    public void OnSprint(InputValue value) {
        player.isSprinting = value.Get<float>() == 0 ? false : true;
        animationController.SetBool("isSprinting", player.isSprinting);
        player.statHandler.MoveSpeed = player.isSprinting ? player.statHandler.SprintSpeed : player.statHandler.WalkSpeed;
    }
}
