using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    public Vector3 targetPos;
    CameraController cameraController;
    CameraSettings cameraSettings;
    Transform targetTransform;

    public RaycastHit hitInfo;
    public bool isBlocked = false;

    void Start()
    {
        targetTransform = GameManager.Instance.playerController.GetComponentInParent<Transform>();
        cameraSettings = GameManager.Instance.gameSettings.cameraSettings;
        cameraController = this.gameObject.GetComponent<CameraController>();
    }

    void LateUpdate()
    {
        if (targetPos == null) return;
        if (targetTransform == null) return;
        
        RaycastHit hit;
        int layerMask = 1 << 9;
        Vector3 dir = targetPos - targetTransform.position;

        if(Physics.Raycast(targetTransform.position, dir, out hit, cameraSettings.followDistance, layerMask)) 
        {
            hitInfo = hit;
            isBlocked = true;
            cameraController.SetState<CameraFollowState>();
            Debug.DrawRay(targetTransform.position, dir, Color.red);   
        }
        else
        {             
            isBlocked = false;
            Debug.DrawRay(targetTransform.position, dir, Color.white);
        }
    }
}
