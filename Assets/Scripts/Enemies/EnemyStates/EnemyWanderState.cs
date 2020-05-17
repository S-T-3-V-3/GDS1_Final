using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy randomly wanders until they can see the target
public class EnemyWanderState : EnemyState
{
    Vector3 currentTargetLocation;
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;
    Transform playerTransform;
    Rigidbody rb;

    float timeSinceLastUpdate = 0f;
    float nextUpdate;

    bool isStuck = false;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        if (enemySettings.traits.wanderDistance <= 0){
            Debug.LogWarning($"{enemy.enemyType} has no wander distance set.");
            enemy.SetState<EnemyInactiveState>();
        }

        rb = this.GetComponent<Rigidbody>();

        UpdateTargetPosition();

        StartCoroutine(DelayedPositon());
    }

    public void RemoveState() {
        StopCoroutine(DelayedPositon());
    }

    void Update() {
        if (timeSinceLastUpdate > nextUpdate) {
             UpdateTargetPosition();
        }
        else {
            timeSinceLastUpdate += Time.deltaTime;

            Vector3 targetDirection = Vector3.Normalize(this.transform.position - currentTargetLocation);
            Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * (enemySettings.stats.moveSpeed/2));
            rb.MovePosition(newPosition);
            this.transform.rotation = Quaternion.LookRotation(-targetDirection);

            if (isStuck)
                UpdateTargetPosition();
        }

        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            EnemyTransitionHandler.OnDetectPlayer(this.enemy);
        }
    }

    void UpdateTargetPosition() {
        isStuck = false;

        Vector3 newTarget = new Vector3(Random.Range(-enemySettings.traits.wanderDistance, enemySettings.traits.wanderDistance),0,Random.Range(-enemySettings.traits.wanderDistance, enemySettings.traits.wanderDistance));

        currentTargetLocation = Vector3.ClampMagnitude(newTarget, enemySettings.traits.wanderDistance);
        currentTargetLocation += this.transform.position;

        nextUpdate = Random.Range(enemySettings.traits.wanderUpdateFrequency.x, enemySettings.traits.wanderUpdateFrequency.y);
        timeSinceLastUpdate = 0f;
    }

    float GetPlayerDistance() {
        return Vector3.Magnitude(this.transform.position - playerTransform.position);
    }

    IEnumerator DelayedPositon() {
        Vector3 previousPosition;
        Vector3 currentPosition = Vector3.zero;

        while (true) {
            yield return new WaitForSeconds(0.5f);

            if (currentPosition == Vector3.zero) {
                currentPosition = this.transform.position;
            }
            else {
                previousPosition = currentPosition;
                currentPosition = this.transform.position;
                isStuck = Vector3.Magnitude(previousPosition - currentPosition) < enemySettings.traits.wanderStuckDistance;
            }
        }
    }
}