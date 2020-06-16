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
    public float critDamage; //UNUSED
    public float range;
    public float shotSpeed;
    public float attackSpeed;
    public float accuracy; //UNUSED
    public bool hasPiercingShots; //UNUSED
    public int numBullets;
    public float chargeTime; //UNUSED
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

