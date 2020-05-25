using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatHandler
{
    public StatHandler(BaseStats baseStats, StatModifiers statModifiers) {
        this.currentStats = baseStats;
        this.statModifiers = statModifiers;
    }

    [Header("Stat Levels")]
    public int MaxHealthLevel = 1;
    public int HealthRegenLevel = 1;
    public int DamageLevel = 1;
    public int AgilityLevel = 1;
    public int AttackSpeedLevel = 1;
    public int CritChanceLevel = 1;

    [Header("Base Stats")]
    [SerializeField] private BaseStats currentStats;

    [Space]
    [SerializeField] private StatModifiers statModifiers;

    [Header("Events")]
    public UnityEvent OnHealthChanged = new UnityEvent();

    public void LevelUp(StatType statType) {
        switch (statType) {
            case StatType.AGILITY:
            AgilityLevel++;
            break;

            case StatType.ATTACK_SPEED:
            AttackSpeedLevel++;
            break;

            case StatType.CRIT_CHANCE:
            CritChanceLevel++;
            break;

            case StatType.DAMAGE:
            DamageLevel++;
            break;

            case StatType.HP_REGEN:
            HealthRegenLevel++;
            break;

            case StatType.MAX_HP:
            float maxhp = MaxHealth;
            MaxHealthLevel++;
            float hpDiff = Mathf.Abs(maxhp - MaxHealth);
            CurrentHealth += hpDiff;
            break;
        }
    }
    public float MaxHealth {
        get {
            return currentStats.maxHealth + ((MaxHealthLevel - 1) * statModifiers.ABMaxHealth);
        }
    }

    public float CurrentHealth {
        get {
            return currentStats.currentHealth;
        }
        set {
            currentStats.currentHealth = value;

            if (currentStats.currentHealth > MaxHealth)
                currentStats.currentHealth = MaxHealth;

            OnHealthChanged.Invoke();
        }
    }

    public float MoveSpeed {
        get {
            return currentStats.moveSpeed;
        }
    }

    public float CurrentStamina {
        get {
            return currentStats.currentStamina;
        }
        set {
            currentStats.currentStamina = value;
        }
    }

    public float StaminaRegenSpeed {
        get {
            return currentStats.staminaRegenSpeed;
        }
    }

    public float HealthRegen {
        get {
            return currentStats.staminaRegenSpeed + (HealthRegenLevel * statModifiers.MBHealthRegen);
        }
    }

    public float Damage {
        get {
            return currentStats.damage + statModifiers.ADamage * DamageLevel;
        }
    }

    public AgilityType Agility {
        get {
            AgilityType newAgility;
            newAgility.maxStamina = currentStats.maxStamina + (AgilityLevel * statModifiers.ABMaxStamina);
            newAgility.sprintSpeed = currentStats.sprintSpeed;

            for (int i = 0; i < AgilityLevel; i++)
                newAgility.sprintSpeed += newAgility.sprintSpeed * statModifiers.MCSprintSpeed * (AgilityLevel - 1);

            return newAgility;
        }
    }

    public float AttackSpeed {
        get {
            return currentStats.attackSpeed + (AttackSpeedLevel - 1) * (statModifiers.MBAttackSpeed * currentStats.attackSpeed);
        }
    }

    public float CritChance {
        get {
            return currentStats.critChance + statModifiers.ACritChance * (CritChanceLevel - 1);
        }
    }

    public StatHandler GetCopy() {
        StatHandler clone = new StatHandler(currentStats, statModifiers);
        return clone;
    }
}

public struct AgilityType {
    public float maxStamina;
    public float sprintSpeed;
}

[System.Serializable]
public enum StatType {
    MAX_HP = 1,
    HP_REGEN = 2,
    DAMAGE = 3,
    AGILITY = 4,
    ATTACK_SPEED = 5,
    CRIT_CHANCE = 6
}

[System.Serializable]
public class StatModifiers {
    // A = Additive stat
    // M = Multiplicitive
    // B = Base
    // C = Current
    [Header("A = Additive, M = Multiplicitive, B = Base, C = Current")]
    [Header("Per Level Calculations")]
    public float ABMaxHealth = 10;// = 10f
    public float  MBHealthRegen = 0.5f;// = 0.5f
    public float ADamage = 5f;// = 5f
    public float ABMaxStamina = 10f;// = 10f
    public float MCSprintSpeed = 0.1f;// = 0.1f
    public float MBAttackSpeed = 0.3f;// = 0.3f
    public float ACritChance = 0.05f;// = 0.05f
}