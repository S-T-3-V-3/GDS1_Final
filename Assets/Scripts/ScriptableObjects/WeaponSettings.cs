using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Type")]
public class WeaponSettings : ScriptableObject
{
    public WeaponType weaponType;
    public FireType fireType;
    [Space]

    public GameObject WeaponPrefab;
    public WeaponStats stats;
}

[System.Serializable]
public struct WeaponStats {
    public float weaponDamage;
    public float critDamage;
    public float range;
    public float shotSpeed;
    public float fireRate;
    public float accuracy;
    public bool hasPiercingShots;
    public int numBullets;
    public int chargeTime;
}

// Should be used for switching in a new component to handle firing when equipped
public enum FireType {
    SINGLE,
    SPREAD,
    BURST,
    BEAM
}

public enum WeaponType {
    RIFLE
}