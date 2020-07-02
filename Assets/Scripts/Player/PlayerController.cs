using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*public InputAction test;
    public UnityEvent OnMovement;
    public UnityEvent OnMouseAim;
    public UnityEvent OnGamepadAim;
    public UnityEvent OnShoot;
    public UnityEvent OnSprint;*/

    [HideInInspector] public PlayerStateManager stateManager;

    void Awake()
    {
        stateManager = gameObject.AddComponent<PlayerStateManager>();
    }

    void Start()
    {
        stateManager.AddState<MovementState>();
    }

    void OnPause(InputValue value) {
        GameManager.Instance.sessionData.TogglePause();
    }
}