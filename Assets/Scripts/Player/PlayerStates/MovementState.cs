﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementState : PlayerState
{
    PlayerController playerController;
    InputSystem playerInput;
    PlayerSettings playerSettings;

    Rigidbody playerRB;

    Vector2 moveInput;
    float deltaX;

    public override void BeginState()
    {
        playerController = GetComponent<PlayerController>();
        playerSettings = GameManager.instance.gameSettings.playerSettings;
        playerInput = playerController.playerInput;

        playerRB = GetComponent<Rigidbody>();

        playerInput.Player.Movement.performed += move => moveInput = move.ReadValue<Vector2>();
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
        if(moveInput.x != 0)
        {
            playerRB.velocity += transform.right * moveInput.x * playerSettings.movementSpeed * Time.fixedDeltaTime;
        }

        if(moveInput.y != 0)
        {
            playerRB.velocity += transform.forward * moveInput.y * playerSettings.movementSpeed * Time.fixedDeltaTime;
        }
    }

    public void RotatePlayer()
    {
        transform.Rotate(0, deltaX * 0.5f, 0);
    }
}
