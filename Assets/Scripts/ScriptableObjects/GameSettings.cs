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
    public WorldTiles tiles;
    public List<EnemySettings> Enemies;
    public List<WeaponDefinition> WeaponList;

    [Header("Game Effects")]
    public GameObject debrySparkEffect;
    public GameObject experienceOrbEffect;
    public GameObject muzzleFlashEffect;
    public GameObject shockwaveEffect;
}