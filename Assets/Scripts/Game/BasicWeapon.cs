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

    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Vector3 rayDirection;
    [HideInInspector] public Vector3 offset;

    [HideInInspector] public bool canShoot = false;

    public virtual void Shoot()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = firePoint.position + firePoint.forward * 0.4f; // Temp until refactor

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;
        currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

        currentBullet.SetBulletVelocity(this.gameObject.transform.forward * weaponStats.shotSpeed);
        AudioManager.Instance.StandardGunFire();

        StartCoroutine(Reload());
    }

    //THIS IS FOR LASER CLASS ONLY
    public virtual void DisableLaser() { }

    public virtual void AddShotEffect(WeaponSettings settings) { }

    public IEnumerator Reload()
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
