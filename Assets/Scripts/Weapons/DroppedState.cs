using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroppedState : MonoBehaviour
{
    Transform indicator;
    public WeaponType weaponType;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        indicator = GameObject.Instantiate(GameManager.Instance.gameSettings.dropIndicator, spawnPos, Quaternion.identity).transform;
        GameObject.Destroy(this.gameObject, 10);
        GameObject.Destroy(indicator.gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        indicator.transform.Rotate(0, 2, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;

        BasicPlayer player = other.gameObject.GetComponent<BasicPlayer>();

        if(player.equippedWeapon.weaponType == weaponType)
        {
            GameObject.Destroy(this.gameObject);
            GameObject.Destroy(indicator.gameObject);
            return;
        }

        WeaponStats newStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First().weaponBaseStats;

        switch (weaponType)
        {
            case WeaponType.RIFLE:
                player.EquipWeapon<RifleWeapon>(weaponType, newStats);
                break;
            case WeaponType.SHOTGUN:
                player.EquipWeapon<ShotgunWeapon>(weaponType, newStats);
                break;
            case WeaponType.MACHINE_GUN:
                player.EquipWeapon<RifleWeapon>(weaponType, newStats);
                break;
        }

        GameObject.Destroy(this.gameObject);
        GameObject.Destroy(indicator.gameObject);
    }
}
