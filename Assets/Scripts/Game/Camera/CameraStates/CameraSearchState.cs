using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSearchState : CameraState
{
    CameraController cameraController;
    GameManager gameManager;

    public override void BeginState()
    {
        cameraController = this.GetComponent<CameraController>();
        gameManager = GameManager.Instance;
    }

    void Update() {
        if (gameManager.playerController == null) return;

        cameraController.targetTransform = gameManager.playerController.gameObject.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.transform;
        cameraController.SetState<CameraFollowState>();
    }
}