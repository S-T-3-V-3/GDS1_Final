using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
        public Vector3 moveDirection;

        public const float maxDashTime = 1.0f;
        public float dashDistance = 10;
        public float dashStoppingSpeed = 0.1f;
        float currentDashTime = maxDashTime;
        float dashSpeed = 6;
        CharacterController controller;

        public Transform firePoint;
        bool canDash = true;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkDash();
                currentDashTime = 0;
            }
            if (canDash)
            {
                if (currentDashTime < maxDashTime)
                {
                    moveDirection = transform.forward * dashDistance;
                    currentDashTime += dashStoppingSpeed;
                }
                else
                {
                    moveDirection = Vector3.zero;
                }
                controller.Move(moveDirection * Time.deltaTime * dashSpeed);
            }
           
        }

        void checkDash()
        {
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, dashDistance))
        {
            canDash = false;
            Debug.Log(hitInfo.distance);
        }
        else
            canDash = true;
        }
  }