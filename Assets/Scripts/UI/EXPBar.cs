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

    float displayedCurrentXP = 0;

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
                levelText.text = scoreManager.playerLevel.ToString();
                experienceNumberText.text = $"{scoreManager.currentExperience}/{scoreManager.experienceToNextLevel}";
            }

            yield return null;
        }
    }

    void OnExpGain() {
        DOTween.Kill(experienceNumberText);
        DOTween.To(
            () => displayedCurrentXP,
            x => {
                displayedCurrentXP = x;
                experienceNumberText.text = $"{displayedCurrentXP}/{scoreManager.experienceToNextLevel}";
                foregroundImage.fillAmount = scoreManager.currentExperience / scoreManager.experienceToNextLevel;
            },
            scoreManager.currentExperience,
            1f
        ).SetOptions(true).SetUpdate(UpdateType.Fixed);
    }

    void OnLevelUp() {
        levelText.text = scoreManager.playerLevel.ToString();
        experienceNumberText.text = $"0/{scoreManager.experienceToNextLevel}";
        foregroundImage.fillAmount = 0f;
        displayedCurrentXP = 0;
    }
}
