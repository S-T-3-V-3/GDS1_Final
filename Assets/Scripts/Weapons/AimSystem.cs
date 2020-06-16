using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimSystem : MonoBehaviour
{
    LineRenderer aimLine;
    RectTransform aimPointUI;
    RaycastHit hit;

    Vector3 startPosition;
    Vector3 prevEndPos;
    float vertOffset = 0.1f;
    float forwardOffset = -0.2f;

    void Awake()
    {
        aimLine = GetComponent<LineRenderer>();
    } 

    public void RenderAimLine(Transform firePoint, Vector3 endPos, bool isInWorld)
    {

        //startPosition = firePoint.position + (firePoint.up * vertOffset) + (firePoint.forward * forwardOffset);
        startPosition = firePoint.position + (firePoint.forward * forwardOffset);
        aimLine.SetPosition(0, startPosition);

        //Debug.Log(aimLine.enabled);

        if (isInWorld)
        {
            aimLine.enabled = true;
            aimLine.SetPosition(1, endPos);
            return;
        }


        aimLine.enabled = false;
    }
}