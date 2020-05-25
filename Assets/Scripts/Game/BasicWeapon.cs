using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public StatHandler ownerStats;
    public WeaponType weaponType;
    public WeaponStats weaponStats;
    public FireType fireType;
    public Transform firePoint;

    bool canShoot = true;

    public void Shoot() {
        if (canShoot) {
            BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
            currentBullet.transform.parent = GameManager.Instance.transform;
            currentBullet.owningObject = this.gameObject;
            currentBullet.range = weaponStats.range;
            currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;
            currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

            currentBullet.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

            AudioManager.audioInstance.StandardGunFire();
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload() {
        float timeSinceFired = 0f;
        canShoot = false;

        while (timeSinceFired < (1/ownerStats.AttackSpeed)) {
            yield return null;
            timeSinceFired += Time.deltaTime;
        }

        canShoot = true;
    }
}
