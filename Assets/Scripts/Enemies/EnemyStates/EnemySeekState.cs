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
    CharacterController characterController;

    float heavyGunnerTimeShooting = 0f;
    float heavyGunnerSinceShooting;
    bool heavyGunnerShooting = false;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;
        
        characterController = this.GetComponent<CharacterController>();       
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        enemy.GravityUpdate();
        characterController.Move(enemy.velocity * Time.deltaTime);

        if (BasicEnemy.IsPlayerInRange(this.enemy)) {
            Vector3 targetDirection = Vector3.Normalize(playerTransform.position - this.transform.position);
            Vector3 newPosition = targetDirection * Time.fixedDeltaTime * (enemy.statHandler.MoveSpeed/2);
            
            characterController.Move(newPosition);
            
            Vector3 lookRotation = new Vector3(targetDirection.x, 0, targetDirection.z);
            this.transform.rotation = Quaternion.LookRotation(lookRotation);
            
            if (enemy.equippedWeapon != null && enemy.equippedWeapon.weaponModel != null)
                enemy.equippedWeapon.weaponModel.transform.LookAt(enemy.equippedWeapon.weaponModel.transform.position + targetDirection * 3f);
        }
        else {
            EnemyTransitionHandler.OnLostPlayer(this.enemy);
        }

        if (enemySettings.weaponType != WeaponType.MELEE && BasicEnemy.IsPlayerInWeaponRange(this.enemy)) {
            EnemyTransitionHandler.OnPlayerInWeaponRange(this.enemy);

            enemy.equippedWeapon.Shoot();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (enemySettings.weaponType != WeaponType.MELEE)
        {
            return;
        }
        
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player Collide");
        }

        if (other.gameObject.GetComponent<IDamageable>() != null && other.gameObject.GetComponent<BasicEnemy>() == null) {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = other.contacts.First().point;
            damage.impactVelocity = enemy.transform.forward * enemy.equippedWeapon.weaponStats.shotSpeed;
            damage.damageAmount = enemy.statHandler.Damage;
            damage.isCrit = false;
            damage.isPiercing = false;

            Debug.Log("Attack");

            other.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);
        }
    }
}
