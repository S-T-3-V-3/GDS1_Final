using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Needs to be clearer that it's a prefab
    public GameObject owningObject;

    WeaponStats rifleStats; // camelCase

    void Start()
    {
        rifleStats = GameManager.Instance.gameSettings.Weapons.Where(x => x.weaponType == WeaponType.RIFLE).First().stats;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject currentBullet = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * rifleStats.shotSpeed);
            currentBullet.GetComponent<BasicProjectile>().range = rifleStats.range;
            currentBullet.GetComponent<BasicProjectile>().damageAmount = rifleStats.weaponDamage;
            currentBullet.GetComponent<BasicProjectile>().owningObject = owningObject;
        }
    }
}
