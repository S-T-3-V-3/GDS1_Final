using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStartState : CameraState
{
    CameraController cameraController;
    Transform targetTransform;
    Vector3 moveToPosition;
    CameraSettings cameraSettings;

    public override void BeginState()
    {
        cameraSettings = GameManager.Instance.gameSettings.cameraSettings;
        cameraController = this.gameObject.GetComponent<CameraController>();
        targetTransform = cameraController.targetTransform;

        this.gameObject.transform.LookAt(targetTransform);
        cameraController.previousPosition = targetTransform.position;
    }

    void Update() {
        if (targetTransform == null)
        {
            cameraController.SetState<CameraSearchState>();
            return;
        }

        this.gameObject.transform.LookAt(targetTransform);
    }
}
