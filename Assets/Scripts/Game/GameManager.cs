using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Prefabs")]
    public GameObject TileManagerPrefab;

    [Header("Settings")]
    public GameSettings gameSettings;

    [Header("Scene Objects")]
    public TileManager tileManager;
    [HideInInspector] public PlayerController playerController;

    private void Awake()
    {
        if(Instance != null)
            GameObject.Destroy(this.gameObject);
        else
            Instance = this;

        tileManager = GameObject.Instantiate(TileManagerPrefab).GetComponent<TileManager>();
    }

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        playerController = GameObject.Instantiate(gameSettings.playerSettings.playerPrefab, new Vector3(0, 5, 0), Quaternion.identity).GetComponent<PlayerController>();
    }
    
}
