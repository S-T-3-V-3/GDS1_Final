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

            enemy.equippedWeapon.Shoot();
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
