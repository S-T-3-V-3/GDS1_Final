using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Camera Settings")]
public class CameraSettings : ScriptableObject{    
    public Vector3 targetOffset;
    public float minOffsetDistance;
    public float followDistance;
    public float lagSpeed;
    public float moveSpeed;
}
