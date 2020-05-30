using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon
{

    public override void Shoot()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = firePoint.position + firePoint.forward * 0.4f; // Temp until refactor

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;

        if (weaponModel != null)
            currentBullet.initVelocity = weaponModel.transform.forward * weaponStats.shotSpeed;
        else
        {
            currentBullet.initVelocity = this.transform.forward * weaponStats.shotSpeed; // Remove when enemies have weapons
        }

        AudioManager.Instance.onSoundEvent.Invoke(SoundType.Rifle);
        StartCoroutine(Reload());
    }
}
