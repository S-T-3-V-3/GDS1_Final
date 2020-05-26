using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicWeapon : MonoBehaviour
{
    public StatHandler ownerStats;
    public WeaponType weaponType;
    public WeaponStats weaponStats;
    public FireType fireType;
    public Transform firePoint;

    LineRenderer laserBeam; // This should probably exist on an inherited class right?
    ParticleSystem shotgunParticles; // Also should be inherited?

    // This class is shared by AI, be careful
    // Try again on a higher level of heirachy
    /*
    IKWeaponsAnimator weaponsIK;
    PlayerSettings playerSettings;
    AimSystem aimSystem;
    */

    // Temp until refactor
    GameObject weaponModel;

    RaycastHit hit;
    Vector3 rayDirection;
    Vector3 offset;

    bool canShoot = true;
    bool laserActive = false;
    bool hasReset = false;
    bool hasAim = false;

// Player specific stuff, this shouldn't exist on a globally shared weapon class
/*
    void Awake()
    {
        playerSettings = GameManager.Instance.gameSettings.playerSettings;

        if(GetComponent<PlayerController>() != null)
        {
            weaponsIK = gameObject.AddComponent<IKWeaponsAnimator>();
            weaponsIK.playerAnimator = this.GetComponent<BasicPlayer>().animationController;
            aimSystem = Instantiate(playerSettings.aimSystem, transform).GetComponent<AimSystem>();
            hasAim = true;
        }
    }
*/

    public void Shoot()
    {
        if (!hasReset) hasReset = true;
        
        // At this point, inheritence should exist - this is not a base/basic weapon 
        // Basic functions can be overrided and rewritten
        // IE Beam weapon implements Shoot() differently to Spread weapon
        // equippedWeapon.Shoot() will still work anywhere as long as it inherits from BasicWeapon!
        switch (fireType)
        {
            case FireType.SINGLE:
                SingleFire();
                break;
            case FireType.SPREAD:
                SpreadFire();
                break;
            case FireType.BEAM:
                BeamFire();
                break;
        }
    }

    //used to stop or reset weapon (needs better use case)
    // This is specific to lasers not a basic weapon
    /*
    public void DisableLaser()
    {
        if (!hasReset) return;
        hasReset = false;

        if (laserBeam == null) return;
        laserBeam.enabled = false;
        laserActive = false;
    }
    */

    // I don't think a 'Weapon' should be implementing a 'ChangeWeapon' (or drop) function
    // It would make more sense to implement this on a class that owns any given weapon(s)
    // Moved the weapon prefab instantiation here, but this should be handled in an equip weapon function elsewhere 
    public void ChangeWeapons(Transform gunPosition, StatHandler ownerStats, WeaponType type, WeaponStats stats, WeaponSettings settings)
    {
        if (weaponType != type) DropWeapon(); // We still want to drop weapons of the same type!
        weaponType = type;
        fireType = settings.fireType;
        weaponStats = stats;
        this.ownerStats = ownerStats;

        //firePoint = weaponsIK.GetTransformsFromIK(gunPosition, settings.weaponPrefab);
        weaponModel = GameObject.Instantiate(settings.weaponPrefab, this.transform);
        // Temp until refactor, should be instantiated by parent

        if (type == WeaponType.SHOTGUN && shotgunParticles == null)
            shotgunParticles = Instantiate(settings.projectileParticles, firePoint).GetComponent<ParticleSystem>();

        if (type == WeaponType.LASER && laserBeam == null)
            laserBeam = Instantiate(settings.beamRay, firePoint).GetComponent<LineRenderer>();
    }

    void DropWeapon()
    {
    }

/*
    public void RenderAim()
    {
        if (!hasAim) return;
        aimSystem.RenderAimLine(firePoint);
    }
*/
    #region WEAPON FIRE METHODS

    void SingleFire()
    {
        if (!canShoot) return;

        // Instantiating the bullet like this sets it's parent to 'fire point', then changes the parent to the game manager
        // You want to instantiate the bullet with the parent set, then just change the position to the fire point position
        //BasicProjectile currentBullet = Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
        //currentBullet.transform.parent = GameManager.Instance.transform;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = this.transform.position + this.transform.forward * 0.4f; // Temp until refactor

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;
        currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

        currentBullet.SetBulletVelocity(this.gameObject.transform.forward * weaponStats.shotSpeed);
        AudioManager.Instance.StandardGunFire();

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
            if (Physics.Raycast(firePoint.position, rayDirection, out hit ,weaponStats.range))
            {
                Debug.DrawLine(firePoint.position, hit.point, Color.cyan, 5f);
                //JUST ADD THE DAMAGE METHODS FROM THE HIT GAMEOBJECT
            }
        }

        shotgunParticles.emission.SetBurst( 0, new ParticleSystem.Burst(0, weaponStats.numBullets));
        shotgunParticles.Play();

        StartCoroutine(Reload());
    }

    void BeamFire()
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
    
    #endregion
    
    //MIGHT BE USEFUL LATER ON
    void RaycastDamage()
    {
    }

    IEnumerator Reload()
    {
        canShoot = false;
        float timeSinceFired = 0;

        while (timeSinceFired < (1/ownerStats.AttackSpeed))
        {
            yield return null;
            timeSinceFired += Time.deltaTime;
        }

        canShoot = true;
    }
}
