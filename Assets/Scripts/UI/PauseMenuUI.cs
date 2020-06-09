using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void SetVolume(float volume)
    {
        GameManager.Instance.audioManager.SetVolume(volume);
    }

    public void Resume()
    {
        GameManager.Instance.OnPauseButton();
    }

    public void LoadControls()
    {
        GameManager.Instance.LoadControls();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
