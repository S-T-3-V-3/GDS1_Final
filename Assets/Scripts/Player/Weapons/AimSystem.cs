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
    float vertOffset = 0.1f;
    float forwardOffset = -0.2f;

    void Awake()
    {
        aimLine = GetComponent<LineRenderer>();
    }

    public void RenderAimLine(Transform firePoint)
    {
        /*if(aimPointUI == null)
        {
            aimPointUI = Instantiate(GameManager.Instance.gameSettings.aimPointUI, GameManager.Instance.hud.transform).GetComponent<RectTransform>();
        }*/

        startPosition = firePoint.position + (firePoint.up * vertOffset) + (firePoint.forward * forwardOffset);
        aimLine.SetPosition(0, startPosition);

        if (Physics.Raycast(startPosition, firePoint.forward, out hit, 100))
        {
            aimLine.SetPosition(1, startPosition + firePoint.forward * hit.distance);  
        } else
        {
            aimLine.SetPosition(1, startPosition + firePoint.forward * 100);
        }
    }

    void RenderAimPoint()
    {
        /*if (Camera.main != null)
        {
            aimPointUI.gameObject.SetActive(true);
            Debug.Log(Camera.main.WorldToScreenPoint(startPosition + firePoint.forward * hit.distance));
            aimPointUI.position = Camera.main.WorldToScreenPoint(startPosition + firePoint.forward * hit.distance);
        }*/
    }
}