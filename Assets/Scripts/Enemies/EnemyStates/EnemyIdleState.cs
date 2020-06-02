using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy stays in place, waiting for target to come in detection range
public class EnemyIdleState : EnemyState
{
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;

    Transform targetTransform;
    CharacterController characterController;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;

        targetTransform = GameManager.Instance.playerController.transform;
        characterController = this.GetComponent<CharacterController>();  
    }

    void Update() {
        if (targetTransform == null) return;

        enemy.GravityUpdate();
        characterController.Move(enemy.velocity * Time.deltaTime);

        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            EnemyTransitionHandler.OnDetectPlayer(this.enemy);
        }
    }
}
