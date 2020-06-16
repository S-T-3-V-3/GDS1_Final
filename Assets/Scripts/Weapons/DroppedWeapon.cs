using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class DroppedWeapon : MonoBehaviour
{
    public BasicPlayer playerReference;
    public Transform modelDisplayPoint;
    public WeaponType weaponType;
    public MeshRenderer[] indicatorRenderers;

    GameObject weaponModel;

    // Start is called before the first frame update
    void Start()
    {
        DelayedAction d = this.gameObject.AddComponent<DelayedAction>();
        d.maxDelayTime = 15f;
    }

    public void Init(GameObject weaponModel, string originName){
        Material glowMat;
        weaponModel = Instantiate(weaponModel, modelDisplayPoint.position, Quaternion.identity);
        weaponModel.transform.parent = this.transform;
        WeaponTransforms weapons = weaponModel.GetComponent<WeaponTransforms>();

        glowMat = GameManager.Instance.gameSettings.yellowGlowMaterial;

        foreach (MeshRenderer rend in indicatorRenderers) {
            rend.material = glowMat;
        }

        weapons.model.GetComponent<MeshRenderer>().material = glowMat;
    }

    public void EquipSelected(){
        WeaponStats newStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First().weaponBaseStats;
        switch (weaponType)
        {
            case WeaponType.RIFLE:
                playerReference.EquipWeapon<RifleWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADRifle);
                break;
            case WeaponType.SHOTGUN:
                playerReference.EquipWeapon<ShotgunWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADShotgun);
                break;
            case WeaponType.MACHINE_GUN:
                playerReference.EquipWeapon<RifleWeapon>(weaponType, newStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADMachine);
                break;
        }

        GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;
        playerReference = other.gameObject.GetComponent<BasicPlayer>();
        

    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;
        

    }
}
