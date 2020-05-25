using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicWeapon : MonoBehaviour
{
    public ObjectStats objectStats;
    public WeaponType weaponType;
    public WeaponStats weaponStats;
    public FireType fireType;
    public Transform firePoint;

    LineRenderer laserBeam;
    ParticleSystem shotgunParticles;
    IKWeaponsAnimator weaponsIK;
    PlayerSettings playerSettings;

    RaycastHit hit;
    Vector3 rayDirection;
    Vector3 offset;

    bool canShoot = true;
    bool laserActive = false;
    bool hasReset = false;

    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;
        weaponsIK = gameObject.AddComponent<IKWeaponsAnimator>();
    }

    //PRIMARY RUN METHOD
    public void RunWeapon()
    {
        if (!hasReset) hasReset = true;
        switch (fireType)
        {
            case FireType.SINGLE:
                BulletFire();
                break;
            case FireType.SPREAD:
                SpreadFire();
                break;
            case FireType.BEAM:
                LaserFire();
                break;
        }
    }

    //used to stop or reset weapon (needs better use case)
    public void StopWeapon()
    {
        if (!hasReset) return;

        hasReset = false;
        laserBeam.enabled = false;
        laserActive = false;
    }

    public void ChangeWeapons(Transform gunPosition, ObjectStats playerStats, WeaponType type, WeaponStats stats, WeaponSettings settings)
    {
        if (weaponType != type) DropWeapon();
        weaponType = type;
        fireType = settings.fireType;
        weaponStats = stats;
        objectStats = playerStats;

        firePoint = weaponsIK.GetTransformsFromIK(gunPosition, settings.weaponPrefab);

        if (type == WeaponType.SHOTGUN && shotgunParticles == null)
            shotgunParticles = Instantiate(settings.projectileParticles, firePoint).GetComponent<ParticleSystem>();

        if (type == WeaponType.LASER && laserBeam == null)
            laserBeam = Instantiate(settings.beamRay, firePoint).GetComponent<LineRenderer>();
    }

    void DropWeapon()
    {

    }

    #region WEAPON FIRE METHODS

    void BulletFire()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
        currentBullet.transform.parent = GameManager.Instance.transform;
        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + objectStats.damage;
        currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

        currentBullet.SetBulletVelocity(this.gameObject.transform.forward * weaponStats.shotSpeed);
        AudioManager.audioInstance.StandardGunFire();

        StartCoroutine(Reload());
    }

    void SpreadFire()
    {
        if (!canShoot) return;

        for (int i = 0; i < weaponStats.numBullets; i++)
        {
            offset = transform.up * Random.Range(-5, 5);
            offset = Quaternion.AngleAxis(Random.Range(0, 360), transform.forward) * offset;
            rayDirection = firePoint.forward * 10 + offset; //may need some fine tuning

            //Debug.DrawRay(firePoint.position, rayDirection, Color.cyan, 5);
            if (Physics.Raycast(firePoint.position, rayDirection, out hit ,weaponStats.range, playerSettings.projectileMask))
            {
                Debug.DrawLine(firePoint.position, hit.point, Color.cyan, 5f);
                //JUST ADD THE DAMAGE METHODS FROM THE HIT GAMEOBJECT
            }
        }

        shotgunParticles.emission.SetBurst( 0, new ParticleSystem.Burst(0, weaponStats.numBullets));
        shotgunParticles.Play();

        StartCoroutine(Reload());
    }

    void LaserFire()
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
            Debug.Log("isHitting");
            Vector3 futurePosition = firePoint.position + firePoint.forward * 20;
            laserBeam.SetPosition(1, futurePosition);
        }

    }

    //MIGHT BE USEFUL LATER ON
    void RaycastDamage()
    {
    }

    IEnumerator Reload()
    {
        canShoot = false;
        yield return new WaitForSeconds(objectStats.fireRate);
        canShoot = true;
    }

    #endregion
}
