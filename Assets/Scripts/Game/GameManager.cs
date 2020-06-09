using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Prefabs")]
    public GameObject TileManagerPrefab;
    public GameObject CameraPrefab;
    public GameObject GameOverUIPrefab;
    public GameObject ProjectilePrefab;
    public GameObject HUDPrefab;
    public GameObject PausePrefab;
    public GameObject ControlsUIPrefab;

    [Header("Environment Materials")]
    public List<Material> rockMaterials;
    public List<Material> groundMaterials;

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
    public GameObject PauseHUD;
    public GameObject ControlsHUD;
    public ScoreEvent OnAddScore;

    public static bool GameIsPaused = false;

    private void Awake()
    {
        if(Instance != null)
            GameObject.Destroy(this.gameObject);
        else
            Instance = this;

        audioManager = Instantiate(gameSettings.audioManager).GetComponent<AudioManager>();
        scoreManager = this.gameObject.AddComponent<ScoreManager>();

        tileManager = GameObject.Instantiate(TileManagerPrefab).GetComponent<TileManager>();
        mainCamera = GameObject.Instantiate(CameraPrefab, this.transform).GetComponent<Camera>();
        gameOverUI = GameObject.Instantiate(GameOverUIPrefab, this.transform).GetComponent<GameOverUI>();
        
        GameObject hudObject = GameObject.Instantiate(HUDPrefab);
        hud = hudObject.GetComponent<HUD>();
        PauseHUD = Instantiate(PausePrefab, this.transform);
        ControlsHUD = Instantiate(ControlsUIPrefab, this.transform);
    }

    void Start()
    {
        SpawnPlayer();
        GameOverUIPrefab.SetActive(false);
        PauseHUD.SetActive(false);
        ControlsHUD.SetActive(false);
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

    public void OnPauseButton()
    {
        if(ControlsHUD.activeSelf)
        {
            PauseHUD.SetActive(true);
            ControlsHUD.SetActive(false);
        }
        else if(GameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Resume()
    {
        PauseHUD.SetActive(false);
        //Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseHUD.SetActive(true);
        //Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadControls()
    {
        PauseHUD.SetActive(false);
        ControlsHUD.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Application Closes on Build Version");
        Application.Quit();
    }
}

[System.Serializable]
public class ScoreEvent : UnityEvent<int, Vector3> {}
[System.Serializable]
public class StatIncreaseEvent : UnityEvent<StatType> {}