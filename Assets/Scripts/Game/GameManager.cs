using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Prefabs")]
    public GameObject TileManagerPrefab;
    public GameObject CameraPrefab;
    public GameObject GameOverUIPrefab;
    public GameObject ProjectilePrefab;
    public GameObject HUDPrefab;
    public GameObject BeamRayPrefab;
    public GameObject ShotgunParticlePrefab;

    [Header("Settings")]
    public GameSettings gameSettings;

    [Header("Scene Objects")]
    public TileManager tileManager;
    public Camera mainCamera;
    public AudioManager audioManager;
    public PlayerController playerController;
    public GameOverUI gameOverUI;
    public ScoreManager scoreManager;
    public HUD hud;
    public ScoreEvent OnAddScore;
    public float playerScore = 0.0f;

    private void Awake()
    {
        if(Instance != null)
            GameObject.Destroy(this.gameObject);
        else
            Instance = this;

        audioManager = Instantiate(gameSettings.audioManager).GetComponent<AudioManager>();
        tileManager = GameObject.Instantiate(TileManagerPrefab).GetComponent<TileManager>();
        mainCamera = GameObject.Instantiate(CameraPrefab, this.transform).GetComponent<Camera>();
        gameOverUI = GameObject.Instantiate(GameOverUIPrefab, this.transform).GetComponent<GameOverUI>();
        GameObject hudObject = GameObject.Instantiate(HUDPrefab);
        hud = hudObject.GetComponent<HUD>();
        scoreManager = hudObject.GetComponentInChildren<ScoreManager>();
    }

    void Start()
    {
        SpawnPlayer();
        GameOverUIPrefab.SetActive(false);
        OnAddScore.Invoke(0, Vector3.zero);
    }

    void SpawnPlayer()
    {   
        playerController = GameObject.Instantiate(gameSettings.playerSettings.playerPrefab, tileManager.rootTile.startLocation.position, Quaternion.identity).GetComponent<PlayerController>();
    }

    public void GameOver(float deathScreenDelay)
    {
        playerController = null;
        gameOverUI.Invoke("ShowDeathScreen", deathScreenDelay);
        //Set player to a dead state
    }
}

[System.Serializable]
public class ScoreEvent : UnityEvent<int, Vector3> {}
[System.Serializable]
public class StatIncreaseEvent : UnityEvent<StatType> {}