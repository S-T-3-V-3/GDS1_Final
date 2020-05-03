using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Weapon Type")]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public GameObject WeaponPrefab;
    public WeaponStats Stats;
}

[System.Serializable]
public class WeaponStats {
    public FireType Type;
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