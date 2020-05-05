using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabShooting : MonoBehaviour
{
    public GameObject bullet;

    WeaponStats standardrifleStats;

    void Start()
    {
        standardrifleStats = GameManager.Instance.gameSettings.Weapons.Where(x => x.weaponName == "StandardRifle").First().Stats;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject instBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
            Rigidbody instBulletRigidbody = instBullet.GetComponent<Rigidbody>();
            instBulletRigidbody.AddForce(Vector3.forward * standardrifleStats.shotSpeed);
            Destroy(instBullet, 3f);
        }
    }
}
