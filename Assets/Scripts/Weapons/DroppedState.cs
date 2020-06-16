using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroppedState : MonoBehaviour
{
    public WeaponType weaponType;
    public MeshRenderer[] indicatorRenderers;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, 10);
    }

    public void Init(string originName){
        Material glowMat;

        if(originName.Equals("Player"))
            glowMat = GameManager.Instance.gameSettings.blueGlowMaterial;
        else 
            glowMat = GameManager.Instance.gameSettings.yellowGlowMaterial;

        foreach (MeshRenderer rend in indicatorRenderers) {
            rend.material = glowMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 2, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;

        BasicPlayer player = other.gameObject.GetComponent<BasicPlayer>();
        if(player.equippedWeapon.weaponType == weaponType)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        WeaponStats newStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First().weaponBaseStats;
        switch (weaponType)
        {
            case WeaponType.RIFLE:
                player.EquipWeapon<RifleWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADRifle);
                break;
            case WeaponType.SHOTGUN:
                player.EquipWeapon<ShotgunWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADShotgun);
                break;
            case WeaponType.MACHINE_GUN:
                player.EquipWeapon<RifleWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADMachine);
                break;
        }

        GameObject.Destroy(this.gameObject);
    }
}
