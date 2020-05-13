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
        if (GetTargetDistance() <= enemySettings.detectionRange) {
            switch (enemyType) {
                case EnemyType.RUSHER:
                    enemy.SetState<EnemySeekingState>();
                    break;

                // Sniper
                // Boomer
                // Shotgunner

                default:
                    break;
            }
        }
    }

    float GetTargetDistance() {
        return Vector3.Magnitude(this.transform.position - targetTransform.position);
    }
}
