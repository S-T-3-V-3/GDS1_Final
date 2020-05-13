using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Enemy Type")]
public class EnemyType : ScriptableObject
{
    public GameObject EnemyPrefab;
    public ObjectStats objectStats;
    public EnemyStats enemyStats;
}

[System.Serializable]
public struct EnemyStats {
    public string enemyName;
    public float turnSpeed;
    public float detectionRange;
    public float perceptiveness;
}