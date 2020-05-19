using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    GameManager gameManager;

    public int playerLevel = 1;
    private float baseLevelUpScore = 100;
    private float scoreUntilNextLevel;
    private int skillPoints = 0;
    private bool skillButtonsEnabled = false;

    BasicPlayer playerRef;

    //Set in the inspector window
    public GameObject[] statButtonArray = new GameObject[6];
    public Image experienceBar;
    public Text scoreText;
    public Text nextLevelText;
    public Text maxHPText;
    public Text HPRegenText;
    public Text speedText;
    public Text damageText;
    public Text fireRateText;
    //Taken from the general canvas, set in inspector
    public Text levelText;

    void Start()
    {
        StartCoroutine(Initialize());
        scoreUntilNextLevel = baseLevelUpScore;
        UpdateScore(0);
        ToggleSkillButtons();
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
            {
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();
                StartCoroutine(DisplayPlayerStats());
            }

            yield return null;
        }
    }

    IEnumerator DisplayPlayerStats()
    {
        yield return null;
        maxHPText.text = "Max Health: " + playerRef.playerStats.maxHealth;
        HPRegenText.text = "HP Regen: " + playerRef.playerStats.healthRegenSpeed;
        speedText.text = "Speed: " + playerRef.playerStats.moveSpeed;
        damageText.text = "Damage: " + playerRef.playerStats.damage;
        fireRateText.text = "Fire Rate: " + playerRef.playerStats.fireRate;
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
                SkillIncrease("maxHP");

            if (Input.GetKeyDown(KeyCode.Alpha2))
                SkillIncrease("HPRegen");

            if (Input.GetKeyDown(KeyCode.Alpha3))
                SkillIncrease("speed");

            if (Input.GetKeyDown(KeyCode.Alpha4))
                SkillIncrease("damage");

            if (Input.GetKeyDown(KeyCode.Alpha5))
                SkillIncrease("fireRate");
        }
        
        ////////////////////////////////////////////
    }

    public void UpdateScore(float newScore)
    {
        scoreText.text = "Score: " + newScore;

        if(newScore >= scoreUntilNextLevel)
            LevelUp();

        nextLevelText.text = "Exp to next level: " + (scoreUntilNextLevel - newScore);

        //TODO: Set the exp bar image to the new experience amount using lerps if possible
        //experienceBar.fillAmount = experiencePercentage;
    }

    void LevelUp()
    {
        playerLevel++;
        skillPoints++;
        levelText.text = playerLevel.ToString();
        scoreUntilNextLevel = scoreUntilNextLevel + playerLevel * baseLevelUpScore;
    }

    public void SkillIncrease(string playerStat)
    {
        skillButtonsEnabled = false;
        ToggleSkillButtons();
        skillPoints--;

        switch(playerStat){
            case "maxHP":
                playerRef.playerStats.maxHealth = playerRef.playerStats.maxHealth + 10.0f;
                break;
            case "HPRegen":
                playerRef.playerStats.healthRegenSpeed = playerRef.playerStats.healthRegenSpeed + 1.0f;
                break;
            case "speed":
                playerRef.playerStats.moveSpeed = playerRef.playerStats.moveSpeed + 7.0f;
                break;
            case "damage":
                playerRef.playerStats.damage = playerRef.playerStats.damage + 10.0f;
                break;
            case "fireRate":
                playerRef.playerStats.fireRate = playerRef.playerStats.fireRate + 10.0f;
                break;
            default:
                break;
        }

        StartCoroutine(DisplayPlayerStats());  
    }

    void ToggleSkillButtons()
    {
        foreach(GameObject statButton in statButtonArray)
        {
            statButton.SetActive(skillButtonsEnabled);
        }
    }
}
