using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemySettings : ScriptableObject
{
    public EnemyType enemyType;
    public ObjectStats stats;
    public WeaponType weaponType;

    [Space]
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
    public float wanderDistance;
    public float wanderUpdateFrequency;
    public float wanderStuckDistance;
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