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

    float timeSinceMeleeStrike = 0f;
    float meleeStrikeCooldown = 1f;

    public override void BeginState() {
        enemy = this.gameObject.GetComponent<BasicEnemy>();
        enemyType = enemy.enemyType;
        enemySettings = enemy.enemySettings;
        playerTransform = GameManager.Instance.playerController.transform;
        
        characterController = this.GetComponent<CharacterController>();       
    }

    void FixedUpdate()
    {
        if (enemy.isPaused) return;
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
        else if (enemySettings.weaponType == WeaponType.MELEE && Vector3.Magnitude(this.transform.position - playerTransform.position) < 1.6f) {
            DoMeleeStrike();
        }
    }

    void DoMeleeStrike() {
        if (timeSinceMeleeStrike > 0) return;

        DamageType damage;
        damage.owningObject = this.gameObject;
        damage.impactPosition = this.gameObject.transform.position - playerTransform.position;
        damage.impactVelocity = enemy.transform.forward * 2f;
        damage.damageAmount = 10f;
        damage.isCrit = false;
        damage.isPiercing = false;

        playerTransform.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);   

        StartCoroutine(ReloadMelee()); 
    }

    IEnumerator ReloadMelee() {
        bool isComplete = false;
        while (!isComplete) {
            timeSinceMeleeStrike += Time.deltaTime;

            if (timeSinceMeleeStrike >= meleeStrikeCooldown) {
                timeSinceMeleeStrike = 0f;
                isComplete = true;
            }
            else {
                yield return null;
            }
        }
        yield return null;
    }
}
