using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponStatsUI : MonoBehaviour
{
    public DroppedWeapon droppedWeapon;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI shotSpeedText;
    public TextMeshProUGUI attackSpeedText;

    public void Init(WeaponType weaponType, WeaponStats weaponStats, bool setColors = false) {
        switch (weaponType) {
            case WeaponType.RIFLE:
                weaponText.text = "Rifle";
                break;
            case WeaponType.SHOTGUN:
                weaponText.text = "Shotgun";
                break;
            case WeaponType.MACHINE_GUN:
                weaponText.text = "Machine Gun";
                break;
            case WeaponType.SNIPER:
                weaponText.text = "Sniper";
                break;
        }

        if (setColors) {
            if (weaponStats.range < GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.range)
                rangeText.color = Color.red;
            else if (weaponStats.range > GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.range)
                rangeText.color = Color.green;
            
            if (weaponStats.weaponDamage < GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.weaponDamage)
                damageText.color = Color.red;
            else if (weaponStats.weaponDamage > GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.weaponDamage)
                damageText.color = Color.green;

            if (weaponStats.shotSpeed < GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.shotSpeed)
                shotSpeedText.color = Color.red;
            else if (weaponStats.shotSpeed > GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.shotSpeed)
                shotSpeedText.color = Color.green;

            if (weaponStats.attackSpeed < GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.attackSpeed)
                attackSpeedText.color = Color.red;
            else if (weaponStats.attackSpeed > GameManager.Instance.playerController.GetComponent<BasicPlayer>().equippedWeapon.weaponStats.attackSpeed)
                attackSpeedText.color = Color.green;
        }

        rangeText.text = $"{(int)weaponStats.range}";
        damageText.text = $"{(int)weaponStats.weaponDamage}";
        shotSpeedText.text = $"{(int)weaponStats.shotSpeed}";
        attackSpeedText.text = $"{Mathf.Round(weaponStats.attackSpeed * 100f) / 100f}";
    }
}
