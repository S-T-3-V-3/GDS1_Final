using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    public abstract void BeginState();
    public virtual void EndState() { }
}

public class PlayerStateManager : MonoBehaviour
{
    PlayerState currentState;

    public void AddState<T>() where T : PlayerState
    {
        if (currentState != null)
            this.RemoveState();

        currentState = this.gameObject.AddComponent<T>();
        currentState.BeginState();
    }

    public void RemoveState()
    {
        if(currentState != null)
        {
            currentState.EndState();
            Component.Destroy(currentState);
        }
    }
}
