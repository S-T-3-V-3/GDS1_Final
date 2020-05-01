using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Settings")]
    public GameSettings gameSettings;

    private void Awake()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayer() //Only needed when  spawning from gamemanager
    {
        Instantiate(gameSettings.playerSettings.playerPrefab, new Vector3(0, 5, 0), Quaternion.identity);
    }
    
}
