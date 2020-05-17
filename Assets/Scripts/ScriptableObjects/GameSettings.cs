using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Settings")]
    public PlayerSettings playerSettings;
    public CameraSettings cameraSettings;
    public WorldTiles worldTiles;
    public List<EnemySettings> Enemies;
    public List<WeaponSettings> Weapons;
}