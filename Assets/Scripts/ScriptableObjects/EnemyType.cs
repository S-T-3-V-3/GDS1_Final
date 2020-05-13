using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemyType : ScriptableObject
{
    public string EnemyName;
    public GameObject EnemyPrefab;
    public EnemyStats enemyStats;
}

[System.Serializable]
public class EnemyStats {
    public EnemyType enemyType;
    public float health;
    public float moveSpeed;
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
    public WeaponType equippedWeapon;
    public float fireRate;
    public float damage;
}