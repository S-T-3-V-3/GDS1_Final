using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleWeapon : Weapon
{
    public override void Init(WeaponDefinition weaponDefinition, Transform gunPosition)
    {
        base.Init(weaponDefinition, gunPosition);
        muzzleFlash = GameObject.Instantiate(GameManager.Instance.gameSettings.MuzzleFlashPrefab, firePoint).GetComponent<ParticleSystem>();
        weaponAim = GetComponent<AutoLookAt>();
        //muzzleFlash.gameObject.transform.parent = firePoint.transform;
    }

    public override void Shoot()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = firePoint.position + firePoint.forward * 0.4f; // Temp until refactor

        if (autoAim && weaponAim.EnemyIsInFieldOfView())
        {
            //Debug.Log("Has Locked Fire");
            currentBullet.transform.LookAt(weaponAim.LockOntoEnemy());
        }

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage * ownerStats.Damage;

        if (weaponModel != null)
        {
            //Slow down enemy rifle shots in comparison to the player
            if(currentBullet.owningObject.GetComponent<BasicPlayer>())
            {
                currentBullet.initVelocity = weaponModel.transform.forward * weaponStats.shotSpeed;
            }
            else
            {
                currentBullet.initVelocity = weaponModel.transform.forward * weaponStats.shotSpeed * 0.3f;
            }
        }
            

        muzzleFlash.Play();
        AudioManager.Instance.onSoundEvent.Invoke(SoundType.Rifle);
        StartCoroutine(Reload());
    }
}
