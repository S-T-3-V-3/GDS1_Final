using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public UnityEvent OnExperienceGained;
    public UnityEvent OnLevelUp;

    public int currentScore = 0;
    
    public int playerLevel = 1;
    public float baseLevelUpExperience = 200;
    public float experienceToNextLevel;
    public float currentExperience;

    BasicPlayer playerRef;

    void Start()
    {
        StartCoroutine(Initialize());

        if (OnExperienceGained == null)
            OnExperienceGained = new UnityEvent();

        if (OnLevelUp == null)
            OnLevelUp = new UnityEvent();

        experienceToNextLevel = baseLevelUpExperience;
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
            {
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();
                yield return new WaitForEndOfFrame();

                GameManager.Instance.OnAddScore.AddListener(AddScore);
            }

            yield return null;
        }
    }

    public void AddScore(int value, Vector3 pos)
    {
        currentScore += value;
        
        currentExperience += value;
        OnExperienceGained.Invoke();

        while (currentExperience >= experienceToNextLevel) {
            currentExperience -= experienceToNextLevel;
            playerLevel++;
            OnLevelUp.Invoke();
            OnExperienceGained.Invoke();
        }
    }
}
