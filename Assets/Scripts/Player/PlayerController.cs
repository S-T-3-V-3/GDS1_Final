using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public InputSystem playerInput;
    [HideInInspector] public PlayerStateManager stateManager;

    void Awake()
    {
        playerInput = new InputSystem();
        stateManager = gameObject.AddComponent<PlayerStateManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stateManager.AddState<MovementState>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

}
