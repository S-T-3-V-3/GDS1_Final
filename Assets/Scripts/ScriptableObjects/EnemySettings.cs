using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemySettings : ScriptableObject
{
    public EnemyType enemyType;
    public ObjectStats stats;
    public EnemyTraits traits;
    public WeaponType weaponType;
}

public enum EnemyType {
    GUNNER,
    RUSHER,
    HEAVY_GUNNER,
    TACTICAL_GUNNER,
    BOOMER,
    SNIPER,
    SHOTGUNNER
}

[System.Serializable]
public struct EnemyTraits {
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
    public float wanderDistance;
    public Vector2 wanderUpdateFrequency;
    public float wanderStuckDistance;
    public bool canShootAndSeek;
    public int enemyScore;
}