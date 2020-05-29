using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    ParticleSystem shotgunParticles;

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
                Debug.DrawLine(firePoint.position, hit.point, Color.cyan, 5f);
                //JUST ADD THE DAMAGE METHODS FROM THE HIT GAMEOBJECT
            }
        }

        shotgunParticles.emission.SetBurst(0, new ParticleSystem.Burst(0, weaponStats.numBullets));
        shotgunParticles.Play();

        StartCoroutine(Reload());
    }

    public override void AddShotEffect(WeaponDefinition settings)
    {
        if (weaponType == WeaponType.SHOTGUN && shotgunParticles == null)
            shotgunParticles = Instantiate(GameManager.Instance.ShotgunParticlePrefab, firePoint).GetComponent<ParticleSystem>();
    }
}
