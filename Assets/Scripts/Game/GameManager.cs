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
        // Really wasn't a fan of the laser / no cursor system
        // Not knowing the distance of your cursor from the player makes turning feel really really bad
        // Laser with visible cursor might be ok, but the laser asset should be much more polished before implemented imo - Steve
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        playerController = GameObject.Instantiate(gameSettings.playerSettings.playerPrefab, new Vector3(0, 2, 0), Quaternion.identity).GetComponent<PlayerController>();
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