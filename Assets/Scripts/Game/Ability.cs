using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
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

        void Update()
        {
           if (Input.GetKeyDown(KeyCode.E))
            {
            switch (abilityType)
                {
                    case AbilityType.DASH:
                        StartCoroutine(dash());
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

        IEnumerator dash()
        {
            checkDash();
            GameManager.Instance.playerController.stateManager.RemoveState();
            moveDirection = transform.forward * abilityStats.dashDistance;
            characterController.Move(moveDirection);
            characterController.Move(player.velocity * Time.deltaTime);
           

            yield return new WaitForSeconds(abilityStats.time);

            moveDirection = Vector3.zero;
            GameManager.Instance.playerController.stateManager.AddState<MovementState>();
        }

        void checkDash()
        {
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            RaycastHit hitInfo;
            float swapStats = abilityStats.dashDistance;
            if (Physics.Raycast(ray, out hitInfo, abilityStats.dashDistance))
                abilityStats.dashDistance = hitInfo.distance;
            Debug.Log(hitInfo.distance);
   
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
       Debug.Log("de");
           yield return new WaitForSeconds(abilityStats.time);

           player.canTakeDamage = true;
       }
}