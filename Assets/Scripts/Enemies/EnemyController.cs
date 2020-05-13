using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyStateManager stateManager;

    void Start()
    {
        stateManager = this.gameObject.AddComponent<EnemyStateManager>();
        SetState<EnemyWanderState>();
    }

    public void SetState<T>() where T : EnemyState
    {
        stateManager.AddState<T>();
    }
}
