using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPivotState : EnemyState
{
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;
    Transform playerTransform;
    Rigidbody rb;

    float heavyGunnerTimeShooting = 0f;
    float heavyGunnerSinceShooting;
    bool heavyGunnerShooting = false;

    public override void BeginState()
    {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        rb = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (BasicEnemy.IsPlayerInRange(this.enemy))
        {
            Vector3 targetDirection = Vector3.Normalize(this.transform.position - playerTransform.position);
            this.transform.rotation = Quaternion.LookRotation(-targetDirection);
        }
        else
        {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }

        if (enemyType != EnemyType.HEAVY_GUNNER)
        {
            enemy.equippedWeapon.Shoot();
        } else
        {
            if (heavyGunnerShooting == false && heavyGunnerTimeShooting < heavyGunnerSinceShooting)
            {
                heavyGunnerTimeShooting = Time.deltaTime;
            }
            if (heavyGunnerShooting == false && heavyGunnerTimeShooting >= heavyGunnerSinceShooting)
            {
                heavyGunnerTimeShooting = Time.deltaTime;
                heavyGunnerSinceShooting = Time.deltaTime + 3f;
                heavyGunnerShooting = true;
            }
            else if (heavyGunnerShooting == true && heavyGunnerTimeShooting < heavyGunnerSinceShooting)
            {
                heavyGunnerTimeShooting = Time.deltaTime;
                enemy.equippedWeapon.Shoot();
            }
            else if (heavyGunnerShooting == true && heavyGunnerTimeShooting >= heavyGunnerSinceShooting)
            {
                heavyGunnerTimeShooting = Time.deltaTime;
                heavyGunnerSinceShooting = Time.deltaTime + 1.5f;
                heavyGunnerShooting = false;
            }
        }
    }
}
