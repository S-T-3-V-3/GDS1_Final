using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    /* The player states in game manager will be used to
    * ensure that this code isn't running multiple times.
    * For the moment a bool is being used which will be 
    * removed later upon the implementation of player states
    */
    private bool isPlayerDead = false;
    GameManager gameManager;

    //Set in inspector window
    public Text finalScoreText;

    public void ShowDeathScreen()
    {
        if(!isPlayerDead)
        {
            isPlayerDead = true;
            gameObject.SetActive(true);
            gameManager = GameManager.Instance;
            finalScoreText.text = "Your Score: " + gameManager.scoreManager.currentScore;
        }
    }

    public void PlayAgainBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
