using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPivotState : EnemyState
{
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;
    Transform playerTransform;
    CharacterController characterController;
    private float cooldownTime = 0;
    private float shootTime = 0;
    private bool isShooting = false;

    public override void BeginState()
    {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;
        characterController = this.GetComponent<CharacterController>();    
    }

    private void FixedUpdate()
    {
        enemy.GravityUpdate();
        characterController.Move(enemy.velocity * Time.deltaTime);

        if (BasicEnemy.IsPlayerInWeaponRange(this.enemy))
        {
            Vector3 targetDirection = Vector3.Normalize(playerTransform.position - this.transform.position);
            Vector3 lookRotation = new Vector3(targetDirection.x, 0, targetDirection.z);
            this.transform.rotation = Quaternion.LookRotation(lookRotation);
            
            if (enemy.equippedWeapon.weaponModel != null)
                enemy.equippedWeapon.weaponModel.transform.LookAt(enemy.equippedWeapon.weaponModel.transform.position + targetDirection * 3f);

            if (enemyType != EnemyType.HEAVY_GUNNER)
            {
                enemy.equippedWeapon.Shoot();
            } else
            {
                if(isShooting == false && Time.time >= cooldownTime)
                {
                    isShooting = true;
                    enemy.equippedWeapon.Shoot();
                    shootTime = Time.time + 3f;
                }else if(isShooting == true && Time.time <= shootTime)
                {
                    enemy.equippedWeapon.Shoot();
                }
                else if(isShooting == true && Time.time >= shootTime)
                {
                    isShooting = false;
                    cooldownTime = Time.time + 1.5f;
                }
            }
        }
        else if (BasicEnemy.IsPlayerInRange(this.enemy))
        {
            EnemyTransitionHandler.OnDetectPlayer(this.enemy);
        }
        else {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }
    }
}
