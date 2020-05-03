using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public float rotationSpeed; // This probably shouldn't exist, it would feel bad if it took time to rotate towards cursor or stick position
    public PlayerStats baseStats;
}

[System.Serializable]
public class PlayerStats {
    public float maxHealth;
    public float currentHealth;
    public float healthRegenSpeed;
    [Range(0.1f, 3f)]
    public float moveSpeed;
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