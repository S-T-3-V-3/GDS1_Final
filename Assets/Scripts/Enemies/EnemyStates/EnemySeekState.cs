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

    bool canShoot = true;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;

        rb = this.GetComponent<Rigidbody>();        
    }

    void FixedUpdate()
    {
        if (enemySettings.traits.canShootAndSeek)
            Shoot();

        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            Vector3 targetDirection = Vector3.Normalize(this.transform.position - playerTransform.position);
            Vector3 newPosition = this.transform.position - (targetDirection * Time.fixedDeltaTime * enemySettings.stats.moveSpeed);
            this.GetComponent<Rigidbody>().MovePosition(newPosition);
        }
        else {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }
    }

    void Shoot() {
        if (canShoot == false) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, enemy.firePoint).GetComponent<BasicProjectile>();
        currentBullet.transform.parent = GameManager.Instance.transform;
        currentBullet.owningObject = this.gameObject;
        currentBullet.range = GameManager.Instance.gameSettings.Weapons.First().stats.range;

        currentBullet.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * GameManager.Instance.gameSettings.Weapons.First().stats.shotSpeed); // TODO: use actual weapon

        StartCoroutine(ReloadWeapon());
    }

    IEnumerator ReloadWeapon() {
        float elapsedTime = 0f;
        canShoot = false;

        while (elapsedTime <= enemySettings.stats.fireRate) {
            yield return new WaitForEndOfFrame();

            elapsedTime += Time.deltaTime;
        }

        canShoot = true;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<IDamageable>() != null) {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = this.gameObject.GetComponent<Rigidbody>().velocity;
            damage.damageAmount = enemySettings.stats.damage;
            damage.isCrit = false;
            damage.isPiercing = false;

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage);
        }
    }
}
