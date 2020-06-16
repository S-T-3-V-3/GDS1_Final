using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCameraTrigger : MonoBehaviour
{
    CameraController cameraController;
    Camera playerCamera;
    Transform startingTransform;

    void Start()
    {
        startingTransform = this.transform;
        
        playerCamera = GameManager.Instance.mainCamera;
        cameraController = playerCamera.GetComponent<CameraController>();
        playerCamera.transform.position = startingTransform.position;
    }

    private void OnTriggerExit(Collider other) {
        if(other.GetComponent<BasicPlayer>() != null)
        {
            cameraController.SetState<CameraFollowState>();
        }
    }

}
