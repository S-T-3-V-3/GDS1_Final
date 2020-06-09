using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EXPBar : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI experienceNumberText;
    public Image foregroundImage;

    BasicPlayer playerRef;
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize() {
        while (scoreManager == null) {

            if (GameManager.Instance.scoreManager != null)
            {
                scoreManager = GameManager.Instance.scoreManager;;
                yield return null;

                scoreManager.OnExperienceGained.AddListener(OnExpGain);
                scoreManager.OnLevelUp.AddListener(OnLevelUp);
            }

            yield return null;
        }
    }

    void OnExpGain() {
        experienceNumberText.text = $"{scoreManager.currentExperience}/{scoreManager.experienceToNextLevel}";
    }

    void OnLevelUp() {
        levelText.text = scoreManager.playerLevel.ToString();
    }
}
