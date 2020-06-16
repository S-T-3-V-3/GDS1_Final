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
    public float gravity = -9.8f;

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