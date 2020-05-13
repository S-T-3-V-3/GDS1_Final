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

    bool isStuck = false;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        rb = this.GetComponent<Rigidbody>();

        UpdateTargetPosition();

        StartCoroutine("DelayedPosition");
    }

    public void RemoveState() {
        StopCoroutine("DelayedPosition");
    }

    void Update() {
        if (timeSinceLastUpdate > enemySettings.wanderUpdateFrequency) {
             UpdateTargetPosition();
        }
        else {
            timeSinceLastUpdate += Time.deltaTime;

            Vector3 targetDirection = Vector3.Normalize(this.transform.position - currentTargetLocation);
            Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * enemySettings.stats.moveSpeed);
            rb.MovePosition(newPosition);

            if (isStuck)
                UpdateTargetPosition();
        }
    }

    void UpdateTargetPosition() {
        Vector3 newTarget = new Vector3(Random.Range(-enemySettings.wanderDistance, enemySettings.wanderDistance),this.transform.position.y,Random.Range(-enemySettings.wanderDistance, enemySettings.wanderDistance));
        currentTargetLocation = Vector3.ClampMagnitude(newTarget, enemySettings.wanderDistance);
        timeSinceLastUpdate = 0f;
    }

    float GetPlayerDistance() {
        return Vector3.Magnitude(this.transform.position - playerTransform.position);
    }

    IEnumerable DelayedPositon() {
        Vector3 previousPosition;
        Vector3 currentPosition = Vector3.zero;

        while (true) {
            yield return new WaitForSeconds(1);

            if (currentPosition == Vector3.zero) {
                currentPosition = this.transform.position;
            }
            else {
                previousPosition = currentPosition;
                currentPosition = this.transform.position;
                isStuck = Vector3.Magnitude(previousPosition - currentPosition) < enemySettings.wanderStuckDistance;
            }
        }
    }
}