using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyTransitionHandler
{
    public static void OnStart(BasicEnemy enemy) {
        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.spotLight.gameObject.SetActive(true);
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.RUSHER:
                enemy.SetState<EnemyIdleState>();
                break;

            default:
                break;
        }
    }

    public static void OnDetectPlayer(BasicEnemy enemy) {
        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.SetState<EnemySeekState>();
                break;

            case EnemyType.RUSHER:
                enemy.SetState<EnemySeekState>();
                break;

            default:
                break;
        }
    }

    public static void OnLostPlayer(BasicEnemy enemy) {
        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.RUSHER:
                enemy.SetState<EnemyIdleState>();
                break;

            default:
                break;
        }
    }
}