using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;
    public WeaponType weaponType;
    public StatHandler ownerStats;
    public WeaponStats weaponStats;
    public Transform firePoint;

    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Vector3 rayDirection;
    [HideInInspector] public Vector3 offset;
    [HideInInspector] public bool canShoot = false;

    public void Init(WeaponDefinition weaponDefinition, Transform gunPosition) {
        weaponModel = GameObject.Instantiate(weaponDefinition.WeaponPrefab);
        firePoint = weaponModel.GetComponent<WeaponTransforms>().firePoint;
    }

    public virtual void Shoot()
    {
        if (!canShoot) return;

        BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, GameManager.Instance.transform).GetComponent<BasicProjectile>();
        currentBullet.transform.position = firePoint.position + firePoint.forward * 0.4f; // Temp until refactor

        currentBullet.owningObject = this.gameObject;
        currentBullet.range = weaponStats.range;
        currentBullet.damageAmount = weaponStats.weaponDamage + ownerStats.Damage;
        currentBullet.previousVelocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

        if (weaponModel != null)
            currentBullet.SetBulletVelocity(weaponModel.transform.forward * weaponStats.shotSpeed);
        else {
            currentBullet.SetBulletVelocity(this.transform.forward * weaponStats.shotSpeed); // Remove when enemies have weapons
        }
        AudioManager.Instance.StandardGunFire();

        StartCoroutine(Reload());
    }

    public virtual void AddShotEffect(WeaponDefinition settings) { }

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