using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon
{
    public override void Init(WeaponDefinition weaponDefinition, Transform gunPosition)
    {
        base.Init(weaponDefinition, gunPosition);
        muzzleFlash = GameObject.Instantiate(GameManager.Instance.gameSettings.MuzzleFlashPrefab, firePoint).GetComponent<ParticleSystem>();
        //muzzleFlash.gameObject.transform.parent = firePoint.transform;
    }

    public override void Shoot()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = firePoint.position + firePoint.forward * 0.4f; // Temp until refactor

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;

        if (weaponModel != null)
        {
            if(currentBullet.owningObject.GetComponent<BasicPlayer>())
            {
                currentBullet.initVelocity = weaponModel.transform.forward * weaponStats.shotSpeed;
            }
            else
            {
                currentBullet.initVelocity = weaponModel.transform.forward * weaponStats.shotSpeed * 0.35f;
            }
        }
            

        muzzleFlash.Play();
        AudioManager.Instance.onSoundEvent.Invoke(SoundType.Rifle);
        StartCoroutine(Reload());
    }
}
