using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: TEXT MESH PRO REFACTOR
public class ScoreManager : MonoBehaviour
{
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
        ToggleSkillButtons();
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
        maxHPText.text = $"Max Health: {playerRef.playerStats.maxHealth}";
        HPRegenText.text = $"HP Regen: {playerRef.playerStats.healthRegenSpeed}";
        speedText.text = $"Speed: {playerRef.playerStats.moveSpeed}";
        damageText.text = $"Damage: {playerRef.playerStats.damage} + <color=green>{playerRef.equippedWeapon.weaponStats.weaponDamage}</color>";
        fireRateText.text = $"Fire Rate: {playerRef.playerStats.fireRate}";
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
        levelText.text = playerLevel.ToString();
        scoreUntilNextLevel = scoreUntilNextLevel + playerLevel * baseLevelUpScore;
    }

    public void SkillIncrease(string playerStat)
    {
        skillButtonsEnabled = false;
        ToggleSkillButtons();
        skillPoints--;

        playerRef.LevelUp(playerStat);

        UpdateStats();
    }

    void ToggleSkillButtons()
    {
        foreach(GameObject statButton in statButtonArray)
        {
            statButton.SetActive(skillButtonsEnabled);
        }
    }
}
