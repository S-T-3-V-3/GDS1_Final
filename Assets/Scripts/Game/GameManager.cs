using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [Header("Settings")]
    public GameSettings gameSettings;

    private void Awake()
    {
        if(Instance != null)
            GameObject.Destroy(this.gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject.Instantiate(gameSettings.playerSettings.playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
    }
    
}
