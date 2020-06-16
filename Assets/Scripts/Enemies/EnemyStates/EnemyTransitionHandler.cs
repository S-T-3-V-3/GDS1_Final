using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyTransitionHandler
{
    public static void OnStart(BasicEnemy enemy) {
        if (GameManager.Instance.playerController == null)
            enemy.SetState<EnemyInactiveState>();

        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.spotLight.gameObject.SetActive(true);
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.RUSHER:
                enemy.SetState<EnemyIdleState>();
                break;

            case EnemyType.HEAVY_GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.TACTICAL_GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.SNIPER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.SHOTGUNNER:
                enemy.SetState<EnemyWanderState>();
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

            case EnemyType.HEAVY_GUNNER:
                enemy.SetState<EnemySeekState>();
                break;

            case EnemyType.TACTICAL_GUNNER:
                enemy.SetState<EnemyDodgeState>();
                break;

            case EnemyType.SNIPER:
                enemy.SetState<EnemyDodgeState>();
                break;

            case EnemyType.SHOTGUNNER:
                enemy.SetState<EnemyDodgeState>();
                break;

            default:
                break;
        }
    }

    public static void OnPlayerInWeaponRange(BasicEnemy enemy) {
        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.SetState<EnemyPivotState>();
                break;

            case EnemyType.TACTICAL_GUNNER:
                enemy.SetState<EnemyDodgeState>();
                break;

            case EnemyType.SNIPER:
                enemy.SetState<EnemyDodgeState>();
                break;

            case EnemyType.SHOTGUNNER:
                enemy.SetState<EnemyDodgeState>();
                break;

            default:
                break;
        }
    }

    public static void OnLostPlayer(BasicEnemy enemy) {
        if (GameManager.Instance.playerController == null) {
            enemy.SetState<EnemyInactiveState>();
            return;
        }

        switch (enemy.enemyType) {
            case EnemyType.GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.RUSHER:
                enemy.SetState<EnemyIdleState>();
                break;

            case EnemyType.HEAVY_GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.TACTICAL_GUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.SNIPER:
                enemy.SetState<EnemyWanderState>();
                break;

            case EnemyType.SHOTGUNNER:
                enemy.SetState<EnemyWanderState>();
                break;

            default:
                break;
        }
    }
}