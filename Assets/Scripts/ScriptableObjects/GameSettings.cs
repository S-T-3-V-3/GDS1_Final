using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public PlayerSettings playerSettings;
    public CameraSettings cameraSettings;
    public AudioSettings audioSettings;
    public AudioManager audioManager;
    public WorldTiles worldTiles;
    public List<EnemySettings> Enemies;
    public List<WeaponSettings> Weapons;
    public List<AbilitySettings> Abilities;

    [Header("UI Settings")]
    public GameObject aimPointUI;
}