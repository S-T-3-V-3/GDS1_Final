using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnState : EnemyState
{
    BasicEnemy enemy;

    public override void BeginState() {
        enemy = this.GetComponent<BasicEnemy>();

        StartCoroutine(GetNextState());
    }

    IEnumerator GetNextState() {
        while (GameManager.Instance == null) {
            yield return null;
        }

        EnemyTransitionHandler.OnStart(enemy);
    }
}
