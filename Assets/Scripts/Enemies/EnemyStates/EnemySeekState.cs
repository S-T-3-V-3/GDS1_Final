using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Enemy persues target, shoots if they can
// If enemy is no longer in range, go back to spawn state(?)
public class EnemySeekState : EnemyState
{
    BasicEnemy enemy;
    EnemyType enemyType;
    EnemySettings enemySettings;
    Transform playerTransform;
    Rigidbody rb;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        rb = this.GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            Vector3 targetDirection = Vector3.Normalize(this.transform.position - playerTransform.position);
            Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * enemySettings.stats.moveSpeed);
            this.GetComponent<Rigidbody>().MovePosition(newPosition);
            this.transform.rotation = Quaternion.LookRotation(-targetDirection);
        }
        else {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }

        if (enemySettings.traits.canShootAndSeek)
            enemy.equippedWeapon.Shoot();
    }

    private void OnCollisionEnter(Collision other) {
        if (enemySettings.weaponType != WeaponType.MELEE) return;

        if (other.gameObject.GetComponent<IDamageable>() != null && other.gameObject.GetComponent<BasicEnemy>() == null) {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
            damage.damageAmount = enemySettings.stats.damage;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);
        }
    }
}
