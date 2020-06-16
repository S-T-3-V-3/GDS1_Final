using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class AutoLookAt : MonoBehaviour
{
    [HideInInspector] public GameObject targetedEnemy;
    public float maxAngle = 90;

    public bool EnemyIsInFieldOfView()
    {
        if (targetedEnemy == null)
            return false;
        //figure out if in field of view

        Vector3 targetDir = targetedEnemy.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if(angle < maxAngle)
        {
            return true;
        }

        return false;
    }

    public Vector3 LockOntoEnemy()
    {
        return targetedEnemy.transform.position;
    }
}
