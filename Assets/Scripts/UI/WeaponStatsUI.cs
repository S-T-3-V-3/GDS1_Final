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

    public void Init(WeaponType weaponType, WeaponStats weaponStats) {
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

        rangeText.text = $"{weaponStats.range}";
        damageText.text = $"{weaponStats.weaponDamage}";
        shotSpeedText.text = $"{weaponStats.shotSpeed}";
        attackSpeedText.text = $"{weaponStats.attackSpeed}";
    }
}
