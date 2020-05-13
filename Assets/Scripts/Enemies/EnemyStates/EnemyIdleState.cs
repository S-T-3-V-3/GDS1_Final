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

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;

        targetTransform = GameManager.Instance.playerController.transform;
    }

    void Update() {
        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            EnemyTransitionHandler.OnDetectPlayer(this.enemy);
        }
    }
}
