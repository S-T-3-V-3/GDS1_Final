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

        public AbilityType abilityType;
        public AbilityStats abilityStats;
        public StatModifiers statModifier;
        CharacterController controller;
        BasicPlayer player;
        public Transform firePoint;
        float timer;

        bool canDash = true;
     
        void Start()
        {
            controller = GameManager.Instance.playerController.GetComponent<CharacterController>();
            player = GameManager.Instance.playerController.GetComponent<BasicPlayer>();

            abilityType = AbilityType.RAPIDFIRE;
            abilityStats.multiplier = 5;
            abilityStats.time = 5;
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch(abilityType)
                {
                    case AbilityType.DASH:
                        //dash();
                        break;
                    case AbilityType.RAPIDHEAL:
                        StartCoroutine(rapidHeal());                  
                        break;
                    case AbilityType.RAPIDFIRE:
                        //StartCoroutine(rapidFire());
                        break;

                }

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

        IEnumerator rapidHeal()
        {
            int swapStat;
            swapStat = player.statHandler.HealthRegenLevel;

            player.statHandler.HealthRegenLevel *= abilityStats.multiplier;

            yield return new WaitForSeconds(abilityStats.time);

            player.statHandler.HealthRegenLevel = swapStat;
        }

}