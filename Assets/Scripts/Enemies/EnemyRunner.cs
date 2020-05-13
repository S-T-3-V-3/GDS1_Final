using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// TODO: Turn all this into state machines very soon!!!
public class EnemyRunner : MonoBehaviour
{
    Transform targetTransform;
    ObjectStats objectStats;
    EnemyStats enemyStats;

    bool hasTarget = false;
    bool isTargetInRange = false;
    float timeSinceLastHit = 0f;

    void Start()
    {
        enemyStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Runner").First().enemyStats;
        objectStats = GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Runner").First().objectStats;

        this.gameObject.GetComponent<BasicEnemy>().Init(GameManager.Instance.gameSettings.Enemies.Where(x => x.enemyStats.enemyName == "Runner").First());
    }

    void FixedUpdate()
    {
        if (hasTarget) {
            isTargetInRange = GetTargetDistance() <= enemyStats.detectionRange;

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
        Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * objectStats.moveSpeed);
        this.GetComponent<Rigidbody>().MovePosition(newPosition);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<IDamageable>() != null) {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
            damage.damageAmount = objectStats.damage;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage);
        }
    }
}
