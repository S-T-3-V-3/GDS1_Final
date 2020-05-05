using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 1.5f)]
    private float fireRate = 1;

    [SerializeField]
    [Range(1, 10)]
    private int damage = 1;

    [SerializeField]
    private Transform firePoint;

    // Update is called once per frame
    void Update()
    {
 
            if (Input.GetMouseButtonDown(0))
            {
                Debug.DrawRay(firePoint.position, firePoint.forward * 1000, Color.red, 2f);
            //Debug.Log("mouse");
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            RaycastHit hitInfo;

            if(Physics.Raycast(ray, out hitInfo, 100))
            {
                Debug.Log(hitInfo);
                Destroy(hitInfo.collider.gameObject);
            }
            }
 
    }

 
}
