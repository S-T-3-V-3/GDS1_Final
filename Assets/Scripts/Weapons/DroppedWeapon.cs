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
    public WeaponStatsUI stats;

    public WeaponStats weaponStats;
    public bool dropped = false;

    GameObject weaponModel;

    public void Init(GameObject weaponModel) {
        if (!dropped) {
            weaponStats = GameManager.Instance.gameSettings.WeaponList.Where(x => x.weaponType == weaponType).First().weaponBaseStats;
            DelayedAction d = this.gameObject.AddComponent<DelayedAction>();
            d.maxDelayTime = 25f;
        }
        else {
            DelayedAction d = this.gameObject.AddComponent<DelayedAction>();
            d.maxDelayTime = 60f;
        }

        if (!dropped) {
            for (int i = 0; i < GameManager.Instance.scoreManager.playerLevel; i++) {
                int randStat = Random.Range(0,3);
                switch (randStat) {
                    case 0:
                        weaponStats.attackSpeed *= 1.1f;
                        break;
                    case 1:
                        weaponStats.weaponDamage *= 1.1f;
                        break;
                    case 2:
                        weaponStats.range *= 1.1f;
                        break;
                    case 3:
                        weaponStats.shotSpeed *= 1.1f;
                        break;
                }
            }
        }  

        Material glowMat;
        weaponModel = Instantiate(weaponModel, modelDisplayPoint.position, Quaternion.identity);
        weaponModel.transform.parent = this.transform;
        weaponModel.transform.DOLocalRotate(new Vector3(0, 1280, 0), 15f, RotateMode.FastBeyond360);
        WeaponTransforms weapons = weaponModel.GetComponent<WeaponTransforms>();

        glowMat = GameManager.Instance.gameSettings.yellowGlowMaterial;

        foreach (MeshRenderer rend in indicatorRenderers) {
            rend.material = glowMat;
        }

        weapons.model.GetComponent<MeshRenderer>().material = glowMat;
    }

    public void EquipSelected(){
        switch (weaponType)
        {
            case WeaponType.RIFLE:
                playerReference.EquipWeapon<RifleWeapon>(weaponType, weaponStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADRifle);
                break;
            case WeaponType.SHOTGUN:
                playerReference.EquipWeapon<ShotgunWeapon>(weaponType, weaponStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADShotgun);
                break;
            case WeaponType.MACHINE_GUN:
                playerReference.EquipWeapon<RifleWeapon>(weaponType, weaponStats);
                AudioManager.Instance.PlaySoundEffect(SoundType.LOADMachine);
                break;
        }

        GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;

        playerReference = other.gameObject.GetComponent<BasicPlayer>();

        if (playerReference.nearbyWeapon != null && playerReference.nearbyWeapon != this)
            playerReference.nearbyWeapon.Hide();

        playerReference.nearbyWeapon = this;

        Show();
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<BasicPlayer>() == null) return;
        
        if (playerReference.nearbyWeapon == this)
            playerReference.nearbyWeapon = null;

        Hide();
    }

    void Show() {
        stats.gameObject.SetActive(true);
        stats.droppedWeapon = this;
        stats.Init(weaponType,weaponStats,true);
    }

    public void Hide() {
        stats.gameObject.SetActive(false);
    }

    private void Update() {
        if (GameManager.Instance.playerController == null || GameManager.Instance.playerController.transform == null) return;
        stats.gameObject.transform.LookAt(GameManager.Instance.mainCamera.transform, Vector3.up);
    }
}
