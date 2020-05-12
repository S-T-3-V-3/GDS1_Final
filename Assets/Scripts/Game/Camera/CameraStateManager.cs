using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStateManager : MonoBehaviour
{
    CameraState currentState;

    public void AddState<T>() where T : CameraState
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

public abstract class CameraState : MonoBehaviour
{
    public abstract void BeginState();
    public virtual void EndState() { }
}