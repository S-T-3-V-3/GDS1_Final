using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Needs to be clearer that it's a prefab

    WeaponStats standardRifleStats; // camelCase

    void Start()
    {
        standardRifleStats = GameManager.Instance.gameSettings.Weapons.Where(x => x.weaponName == "StandardRifle").First().Stats;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject currentBullet = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * standardRifleStats.shotSpeed);
            GameObject.Destroy(currentBullet, 3f);
        }
    }
}
