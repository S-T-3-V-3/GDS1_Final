using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemyType : ScriptableObject
{
    public static string EnemyName;
    public static GameObject EnemyPrefab;
    public EnemyStats enemyStats;
}

public class EnemyStats {
    public float health;
    public float moveSpeed;
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
    public WeaponType equippedWeapon;
    public float fireRate;
    public float damage;
}