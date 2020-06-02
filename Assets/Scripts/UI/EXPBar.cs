using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class EXPBar : MonoBehaviour
{
    //Level and Exp
    public int playerLevel = 1;
    private float baseLevelUpExperience = 200;
    private float experienceToNextLevel;
    private int currentExperience;

    //UI
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI experienceNumberText;

    //Exp Tweener
    int previousExperienceTarget;
    Tween experienceTween;
    [SerializeField]
    private Image foregroundImage;

    //Other Script References
    public UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        experienceToNextLevel = baseLevelUpExperience;
        levelText.text = playerLevel.ToString();
        experienceNumberText.text = $"{currentExperience}/{experienceToNextLevel}";

        //This is just for tween initialization since I couldn't figure it out
        experienceTween = DOTween.To(()=> currentExperience, x=> currentExperience = x, 0, 1);
    }

    public void AddExperience(int value)
    {     
        int targetExperience = currentExperience + value;

        if(experienceTween.IsActive())
        {
            experienceTween.Kill();
            targetExperience = previousExperienceTarget + value;
        }

        experienceTween = DOTween.To(()=> currentExperience, x=> currentExperience = x, targetExperience, 1);
        previousExperienceTarget = targetExperience;
    }

    private void Update()
    {
        //Runs every frame the tweener is alive
        if(experienceTween.IsActive())
        {
            experienceNumberText.text = $"{currentExperience}/{experienceToNextLevel}";
            foregroundImage.fillAmount = (float)currentExperience / (float)experienceToNextLevel;

            if (currentExperience/experienceToNextLevel >= 1)
            {
                LevelUp();
            }
        }
    }

    void LevelUp()
    {
        experienceTween.Kill();

        playerLevel++;
        upgradeManager.skillPoints++;
        upgradeManager.ShowUpgradeWindow();
        levelText.text = playerLevel.ToString();

        //Carry over overflowing previous exp points
        int experienceFromPreviousLevel = (int)(previousExperienceTarget - experienceToNextLevel);

        experienceToNextLevel = experienceToNextLevel + baseLevelUpExperience / 2;
        currentExperience = 0;
        foregroundImage.fillAmount = 0;

        AddExperience(experienceFromPreviousLevel);
    }
}
