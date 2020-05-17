using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    EnemyState currentState;

    public void AddState<T>() where T : EnemyState
    {
        if (currentState != null)
            this.RemoveState();

        currentState = this.gameObject.AddComponent<T>();
        currentState.BeginState();
    }

    public void RemoveState()
    {
        if (currentState != null)
        {
            currentState.EndState();
            Component.Destroy(currentState);
        }
    }
}

public abstract class EnemyState : MonoBehaviour
    {
        public abstract void BeginState();
        public virtual void EndState() { }
    }