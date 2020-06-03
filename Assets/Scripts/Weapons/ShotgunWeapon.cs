using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///
/// IMPORTANT NOTES:
/// - range can be dictated by stats alongside the number of shotgun pellets that are fired.
/// - This weapon is ray cast based but with short range (shotgun like).
/// 
/// </summary>

public class ShotgunWeapon : Weapon
{
    ParticleSystem shotgunParticles;

    public override void Init(WeaponDefinition weaponDefinition, Transform gunPosition)
    {
        base.Init(weaponDefinition, gunPosition);
        muzzleFlash = GameObject.Instantiate(GameManager.Instance.gameSettings.MuzzleFlashPrefab, firePoint).GetComponent<ParticleSystem>();
        firePoint.transform.parent = muzzleFlash.gameObject.transform;
    }

    public override void Shoot()
    {
        if (!canShoot) return;

        for (int i = 0; i < weaponStats.numBullets; i++)
        {
            offset = transform.up * Random.Range(-5, 5);
            offset = Quaternion.AngleAxis(Random.Range(0, 360), transform.forward) * offset;
            rayDirection = firePoint.forward * 10 + offset; //may need some fine tuning

            if (Physics.Raycast(firePoint.position, rayDirection, out hit, weaponStats.range))
            {
                ShotgunImpact();
            }
        }

        AudioManager.Instance.onSoundEvent.Invoke(SoundType.Shotgun);
        shotgunParticles.emission.SetBurst(0, new ParticleSystem.Burst(0, weaponStats.numBullets));
        shotgunParticles.Play();
        muzzleFlash.Play();

        StartCoroutine(Reload());
    }


    //TODO >> This impact method is abit overkill and the stats havnt adjusted accordingly
    void ShotgunImpact()
    {
        GameObject collisionEntity = hit.collider.gameObject;
        if (collisionEntity.gameObject.GetComponent<IDamageable>() != null)
        {
            DamageType damage;
            damage.owningObject = this.gameObject;
            damage.impactPosition = hit.point;
            damage.impactVelocity = this.weaponStats.shotSpeed * rayDirection;
            damage.damageAmount = this.weaponStats.weaponDamage;
            damage.isCrit = false;
            damage.isPiercing = false;

            collisionEntity.gameObject.GetComponent<IDamageable>().OnReceivedDamage(damage, damage.impactPosition, damage.impactVelocity.normalized, damage.impactVelocity.magnitude);

            //Die();
        }
        else
        {
            //Die();
        }
    }

    public override void AddShotEffect(WeaponDefinition settings)
    {
        if (weaponType == WeaponType.SHOTGUN && shotgunParticles == null)
            shotgunParticles = Instantiate(GameManager.Instance.gameSettings.ShotgunParticlePrefab, firePoint).GetComponent<ParticleSystem>();
    }
}
