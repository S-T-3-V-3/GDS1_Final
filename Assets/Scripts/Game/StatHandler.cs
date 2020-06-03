using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatHandler
{
    public StatHandler(PawnStats baseStats, StatModifiers statModifiers) {
        this.baseStats = baseStats;
        this.modifiers = statModifiers;
        currentStats = baseStats;
    }

    [SerializeField] private PawnStats currentStats;
    [SerializeField] private PawnStats baseStats;
    [SerializeField] private StatModifiers modifiers;

    public UnityEvent OnHealthChanged = new UnityEvent();
    public UnityEvent OnEnergyChanged = new UnityEvent();


    //////////// Level UP ////////////
    public void LevelUp(StatType statType) {
        switch (statType) {
            case StatType.MAX_HP:
                MaxHealthLevel++;
                break;

            case StatType.HP_REGEN:
                HealthRegenLevel++;
                break;

            case StatType.DAMAGE:
                DamageLevel++;
                break;

            case StatType.AGILITY:
                AgilityLevel++;
                break;

            case StatType.ATTACK_SPEED:
                AttackSpeedLevel++;
                break;

            case StatType.CRIT_CHANCE:
                CritChanceLevel++;
                break; 
        }
    }

    //////////// Public Getters - Current Stat Levels ////////////
    public int MaxHealthLevel {
        get { return maxHealthLevel; }
        private set {
            currentStats.maxHealth = currentStats.maxHealth + modifiers.MaxHealth;
            maxHealthLevel++;
        }
    }
    [SerializeField] private int maxHealthLevel = 1;

    public int HealthRegenLevel {
        get { return healthRegenLevel; }
        private set {
            healthRegenLevel++;
            currentStats.healthRegen = baseStats.healthRegen + (healthRegenLevel * modifiers.HealthRegen);
        }
    }
    [SerializeField] private int healthRegenLevel = 1;

    public int DamageLevel {
        get { return damageLevel; }
        private set {
            damageLevel++;
            currentStats.damage = baseStats.damage + (modifiers.Damage * damageLevel - 1);
        }
    }
    [SerializeField] private int damageLevel = 1;

    public int AgilityLevel {
        get { return agilityLevel; }
        private set {
            agilityLevel++;
            currentStats.maxEnergy = baseStats.maxEnergy + (agilityLevel * modifiers.MaxEnergy);
            currentStats.sprintSpeed = baseStats.sprintSpeed + agilityLevel * modifiers.SprintSpeed;

            currentStats.sprintSpeed = baseStats.sprintSpeed;

            for (int i = 0; i < agilityLevel; i++)
                currentStats.sprintSpeed += modifiers.SprintSpeed * (agilityLevel - 1);
        }
    }
    [SerializeField] private int agilityLevel = 1;

    public int AttackSpeedLevel {
        get { return attackSpeedLevel; }
        private set {
            attackSpeedLevel++;
            currentStats.attackSpeed =  baseStats.attackSpeed + (attackSpeedLevel - 1) * (modifiers.AttackSpeed * baseStats.attackSpeed);
        }
    }
    [SerializeField] private int attackSpeedLevel = 1;

    public int CritChanceLevel {
        get { return critChanceLevel; }
        private set {
            critChanceLevel++;
            currentStats.critChance = baseStats.critChance + modifiers.CritChance * (critChanceLevel - 1);
        }
    }
    [SerializeField] private int critChanceLevel = 1;

    //////////// Public Getters - Current Stat Values ////////////
    public float CurrentHealth {
        get {
            return currentStats.health;
        }
        set {
            currentStats.health = value; // Used for adjusting current health

            if (currentStats.health > currentStats.maxHealth)
                currentStats.health = currentStats.maxHealth;

            OnHealthChanged.Invoke();
        }
    }

    public float MaxHealth {
        get {
            return currentStats.maxHealth;
        }
    }

    public float HealthRegen {
        get {
            return currentStats.healthRegen;
        }
    }

    public float MoveSpeed {
        get {
            return currentStats.moveSpeed;
        }
        set {
            currentStats.moveSpeed = value; // Used for toggling sprinting only
        }
    }

    public float Energy {
        get {
            return currentStats.energy;
        }
        set {
            currentStats.energy = value; // Used when consuming / regenerating energy

            if (currentStats.energy > currentStats.maxEnergy)
                currentStats.energy = currentStats.maxEnergy;

            OnEnergyChanged.Invoke();
        }
    }

    public float EnergyRegenSpeed {
        get {
            return currentStats.energyRegen;
        }
    }

    public float Damage {
        get {
            return currentStats.damage;
        }
    }

    public float MaxEnergy {
        get {
            return currentStats.maxEnergy;
        }
    }

    public float SprintSpeed {
        get {
            return currentStats.sprintSpeed;
        }
    }

    public float WalkSpeed {
        get {
            return baseStats.moveSpeed;
        }
    }

    public float AttackSpeed {
        get {
            return currentStats.attackSpeed;
        }
    }

    public float CritChance {
        get {
            return currentStats.critChance;
        }
    }

    public StatHandler GetCopy() {
        StatHandler clone = new StatHandler(baseStats, modifiers);
        return clone;
    }
}

//////////// Stat Enums ////////////
[System.Serializable]
public enum StatType {
    MAX_HP = 1,
    HP_REGEN = 2,
    DAMAGE = 3,
    AGILITY = 4,
    ATTACK_SPEED = 5,
    CRIT_CHANCE = 6
}

//////////// Level Calculations ////////////
[System.Serializable]
public class StatModifiers {
    [Header("Per Level Calculations")]

    [Header("Flat per level : Default 10")]
    public float MaxHealth = 10;

    [Header("Base stat multiplier : Default 0.5")]
    public float  HealthRegen = 0.5f;// = 0.5f

    [Header("Flat per level : Default 5")]
    public float Damage = 5f;// = 5f

    [Header("Flat per level : Default 10")]
    public float MaxEnergy = 10f;// = 10f

    [Header("Flat per level : Default 0.1")]
    public float SprintSpeed = 0.1f;// = 0.1f

    [Header("Base stat multiplier : Default 0.3")]
    public float AttackSpeed = 0.3f;// = 0.3f

    [Header("Flat per level : Default 5")]
    public float CritChance = 5f;// = 5f
}

//////////// Raw Stats Class ////////////
[System.Serializable]
public struct PawnStats {
    public float health;
    public float maxHealth;
    public float healthRegen;
    public float energy;
    public float maxEnergy;
    public float energyRegen;
    public float moveSpeed;
    public float sprintSpeed;
    public float attackSpeed;
    public float damage;
    public float critChance;
}