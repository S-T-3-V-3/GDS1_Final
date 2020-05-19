using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWeapon : MonoBehaviour
{
    public WeaponType weaponType;
    public WeaponStats weaponStats;
    public FireType fireType;
    public Transform firePoint;

    bool canShoot = true;

    List<Quaternion> bullets;

    void Start()
    {
        bullets = new List<Quaternion>(weaponStats.numBullets);
        for (int i = 0; i < weaponStats.numBullets; i++)
        {
            bullets.Add(Quaternion.Euler(Vector3.zero));
        }
    }

    public void Shoot() {
        switch((int)fireType)
            {
                case 0: singleShoot();
                        break;
                case 1: spreadShoot();
                        break;
                case 2: burstShoot();
                        break;
        }
    }

    IEnumerator Reload() {
        float timeSinceFired = 0f;
        canShoot = false;

        while (timeSinceFired < weaponStats.fireRate) {
            yield return new WaitForEndOfFrame();
            timeSinceFired += Time.deltaTime;
        }

        canShoot = true;
    }

    void singleShoot()
    {
        if (canShoot)
        {
            BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
            currentBullet.transform.parent = GameManager.Instance.transform;
            currentBullet.owningObject = this.gameObject;
            currentBullet.range = weaponStats.range;
            currentBullet.damageAmount = weaponStats.weaponDamage;

            currentBullet.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * weaponStats.shotSpeed;

            StartCoroutine(Reload());
        }
    }

    void spreadShoot()
    {
        if (canShoot)
        {
            for (int i = 0; i < weaponStats.numBullets; i++)
            {
                bullets[i] = Random.rotation;
                BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
                currentBullet.transform.parent = GameManager.Instance.transform;
                currentBullet.transform.rotation = Quaternion.RotateTowards(currentBullet.transform.rotation, bullets[i], 10);
                currentBullet.owningObject = this.gameObject;
                currentBullet.range = weaponStats.range;
                currentBullet.damageAmount = weaponStats.weaponDamage;

                currentBullet.GetComponent<Rigidbody>().velocity = currentBullet.gameObject.transform.forward * weaponStats.shotSpeed;
                StartCoroutine(Reload());
            }
            
        }
    }

    void burstShoot()
    {
        if (canShoot)
        {
            for (int i = 0; i < weaponStats.numBullets; i++)
            {
                BasicProjectile currentBullet = GameObject.Instantiate(GameManager.Instance.ProjectilePrefab, firePoint).GetComponent<BasicProjectile>();
                currentBullet.transform.parent = GameManager.Instance.transform;
                currentBullet.owningObject = this.gameObject;
                currentBullet.range = weaponStats.range;
                currentBullet.damageAmount = weaponStats.weaponDamage;

                currentBullet.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * weaponStats.shotSpeed;                
            }
            StartCoroutine(Reload());
        }
    }
}   