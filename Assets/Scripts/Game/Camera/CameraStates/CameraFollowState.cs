﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowState : CameraState
{
    CameraController cameraController;
    Transform targetTransform;
    Vector3 moveToPosition;
    CameraSettings cameraSettings;
    Vector3 velocity;
    CameraRaycast cameraRaycast;
    float step = 1;

    public override void BeginState()
    {
        cameraSettings = GameManager.Instance.gameSettings.cameraSettings;
        cameraController = this.gameObject.GetComponent<CameraController>();
        targetTransform = cameraController.targetTransform;
        velocity = Vector3.zero;

        cameraRaycast = this.gameObject.GetComponent<CameraRaycast>();

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
        Vector3 offset = targetTransform.position + Vector3.Normalize(cameraSettings.targetOffset) * cameraSettings.followDistance;
        moveToPosition = offset + Vector3.Normalize(this.transform.position - offset) * cameraSettings.minOffsetDistance;
        moveToPosition.y = offset.y;

        if(cameraRaycast.isBlocked)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(
                this.gameObject.transform.position,
                cameraRaycast.hitInfo.point,
                step);
        }
        else if (force)
            this.gameObject.transform.position = moveToPosition;
        else
            this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, moveToPosition, ref velocity, cameraSettings.lagSpeed);
    }
}