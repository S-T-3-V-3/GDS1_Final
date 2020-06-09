using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Ability Type")]

public class AbilitySettings : ScriptableObject
{
    public AbilityType abilityType;
    [Space]
    public AbilityStats abilityStats;

}

[System.Serializable]
public struct AbilityStats
{
    public float time;
    public int multiplier;
    public float dashDistance;
}


public enum AbilityType
{
    DASH,
    RAPIDHEAL,
    RAPIDFIRE,
    INVISIBILITY
}
