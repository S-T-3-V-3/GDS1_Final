using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    /* The player states in game manager will be used to
    * ensure that this code isn't running multiple times.
    * For the moment a bool is being used which will be 
    * removed later upon the implementation of player states
    */
    private bool isPlayerDead = false;

    public void ShowDeathScreen()
    {
        if(!isPlayerDead)
        {
            isPlayerDead = true;
            gameObject.SetActive(true);
            Debug.Log("Player is Dead");
        }
    }

    public void PlayAgainBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
