using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject playerPrefab;

    [Header("Values")]
    public float movementSpeed;
    [Range(0.1f, 3f)]
    public float rotationSpeed;
}
