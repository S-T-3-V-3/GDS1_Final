using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 cameraDestination;
    CameraStateManager stateManager;

    void Start()
    {
        stateManager = this.gameObject.AddComponent<CameraStateManager>();
        SetState<CameraSearchState>();
    }

    public void SetState<T>() where T : CameraState
    {
        stateManager.AddState<T>();
    }
}