using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowState : CameraState
{
    CameraController cameraController;
    Transform targetTransform;
    Vector3 moveToPosition;
    CameraSettings cameraSettings;
    Vector3 velocity;

    public override void BeginState()
    {
        cameraSettings = GameManager.Instance.gameSettings.cameraSettings;
        cameraController = this.gameObject.GetComponent<CameraController>();
        targetTransform = cameraController.targetTransform;
        velocity = Vector3.zero;

        DoMovement(true);
        this.gameObject.transform.LookAt(targetTransform);
    }

    void Update() {
        if (targetTransform == null)
        {
            cameraController.SetState<CameraSearchState>();
            return;
        }

        DoMovement();
        this.gameObject.transform.LookAt(targetTransform);
    }

    void DoMovement(bool force = false) {
        Vector3 offset;
        float pauseZoom = 0.6f;

        if (GameManager.Instance.sessionData.isPaused) {
            offset = targetTransform.position + Vector3.Normalize(cameraSettings.targetOffset) * (cameraSettings.followDistance * pauseZoom);
            moveToPosition = offset + Vector3.Normalize(this.transform.position - offset) * (cameraSettings.minOffsetDistance * pauseZoom);
            moveToPosition.y = (offset.y * pauseZoom);
        }
        else {
            offset = targetTransform.position + Vector3.Normalize(cameraSettings.targetOffset) * cameraSettings.followDistance;
            moveToPosition = offset + Vector3.Normalize(this.transform.position - offset) * cameraSettings.minOffsetDistance;
            moveToPosition.y = offset.y;
        }

        if (force)
            this.gameObject.transform.position = moveToPosition;
        else
            this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, moveToPosition, ref velocity, cameraSettings.lagSpeed);
    }
}