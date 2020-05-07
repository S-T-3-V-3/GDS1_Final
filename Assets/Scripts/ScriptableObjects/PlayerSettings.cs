using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public PlayerStats baseStats;
}

[System.Serializable]
public class PlayerStats {
    public float maxHealth;
    public float currentHealth;
    public float healthRegenSpeed;
    public float moveSpeed = 30;
    public float damage;
    public float fireRate;
    public float critChance;
    public float maxMana;
    public float currentMana;
    public float manaRegenSpeed;
    
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