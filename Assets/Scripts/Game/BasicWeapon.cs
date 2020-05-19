using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
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
            currentBullet.damageAmount = weaponStats.weaponDamage;
            currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

            currentBullet.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

            AudioManager.audioInstance.StandardGunFire();
            StartCoroutine(Reload());
        }
    }

    IEnumerator Reload() {
        float timeSinceFired = 0f;
        canShoot = false;

        while (timeSinceFired < weaponStats.fireRate) {
            yield return new WaitForEndOfFrame();
            timeSinceFired += Time.deltaTime;
        }

        canShoot = true;
    }
}
