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
<<<<<<< HEAD
    public List<WeaponSettings> Weapons;
    public List<AbilitySettings> Abilities;
=======
    public List<WeaponDefinition> WeaponList;
    public float gravity = -9.8f;
>>>>>>> 2c0e89c10ed7f0ee5d814b63e22b9b80f2b6e687

    [Header("Game Effects")]
    public GameObject DebrisSparkPrefab;
    public GameObject ExperienceOrbPrefab;
    public GameObject MuzzleFlashPrefab;
    public GameObject ShockwavePrefab;
    public GameObject DirectionalExplosionPrefab;
    public GameObject ShotgunParticlePrefab;
    public GameObject BeamRayEffectPrefab;

    [Header("Game Indicators")]
    public GameObject dropIndicator;
    public Material blueGlowMaterial;
    public Material yellowGlowMaterial;
}