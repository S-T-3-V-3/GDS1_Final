using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
 
public class Ability : MonoBehaviour
{
        public Vector3 moveDirection = Vector3.zero;

        GameManager gameManager;
        GameSettings gameSettings;

        AbilitySettings abilitySettings;
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

            GameManager.Instance.scoreManager.OnLevelUp.AddListener(OnLevelUp);

            firePoint = player.transform;
        }


        public void OnAbility(InputValue value) {
            if(value.isPressed && abilitySettings != null)
            {
                switch (abilityType)
                {
                    case AbilityType.DASH:
                        Dash();
                        break;
                    case AbilityType.RAPIDHEAL:
                        RapidHealAbility();
                        break;
                    case AbilityType.RAPIDFIRE:
                        RapidFireAbility();
                        break;
                    case AbilityType.INVISIBILITY:
                        Invisible();
                        break;
                }
            }
        }

        void OnLevelUp()
        {
            if(GameManager.Instance.scoreManager.playerLevel == 5)
            {
                UnlockAbility();
            }
        }

        void UnlockAbility()
        {
            switch(Random.Range(0, 4))
            {
                case 0:
                    abilitySettings = gameManager.gameSettings.Abilities.Where(x => x.abilityType == AbilityType.DASH).First();
                    break;
                case 1:
                    abilitySettings = gameManager.gameSettings.Abilities.Where(x => x.abilityType == AbilityType.RAPIDHEAL).First();
                    break;
                case 2:
                    abilitySettings = gameManager.gameSettings.Abilities.Where(x => x.abilityType == AbilityType.RAPIDFIRE).First();
                    break;
                case 3:
                    abilitySettings = gameManager.gameSettings.Abilities.Where(x => x.abilityType == AbilityType.INVISIBILITY).First();
                    break;
                default:
                    Debug.LogWarning("Out of bounds for ability array");
                    break;
            }
            abilityType = abilitySettings.abilityType;
            abilityStats = abilitySettings.abilityStats;
            GameManager.Instance.hud.SetAbilityText(abilityType);
        }

        void Dash()
        {
            if(player.statHandler.Energy > 15)
            {
                AudioManager.Instance.PlaySoundEffect(SoundType.AbilitySound);
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
   
        void RapidHealAbility()
        {
            if(player.statHandler.Energy == player.statHandler.MaxEnergy)
            {
                AudioManager.Instance.PlaySoundEffect(SoundType.AbilitySound);
                player.statHandler.RegenAbility();
                StartCoroutine(healPlayer());
            }
        }
        IEnumerator healPlayer()
        {
            yield return null;
            if(player.statHandler.Energy > 0)
            {
                Debug.Log("Rapidly healing player");
                player.statHandler.Energy -= 20f * Time.deltaTime;
                StartCoroutine(healPlayer());
            }
            else
            {
                player.statHandler.StopRegenAbility();
            }
        }

        void RapidFireAbility()
        {
            if(player.statHandler.Energy == player.statHandler.MaxEnergy)
            {
                AudioManager.Instance.PlaySoundEffect(SoundType.AbilitySound);
                player.statHandler.AttackSpeedAbility();
                StartCoroutine(rapidFire());
            }
        }
        IEnumerator rapidFire()
        {
            yield return null;
            if(player.statHandler.Energy > 0)
            {
                Debug.Log("Increasing Player Attack Speed");
                player.statHandler.Energy -= 15f * Time.deltaTime;
                StartCoroutine(rapidFire());
            }
            else
            {
                player.statHandler.StopAttackSpeedAbility();
            }
        }

        void Invisible()
        {
            if(player.statHandler.Energy == player.statHandler.MaxEnergy)
            {
                AudioManager.Instance.PlaySoundEffect(SoundType.AbilitySound);
                player.canTakeDamage = false;
                StartCoroutine(ImmunityFrames());
            }
        }
        IEnumerator ImmunityFrames()
        {
            yield return null;
            if(player.statHandler.Energy > 0)
            {
                player.statHandler.Energy -= 20f * Time.deltaTime;
                StartCoroutine(ImmunityFrames());
            }
            else
            {
                player.canTakeDamage = true;
            }

            Debug.Log("Player is invincible: " + !player.canTakeDamage);
        }
}