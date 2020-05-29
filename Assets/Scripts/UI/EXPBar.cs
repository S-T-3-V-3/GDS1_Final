using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXPBar : MonoBehaviour
{
    //Level and Exp
    public int playerLevel = 1;
    private float baseLevelUpExperience = 200;
    private float experienceToNextLevel;
    private float currentExp;

    //UI
    public TextMeshProUGUI levelText;

    //Coroutine for exp bar lerping
    Coroutine currentCoroutine;
    float timeSinceModified = 0;
    [SerializeField]
    private Image foregroundImage;
    public float sensitivity = 0.01f;
    public float lerpSpeed = 1f; // Seconds

    //Other Script References
    public UpgradeManager upgradeManager;

    // Start is called before the first frame update
    void Start()
    {
        experienceToNextLevel = baseLevelUpExperience;
        levelText.text = playerLevel.ToString();
    }

    public void AddExperience(int value)
    {   
        currentExp += value;

        float targetExperiencePercent = currentExp / experienceToNextLevel;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = StartCoroutine(UpdateImage(targetExperiencePercent));
        
    }

    IEnumerator UpdateImage(float targetExperiencePercent)
    {
        float currentExperiencePercent = foregroundImage.fillAmount;
        float startExperiencePercent = currentExperiencePercent;
        timeSinceModified = 0f;

        //Lerp to the new health percent
        while (Mathf.Abs(currentExperiencePercent - targetExperiencePercent) > sensitivity)
        {
            yield return new WaitForEndOfFrame();

            timeSinceModified += Time.deltaTime;
            currentExperiencePercent = Mathf.Lerp(startExperiencePercent, targetExperiencePercent, timeSinceModified / lerpSpeed);

            //Break if a level up is detected
            if (currentExperiencePercent >= 1)
            {
                LevelUp();
                yield break;
            }
            
            foregroundImage.fillAmount = currentExperiencePercent;
        }

        //Set image to the new health amount
        foregroundImage.fillAmount = targetExperiencePercent;
        //Level up if cap is hit
        if (targetExperiencePercent >= 1)
            LevelUp();
    }

    void LevelUp()
    {
        playerLevel++;
        upgradeManager.skillPoints++;
        levelText.text = playerLevel.ToString();

        //Carry over overflowing previous exp points
        int newExperienceAmount = (int)(currentExp - experienceToNextLevel);

        experienceToNextLevel = experienceToNextLevel + baseLevelUpExperience / 2;
        currentExp = 0;
        foregroundImage.fillAmount = 0;
        AddExperience(newExperienceAmount);
    }
}
