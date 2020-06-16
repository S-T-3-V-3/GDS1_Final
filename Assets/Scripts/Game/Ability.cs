﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
 
public class Ability : MonoBehaviour
{
        public Vector3 moveDirection = Vector3.zero;

        GameManager gameManager;
        GameSettings gameSettings;

        public AbilityType abilityType;
        public AbilityStats abilityStats;

        CharacterController characterController;
        BasicPlayer player;

        public Transform firePoint;
     
        void Start()
        {
            gameManager = GameManager.Instance;
            gameSettings = gameManager.gameSettings;

            characterController= GameManager.Instance.playerController.GetComponent<CharacterController>();
            player = GameManager.Instance.playerController.GetComponent<BasicPlayer>();

            AbilitySettings abilitySettings = gameManager.gameSettings.Abilities.Where(x => x.abilityType == AbilityType.DASH).First();
            abilityType = abilitySettings.abilityType;
            abilityStats = abilitySettings.abilityStats;

            firePoint = player.transform;
        }


        public void OnAbility(InputValue value) {
            if(value.isPressed)
            {
                Dash();
            }
        }

        void Update()
        {
           if (Input.GetKeyDown(KeyCode.E))
            {
            switch (abilityType)
                {
                    case AbilityType.DASH:
                        Dash();
                        break;
                    case AbilityType.RAPIDHEAL:
                        StartCoroutine(rapidHeal());                  
                        break;
                    case AbilityType.RAPIDFIRE:
                        StartCoroutine(rapidFire());
                        break;
                    case AbilityType.INVISIBILITY:
                         StartCoroutine(invisible());
                        break;

            }

            }
         }

        void Dash()
        {
            if(player.statHandler.Energy > 15)
            {
                moveDirection = transform.forward * checkDashDistance();
                characterController.Move(moveDirection);
                characterController.Move(player.velocity * Time.deltaTime);

                player.statHandler.Energy -= 15f;
            }
            
        }

        float checkDashDistance()
        {
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, abilityStats.dashDistance))
                return hitInfo.distance;
            
            return abilityStats.dashDistance;
        }
   
        IEnumerator rapidHeal()
        {
            int swapStat;
            swapStat = player.statHandler.HealthRegenLevel;
            
            //player.statHandler.currentsStats.HealthRegen *= abilityStats.multiplier;

            yield return new WaitForSeconds(abilityStats.time);

            //player.statHandler.HealthRegenLevel = swapStat;
        }
        IEnumerator rapidFire()
       {
           int swapStat;
           swapStat = player.statHandler.AttackSpeedLevel;

           //player.statHandler.AttackSpeedLevel *= abilityStats.multiplier;

           yield return new WaitForSeconds(abilityStats.time);

           //player.statHandler.AttackSpeedLevel = swapStat;
       }

       IEnumerator invisible()
       {
           player.canTakeDamage = false;
            Debug.Log("test1");
           yield return new WaitForSeconds(abilityStats.time);
            Debug.Log("test2");
           player.canTakeDamage = true;
       }
}