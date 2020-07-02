using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemySettings : ScriptableObject
{
    public EnemyType enemyType;
    public StatHandler statHandler;
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
    public Vector2 wanderUpdateFrequency;
    public Material material;
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
    public float wanderDistance;    
    public float wanderStuckDistance;
    public int enemyScore;
}