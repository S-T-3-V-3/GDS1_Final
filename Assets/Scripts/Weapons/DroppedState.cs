using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroppedState : MonoBehaviour
{
    public BasicPlayer playerReference;
    public Transform modelDisplayPoint;
    public WeaponType weaponType;
    public MeshRenderer[] indicatorRenderers;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, 10);
    }

    public void Init(GameObject weaponModel, string originName){
        Material glowMat;
        GameObject model = Instantiate(weaponModel, modelDisplayPoint.position, Quaternion.identity);
        //weaponModel.transform.parent = modelDisplayPoint;
        WeaponTransforms weapons = model.GetComponent<WeaponTransforms>();
        //ndicatorRenderers.Append(weapons.model.GetComponent<MeshRenderer>());

        if(originName.Equals("Player"))
            glowMat = GameManager.Instance.gameSettings.blueGlowMaterial;
        else 
            glowMat = GameManager.Instance.gameSettings.yellowGlowMaterial;

        foreach (MeshRenderer rend in indicatorRenderers) {
            rend.material = glowMat;
        }

        weapons.model.GetComponent<MeshRenderer>().material = glowMat;
        GameObject.Destroy(model, 10);
    }

    public void EquipSelected(){
        if(playerReference.equippedWeapon.weaponType == weaponType)
        {
            GameObject.Destroy(gameObject);
            return;
        }

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
        if (playerReference == null) playerReference = other.gameObject.GetComponent<BasicPlayer>();
        
        WeaponPanel panel = GameManager.Instance.hud.weaponPanel;
        panel.gameObject.SetActive(true);
        panel.PanelSetup(weaponType, gameObject);

        /*BasicPlayer player = other.gameObject.GetComponent<BasicPlayer>();
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

        GameObject.Destroy(this.gameObject);*/
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;
        
        WeaponPanel panel = GameManager.Instance.hud.weaponPanel;
        panel.gameObject.SetActive(false);
    }
}
