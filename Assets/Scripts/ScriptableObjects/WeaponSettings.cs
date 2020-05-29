using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Definition")]
public class WeaponDefinition : ScriptableObject
{
    public GameObject WeaponPrefab;
    public WeaponType weaponType;
    public WeaponStats weaponBaseStats;
}

[System.Serializable]
public struct WeaponStats {
    public float weaponDamage;
    public float critDamage;
    public float range;
    public float spread;
    public float shotSpeed;
    public float fireRate;
    public float accuracy;
    public bool hasPiercingShots;
    public int numBullets;
    public int chargeTime;
}

public enum WeaponType {
    MELEE,
    RIFLE,
    MACHINE_GUN,
    SHOTGUN,
    SNIPER,
    RPG,
    BURST_FIRE,
    CHARGE,
    LASER
}

