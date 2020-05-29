﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: TEXT MESH PRO REFACTOR
public class ScoreManager : MonoBehaviour
{
    public Button MaxHP;
    public Button HPRegen;
    public Button Agility;
    public Button Damage;
    public Button AttackSpeed;
    public Button CritChance;
    GameManager gameManager;

    public int playerLevel = 1;
    private float baseLevelUpScore = 100;
    private float scoreUntilNextLevel;
    private int skillPoints = 0;
    private bool skillButtonsEnabled = false;
    
    // TODO: Kill me, move to session data
    private int currentScore = 0;

    BasicPlayer playerRef;

    //Set in the inspector window
    public Image experienceBar;
    public Text scoreText;
    public Text nextLevelText;
    public Text maxHPText;
    public Text HPRegenText;
    public Text speedText;
    public Text damageText;
    public Text attackSpeedText;
    public Text critChanceText;

    //Taken from the general canvas, set in inspector
    public Text levelText;

    void Start()
    {
        StartCoroutine(Initialize());
        scoreUntilNextLevel = baseLevelUpScore;
        ToggleSkillButtons();

        MaxHP.onClick.AddListener(() => {
            SkillIncrease(StatType.MAX_HP);
        });

        HPRegen.onClick.AddListener(() => {
            SkillIncrease(StatType.HP_REGEN);
        });

        Agility.onClick.AddListener(() => {
            SkillIncrease(StatType.AGILITY);
        });

        Damage.onClick.AddListener(() => {
            SkillIncrease(StatType.DAMAGE);
        });

        AttackSpeed.onClick.AddListener(() => {
            SkillIncrease(StatType.ATTACK_SPEED);
        });

        CritChance.onClick.AddListener(() => {
            SkillIncrease(StatType.CRIT_CHANCE);
        });
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
            {
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();
                yield return new WaitForEndOfFrame();

                UpdateStats();
                GameManager.Instance.OnAddScore.AddListener(OnAddScore);
            }

            yield return null;
        }
    }

    void UpdateStats() {
        maxHPText.text = $"Max Health: {playerRef.statHandler.MaxHealth}";
        HPRegenText.text = $"HP Regen: {playerRef.statHandler.HealthRegen}";
        speedText.text = $"Speed: {playerRef.statHandler.MoveSpeed}";
        damageText.text = $"Damage: {playerRef.statHandler.Damage} + <color=green>{playerRef.equippedWeapon.weaponStats.weaponDamage}</color>";
        attackSpeedText.text = $"Attack Speed: {playerRef.statHandler.AttackSpeed}";
        critChanceText.text = $"Crit Chance: {playerRef.statHandler.CritChance * 100}%";
    }

    void Update() {
        //////////// Skillpoint Input ///////////////////
        if(skillPoints > 0)
        {
            if(!skillButtonsEnabled)
            {
                skillButtonsEnabled = true;
                ToggleSkillButtons();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                SkillIncrease(StatType.MAX_HP);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SkillIncrease(StatType.HP_REGEN);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SkillIncrease(StatType.AGILITY);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                SkillIncrease(StatType.DAMAGE);

            if (Input.GetKeyDown(KeyCode.Alpha5))
                SkillIncrease(StatType.ATTACK_SPEED);

            if (Input.GetKeyDown(KeyCode.Alpha6))
                SkillIncrease(StatType.CRIT_CHANCE);
        }
        
        ////////////////////////////////////////////
    }

    public void OnAddScore(int value, Vector3 pos)
    {   
        currentScore += value;
        scoreText.text = $"Score: {currentScore}";
        nextLevelText.text = "Exp to next level: " + (scoreUntilNextLevel - currentScore);

        if (scoreUntilNextLevel - currentScore <= 0)
            LevelUp();

        //TODO: Set the exp bar image to the new experience amount using lerps if possible
        //experienceBar.fillAmount = experiencePercentage;
    }

    void LevelUp()
    {
        playerLevel++;
        skillPoints++;
        //levelText.text = playerLevel.ToString();
        scoreUntilNextLevel = scoreUntilNextLevel + playerLevel * baseLevelUpScore;
    }

    public void SkillIncrease(StatType playerStat)
    {
        skillButtonsEnabled = false;
        ToggleSkillButtons();
        skillPoints--;

        playerRef.statHandler.LevelUp(playerStat);

        UpdateStats();
    }

    void ToggleSkillButtons()
    {
        MaxHP.gameObject.SetActive(skillButtonsEnabled);
        HPRegen.gameObject.SetActive(skillButtonsEnabled);
        Agility.gameObject.SetActive(skillButtonsEnabled);
        Damage.gameObject.SetActive(skillButtonsEnabled);
        AttackSpeed.gameObject.SetActive(skillButtonsEnabled);
        CritChance.gameObject.SetActive(skillButtonsEnabled);
    }
}
