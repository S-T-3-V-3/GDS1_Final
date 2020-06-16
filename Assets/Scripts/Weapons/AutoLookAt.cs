using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class AutoLookAt : MonoBehaviour
{
    [HideInInspector] public GameObject targetedEnemy;
    public float maxAngle = 90;

    Vector3 targetDir;

    public bool EnemyIsInFieldOfView()
    {
        if (targetedEnemy == null)
            return false;

        targetDir = targetedEnemy.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if(angle < maxAngle) return true;

        return false;
    }

    public Vector3 LockOntoEnemy()
    {
        return targetedEnemy.transform.position;
    }
}
