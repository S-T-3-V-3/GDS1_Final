using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public float rotationSpeed = 2; // This probably shouldn't exist, it would feel bad if it took time to rotate towards cursor or stick position
    public PlayerStats baseStats;

    [Header("Values")]
    public float movementDelta = 0.04f;
}

[System.Serializable]
public class PlayerStats {
    public float maxHealth;
    public float currentHealth;
    public float healthRegenSpeed;
    //[Range(6, 20f)]
    public float moveSpeed = 30;
    public float damage;
    public float fireRate;
    public float critChance;
    public float maxMana;
    public float currentMana;
    public float manaRegenSpeed;
    
}

[System.Serializable]
// Used to track # skill points allocated to each stat
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