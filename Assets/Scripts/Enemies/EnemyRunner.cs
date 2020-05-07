using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyRunner : MonoBehaviour
{
    Transform targetTransform;
    EnemyStats runnerStats;

    bool hasTarget = false;
    bool isTargetInRange = false;

    void Start()
    {
        runnerStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.EnemyName == "Runner").First().enemyStats;
    }

    void FixedUpdate()
    {
        if (hasTarget) {
            isTargetInRange = GetTargetDistance() <= runnerStats.detectionRange;

            if (isTargetInRange)
                MoveToTarget();
        }
        else {
            SearchForTarget();
        }
    }

    void SearchForTarget()
    {
        if (GameManager.Instance.playerController == null) return;

        targetTransform = GameManager.Instance.playerController.transform;
        hasTarget = true;
    }

    float GetTargetDistance() {
        return Vector3.Magnitude(this.transform.position - targetTransform.position);
    }

    void MoveToTarget() {
        Vector3 targetDirection = Vector3.Normalize(this.transform.position - targetTransform.position);
        Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * runnerStats.moveSpeed);
        this.GetComponent<Rigidbody>().MovePosition(newPosition);
    }
}
