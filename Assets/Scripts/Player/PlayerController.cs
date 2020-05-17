using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerStateManager stateManager;

    void Awake()
    {
        stateManager = gameObject.AddComponent<PlayerStateManager>();
    }

    void Start()
    {
        stateManager.AddState<MovementState>();
    }
}