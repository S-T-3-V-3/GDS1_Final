using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    LineRenderer laserBeam;

    bool laserActive = false;
    bool hasReset = false;

    public override void Shoot()
    {
        if (!canShoot)
        {
            laserBeam.enabled = false;
            return;
        }

        //refreshes line renderer when activating
        if (laserActive == false)
        {
            laserActive = true;
            laserBeam.SetPosition(0, Vector3.zero);
            laserBeam.SetPosition(1, Vector3.zero);
        }

        laserBeam.enabled = true;
        laserBeam.SetPosition(0, firePoint.position);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, weaponStats.range))
        {
            laserBeam.SetPosition(1, firePoint.position + firePoint.forward * hit.distance);
            //JUST ADD THE DAMAGE METHODS FROM THE HIT GAMEOBJECT
        }
        else
        {
            Vector3 futurePosition = firePoint.position + firePoint.forward * 20;
            laserBeam.SetPosition(1, futurePosition);
        }

    }

    public override void AddShotEffect(WeaponDefinition settings)
    {
        if (weaponType == WeaponType.LASER && laserBeam == null)
            laserBeam = GameObject.Instantiate(GameManager.Instance.BeamRayPrefab, firePoint).GetComponent<LineRenderer>();
    }

    public override void DisableLaser()
    {
        if (!hasReset) return;
        hasReset = false;

        if (laserBeam == null) return;
        laserBeam.enabled = false;
        laserActive = false;
    }
}
