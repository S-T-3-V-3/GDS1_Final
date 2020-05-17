using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Prefabs")]
    public GameObject TileManagerPrefab;
    public GameObject CameraPrefab;
    public GameObject GameOverUIPrefab;
    public GameObject ProjectilePrefab;

    [Header("Settings")]
    public GameSettings gameSettings;

    [Header("Scene Objects")]
    public TileManager tileManager;
    public Camera mainCamera;
    public PlayerController playerController;
    public GameOverUI gameOverUI;

    private void Awake()
    {
        if(Instance != null)
            GameObject.Destroy(this.gameObject);
        else
            Instance = this;

        tileManager = GameObject.Instantiate(TileManagerPrefab).GetComponent<TileManager>();

        mainCamera = GameObject.Instantiate(CameraPrefab, this.transform).GetComponent<Camera>();

        gameOverUI = GameObject.Instantiate(GameOverUIPrefab, this.transform).GetComponent<GameOverUI>();
        
    }

    void Start()
    {
        SpawnPlayer();
        GameOverUIPrefab.SetActive(false);
    }

    void SpawnPlayer()
    {
        playerController = GameObject.Instantiate(gameSettings.playerSettings.playerPrefab, new Vector3(0, 2, 0), Quaternion.identity).GetComponent<PlayerController>();
    }

    public void GameOver(float deathScreenDelay)
    {
        playerController = null;
        gameOverUI.Invoke("ShowDeathScreen", deathScreenDelay);
        //Set player to a dead state
    }
    
}
