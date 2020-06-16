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

        AudioManager.Instance.onSoundEvent.Invoke(SoundType.Shotgun);
        shotgunParticles.emission.SetBurst(0, new ParticleSystem.Burst(0, weaponStats.numBullets));
        shotgunParticles.Play();
        muzzleFlash.Play();

        StartCoroutine(Reload());
    }

    public override void AddShotEffect(WeaponDefinition settings)
    {
        if (weaponType == WeaponType.SHOTGUN && shotgunParticles == null)
            shotgunParticles = Instantiate(GameManager.Instance.gameSettings.ShotgunParticlePrefab, firePoint).GetComponent<ParticleSystem>();
            shotgunParticles.Stop();
    }
}
