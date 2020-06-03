using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserWeapon : Weapon
{
    LineRenderer laserBeam;
    bool isLaserActive = false;
    float elapsedTime = 0f;
    float resetDelay = 0.1f;

    public override void Shoot()
    {
        elapsedTime = 0f;

        if (!canShoot)
        {
            laserBeam.enabled = false;
            return;
        }

        //refreshes line renderer when activating
        if (isLaserActive == false)
        {
            isLaserActive = true;
            StartCoroutine(LaserActiveCheck());
            
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
            laserBeam = GameObject.Instantiate(GameManager.Instance.gameSettings.BeamRayEffectPrefab, firePoint).GetComponent<LineRenderer>();
    }

    IEnumerator LaserActiveCheck() {
        while (isLaserActive) {
            yield return null;

            elapsedTime += Time.deltaTime;

            if (elapsedTime > resetDelay) {
                isLaserActive = false;
                laserBeam.enabled = false;
            }
        }
    }
}
