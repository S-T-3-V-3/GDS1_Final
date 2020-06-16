﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDodgeState : EnemyState
{
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;
    Transform playerTransform;
    CharacterController characterController;
    Vector3 currentTargetLocation;

    float timeSinceLastUpdate = 0f;
    float nextUpdate;

    bool isStuck = false;

    // Start is called before the first frame update
    public override void BeginState()
    {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        if (enemySettings.traits.wanderDistance <= 0)
        {
            Debug.LogWarning($"{enemy.enemyType} has no wander distance set.");
            enemy.SetState<EnemyInactiveState>();
        }

        characterController = this.GetComponent<CharacterController>();

        UpdateTargetPosition();

        StartCoroutine(DelayedPositon());
    }

    public void RemoveState()
    {
        StopCoroutine(DelayedPositon());
    }
    
    void Update()
    {
        if (enemy.isPaused) return;

        enemy.GravityUpdate();
        characterController.Move(enemy.velocity * Time.deltaTime);

        if (timeSinceLastUpdate > nextUpdate)
        {
            UpdateTargetPosition();
        }
        else if (BasicEnemy.IsPlayerInWeaponRange(this.enemy))
        {
            timeSinceLastUpdate += Time.deltaTime;

            if (Vector3.Magnitude(this.transform.position - currentTargetLocation) > 0.5f)
            {
                Vector3 targetDirection = Vector3.Normalize(currentTargetLocation - this.transform.position);
                Vector3 newPosition = targetDirection * Time.fixedDeltaTime * (enemy.statHandler.MoveSpeed / 2);
                characterController.Move(newPosition);

                Vector3 playerDirection = Vector3.Normalize(playerTransform.position - this.transform.position);
                Vector3 lookRotation = new Vector3(playerDirection.x, 0, playerDirection.z);
                this.transform.rotation = Quaternion.LookRotation(lookRotation);

                if (enemy.equippedWeapon.weaponModel != null)
                    enemy.equippedWeapon.weaponModel.transform.LookAt(enemy.equippedWeapon.weaponModel.transform.position + playerDirection * 3f);

                if (isStuck)
                    UpdateTargetPosition();

                enemy.equippedWeapon.Shoot();
            }
        }
        else if (BasicEnemy.IsPlayerInRange(this.enemy))
        {
            EnemyTransitionHandler.OnDetectPlayer(this.enemy);
        }
        else
        {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }
    }

    void UpdateTargetPosition()
    {
        isStuck = false;

        Vector3 newTarget = new Vector3(Random.Range(-enemySettings.traits.wanderDistance, enemySettings.traits.wanderDistance), 0, Random.Range(-enemySettings.traits.wanderDistance, enemySettings.traits.wanderDistance));

        currentTargetLocation = Vector3.ClampMagnitude(newTarget, enemySettings.traits.wanderDistance);
        currentTargetLocation += this.transform.position;

        nextUpdate = Random.Range(enemySettings.traits.wanderUpdateFrequency.x, enemySettings.traits.wanderUpdateFrequency.y);
        timeSinceLastUpdate = 0f;
    }

    float GetPlayerDistance()
    {
        return Vector3.Magnitude(this.transform.position - playerTransform.position);
    }

    IEnumerator DelayedPositon()
    {
        Vector3 previousPosition;
        Vector3 currentPosition = Vector3.zero;

        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (currentPosition == Vector3.zero)
            {
                currentPosition = this.transform.position;
            }
            else
            {
                previousPosition = currentPosition;
                currentPosition = this.transform.position;
                isStuck = Vector3.Magnitude(previousPosition - currentPosition) < enemySettings.traits.wanderStuckDistance;
            }
        }
    }
}
