using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnState : EnemyState
{
    BasicEnemy enemy;

    public override void BeginState() {
        enemy = this.GetComponent<BasicEnemy>();

        StartCoroutine("GetNextState");
    }

    IEnumerable GetNextState() {
        while (GameManager.Instance == null) {
            yield return new WaitForEndOfFrame();
        }

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
