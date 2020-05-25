using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public ObjectStats baseStats;

    [Header("Layer Masking")]
    public LayerMask projectileMask;
}

// Used to track # skill points allocated to each stat
[System.Serializable]
public class PlayerSkillPoints {
    public int maxHealth;
    public int healthRegenSpeed;
    public int moveSpeed;
    public int damage;
    public int fireRate;
    public int critChance;
    public int maxMana;
    public int manaRegenSpeed;
}