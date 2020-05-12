using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyRunner : MonoBehaviour
{
    Transform targetTransform;
    EnemyStats runnerStats;

    bool hasTarget = false;
    bool isTargetInRange = false;
    float timeSinceLastHit = 0f;

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

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<IDamageable>() != null) {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
            damage.damageAmount = runnerStats.damage;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage);
        }
    }
}
