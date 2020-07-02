using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SessionData : MonoBehaviour
{
    public bool isPaused = false;
    public float currentScore = 0;

    public void TogglePause() {
        isPaused = !isPaused;

        List<GameObject> pausables = FindObjectsOfType<GameObject>().Where(x => x.GetComponent<IPausable>() != null).ToList();
        foreach (GameObject p in pausables) {
            if (isPaused)
                p.GetComponent<IPausable>().Pause();
            else
                p.GetComponent<IPausable>().UnPause();
        }
    }
}
