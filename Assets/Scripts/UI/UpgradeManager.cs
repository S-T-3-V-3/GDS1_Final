using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

// TODO: TEXT MESH PRO REFACTOR
public class UpgradeManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button MaxHP;
    public Button HPRegen;
    public Button Energy;
    public Button Damage;
    public Button AttackSpeed;
    public Button CritChance;
    GameManager gameManager;

    public int skillPoints = 0;
    private bool skillButtonsEnabled = false;

    BasicPlayer playerRef;

    //Set in the inspector window
    public TextMeshProUGUI maxHPText;
    public TextMeshProUGUI HPRegenText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI critChanceText;

    //Used for the tweener
    private RectTransform upgradeInterface;
    private Vector2 originalInterfacePosition;
    Tween upgradeInterfaceTween;

    void Start()
    {
        upgradeInterface = GetComponent<RectTransform>();
        originalInterfacePosition = upgradeInterface.position;

        StartCoroutine(Initialize());
        ToggleSkillButtons();

        MaxHP.onClick.AddListener(() => {
            SkillIncrease(StatType.MAX_HP);
        });

        HPRegen.onClick.AddListener(() => {
            SkillIncrease(StatType.HP_REGEN);
        });

        Energy.onClick.AddListener(() => {
            SkillIncrease(StatType.ENERGY);
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
            }

            yield return null;
        }
    }

    void UpdateStats() {
        maxHPText.text = $"{playerRef.statHandler.MaxHealth}";
        HPRegenText.text = $"{playerRef.statHandler.HealthRegen}";
        speedText.text = $"{System.Math.Round(playerRef.statHandler.SprintSpeed, 2)}";
        damageText.text = $"{playerRef.statHandler.Damage} + <color=green>{playerRef.equippedWeapon.weaponStats.weaponDamage}</color>";
        attackSpeedText.text = $"{playerRef.statHandler.AttackSpeed}";
        critChanceText.text = $"{playerRef.statHandler.CritChance}%";
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
                SkillIncrease(StatType.ENERGY);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                SkillIncrease(StatType.DAMAGE);

            if (Input.GetKeyDown(KeyCode.Alpha5))
                SkillIncrease(StatType.ATTACK_SPEED);

            if (Input.GetKeyDown(KeyCode.Alpha6))
                SkillIncrease(StatType.CRIT_CHANCE);
        }
        ////////////////////////////////////////////

        //////////// Tweener ///////////////////

        ////////////////////////////////////////////
    }

    public void SkillIncrease(StatType playerStat)
    {
        skillButtonsEnabled = false;
        ToggleSkillButtons();
        skillPoints--;

        playerRef.statHandler.LevelUp(playerStat);

        UpdateStats();

        if(skillPoints <= 0)
        {
            HideUpgradeWindow();
        }
    }

    void ToggleSkillButtons()
    {
        MaxHP.interactable = skillButtonsEnabled;
        HPRegen.interactable = skillButtonsEnabled;
        Energy.interactable = skillButtonsEnabled;
        Damage.interactable= skillButtonsEnabled;
        AttackSpeed.interactable = skillButtonsEnabled;
        CritChance.interactable = skillButtonsEnabled;
    }


    /////////////////////////////////
    // DISPLAY AND HIDE UPGRADE UI //
    /////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillButtonsEnabled) return;
        ShowUpgradeWindow();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (skillButtonsEnabled) return;
        HideUpgradeWindow();
    }

    public void ShowUpgradeWindow()
    {
        if(upgradeInterface.position.y < 0)
        {
            upgradeInterface.DOMoveY(0, 0.5f, false);
        }
    }

    public void HideUpgradeWindow()
    {
        upgradeInterface.DOMoveY(originalInterfacePosition.y, 0.5f, false);
    }
}
