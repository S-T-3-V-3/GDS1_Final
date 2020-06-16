using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UIUpgradeHandler : MonoBehaviour
{
    public TextMeshProUGUI remainingSkillPointsText;
    public int RemainingSkillPoints {
        get {
            return remainingSkillPoints;
        }
        
        set {
            if (remainingSkillPoints <= 0) {
                // Fade In
                remainingSkillPointsText.DOFade(1f,0.5f);
                remainingSkillPointsText.gameObject.transform.DOMoveX(0f, 0.5f).From(-50f, true);

                        AnimateStat(maxHPText, maxHPValue, LevelStatus.FULL);
                        AnimateStat(HPRegenText, HPRegenValue, LevelStatus.FULL);
                        AnimateStat(energyText, energyValue, LevelStatus.FULL);
                        AnimateStat(damageText, damageValue, LevelStatus.FULL);
                        AnimateStat(attackSpeedText, attackSpeedValue, LevelStatus.FULL);
                        AnimateStat(critChanceText, critChanceValue, LevelStatus.FULL);
            }

            remainingSkillPoints = value;
            remainingSkillPointsText.text = $"Points\n <b><size=30> {remainingSkillPoints}</size></b>";
            
            if (remainingSkillPoints <= 0) {
                // Fade Out
                remainingSkillPointsText.DOFade(0f,0.5f);
                remainingSkillPointsText.gameObject.transform.DOMoveX(-50f, 0.5f).From(0f, true);

                        AnimateStat(maxHPText, maxHPValue, LevelStatus.VALUEONLY);
                        AnimateStat(HPRegenText, HPRegenValue, LevelStatus.VALUEONLY);
                        AnimateStat(energyText, energyValue, LevelStatus.VALUEONLY);
                        AnimateStat(damageText, damageValue, LevelStatus.VALUEONLY);
                        AnimateStat(attackSpeedText, attackSpeedValue, LevelStatus.VALUEONLY);
                        AnimateStat(critChanceText, critChanceValue, LevelStatus.VALUEONLY);
            }
        }
    }
    int remainingSkillPoints = 0;

    public Color highlightColor;
    public Button HPButton;
    public Button RegenButton;
    public Button EnergyButton;
    public Button DamageButton;
    public Button AttackSpeedButton;
    public Button CritChanceButton;

    BasicPlayer playerRef;

    // Level + Name
    public TextMeshProUGUI maxHPText;
    public TextMeshProUGUI HPRegenText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI attackSpeedText;
    public TextMeshProUGUI critChanceText;

    // Current displayed value
    public TextMeshProUGUI maxHPValue;
    public TextMeshProUGUI HPRegenValue;
    public TextMeshProUGUI energyValue;
    public TextMeshProUGUI damageValue;
    public TextMeshProUGUI attackSpeedValue;
    public TextMeshProUGUI critChanceValue;

    // Current status
    public LevelStatus HPStatus = LevelStatus.VALUEONLY;
    public LevelStatus RegenStatus = LevelStatus.VALUEONLY;
    public LevelStatus EnergyStatus = LevelStatus.VALUEONLY;
    public LevelStatus DamageStatus = LevelStatus.VALUEONLY;
    public LevelStatus AttackSpeedStatus = LevelStatus.VALUEONLY;
    public LevelStatus CritChanceStatus = LevelStatus.VALUEONLY;

    //Used for the tweener
    private RectTransform upgradeInterface;
    private Vector2 originalInterfacePosition;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();

        remainingSkillPointsText.color = highlightColor;

        upgradeInterface = GetComponent<RectTransform>();
        originalInterfacePosition = upgradeInterface.position;

        StartCoroutine(Initialize());

        HPButton.onClick.AddListener(() => {
            SkillIncrease(StatType.MAX_HP);
            HPButton.OnDeselect(null);
        });

        RegenButton.onClick.AddListener(() => {
            SkillIncrease(StatType.HP_REGEN);
            RegenButton.OnDeselect(null);
        });

        EnergyButton.onClick.AddListener(() => {
            SkillIncrease(StatType.ENERGY);
            EnergyButton.OnDeselect(null);
        });

        DamageButton.onClick.AddListener(() => {
            SkillIncrease(StatType.DAMAGE);
            DamageButton.OnDeselect(null);
        });

        AttackSpeedButton.onClick.AddListener(() => {
            SkillIncrease(StatType.ATTACK_SPEED);
            AttackSpeedButton.OnDeselect(null);
        });

        CritChanceButton.onClick.AddListener(() => {
            SkillIncrease(StatType.CRIT_CHANCE);
            CritChanceButton.OnDeselect(null);
        });

        AnimateStat(maxHPText, maxHPValue, LevelStatus.VALUEONLY);
        AnimateStat(HPRegenText, HPRegenValue, LevelStatus.VALUEONLY);
        AnimateStat(energyText, energyValue, LevelStatus.VALUEONLY);
        AnimateStat(damageText, damageValue, LevelStatus.VALUEONLY);
        AnimateStat(attackSpeedText, attackSpeedValue, LevelStatus.VALUEONLY);
        AnimateStat(critChanceText, critChanceValue, LevelStatus.VALUEONLY);
        RemainingSkillPoints = 0;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1))
            SkillIncrease(StatType.MAX_HP);
        
        if (Input.GetKeyDown(KeyCode.F2))
            SkillIncrease(StatType.HP_REGEN);
        
        if (Input.GetKeyDown(KeyCode.F3))
            SkillIncrease(StatType.ENERGY);
        
        if (Input.GetKeyDown(KeyCode.F4))
            SkillIncrease(StatType.DAMAGE);
        
        if (Input.GetKeyDown(KeyCode.F5))
            SkillIncrease(StatType.ATTACK_SPEED);

        if (Input.GetKeyDown(KeyCode.F6))
            SkillIncrease(StatType.CRIT_CHANCE);
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
            {
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();
                yield return null;

                GameManager.Instance.scoreManager.OnLevelUp.AddListener(OnLevelUp);
                UpdateStats();
            }

            yield return null;
        }
    }

    void OnLevelUp() {
        RemainingSkillPoints++;
    }

    public void Show() {
        MoveTransformX(0);
        canvasGroup.DOFade(1f,0.5f);
    }

    public void Hide() {
        MoveTransformX(-this.GetComponent<RectTransform>().rect.width);
        canvasGroup.DOFade(0f,0.5f);
    }

    public void MoveTransformX(float target) {
        this.transform.DOKill();
        this.transform.DOMoveX(target,0.5f);
        this.transform.DOPlay();
    }

    public void ShowStats() {
        HPButton.interactable = true;
        RegenButton.interactable = true;
        EnergyButton.interactable = true;
        DamageButton.interactable= true;
        AttackSpeedButton.interactable = true;
        CritChanceButton.interactable = true;
    }

    void UpdateStats() {
        maxHPText.text = $"[{playerRef.statHandler.MaxHealthLevel}] HP";
        maxHPValue.text = $"{playerRef.statHandler.MaxHealth}";
        HPStatus = LevelStatus.FULL;

        HPRegenText.text = $"[{playerRef.statHandler.HealthRegenLevel}] Regen";
        HPRegenValue.text = $"+{playerRef.statHandler.HealthRegen} HP/s";
        RegenStatus = LevelStatus.FULL;

        energyText.text = $"[{playerRef.statHandler.EnergyLevel}] Energy";
        energyValue.text = $"{playerRef.statHandler.MaxEnergy}";
        EnergyStatus = LevelStatus.FULL;

        damageText.text = $"[{playerRef.statHandler.DamageLevel}] Damage";
        damageValue.text = $"{playerRef.statHandler.Damage}x";
        DamageStatus = LevelStatus.FULL;

        attackSpeedText.text = $"[{playerRef.statHandler.AttackSpeedLevel}] Attack Speed";
        attackSpeedValue.text = $"{playerRef.statHandler.AttackSpeed}x";
        AttackSpeedStatus = LevelStatus.FULL;

        critChanceText.text = $"[{playerRef.statHandler.CritChanceLevel}] Crit Chance";
        critChanceValue.text = $"{playerRef.statHandler.CritChance}%";
        CritChanceStatus = LevelStatus.FULL;

    }

    public void SkillIncrease(StatType playerStat)
    {
        if (RemainingSkillPoints > 0)
            RemainingSkillPoints--;
        else
            return;

        playerRef.statHandler.LevelUp(playerStat);

        UpdateStats();
    }

    void AnimateStat(TextMeshProUGUI statText, TextMeshProUGUI statValue, LevelStatus targetStatus) {
        switch (targetStatus) {
            case LevelStatus.FULL:
                statText.DOKill();
                statText.DOFade(1f,0.5f);

                statValue.DOKill();
                statValue.GetComponent<RectTransform>().DOLocalMoveY(5f,0.5f);

                statText.DOPlay();
                statValue.DOPlay();
                break;

            case LevelStatus.VALUEONLY:
                statText.DOKill();
                statText.DOFade(0f,0.5f);

                statValue.DOKill();
                statValue.GetComponent<RectTransform>().DOLocalMoveY(15f,0.5f);

                statText.DOPlay();
                statValue.DOPlay();
                break;

            case LevelStatus.HIDDEN:

                break;
        }
    }

    public void OnHPMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            maxHPText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.MaxHealthLevel + 1}</color>] HP";
            maxHPValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.PreviewStatIncrease(StatType.MAX_HP)}</color>";
            HPStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(maxHPText, maxHPValue, LevelStatus.FULL);
            HPStatus = LevelStatus.FULL;
        }
    }

    public void OnHPMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (HPStatus == LevelStatus.PREVIEW) {
                maxHPText.text = $"[{playerRef.statHandler.MaxHealthLevel}] HP";
                maxHPValue.text = $"{playerRef.statHandler.MaxHealth}";
                HPStatus = LevelStatus.FULL;
            }
        }
        
        else if (HPStatus == LevelStatus.FULL) {
            AnimateStat(maxHPText, maxHPValue, LevelStatus.VALUEONLY);
            HPStatus = LevelStatus.VALUEONLY;
        }
    }

    public void OnRegenMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            HPRegenText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.HealthRegenLevel + 1}</color>] Regen";
            HPRegenValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>+{playerRef.statHandler.PreviewStatIncrease(StatType.HP_REGEN)}</color> HP/s";
            RegenStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(HPRegenText, HPRegenValue, LevelStatus.FULL);
            RegenStatus = LevelStatus.FULL;
        }
    }

    public void OnRegenMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (RegenStatus == LevelStatus.PREVIEW) {
                HPRegenText.text = $"[{playerRef.statHandler.HealthRegenLevel}] Regen";
                HPRegenValue.text = $"+{playerRef.statHandler.HealthRegen} HP/s";
                RegenStatus = LevelStatus.FULL;
            }
        }
        
        else if (RegenStatus == LevelStatus.FULL) {
            AnimateStat(HPRegenText, HPRegenValue, LevelStatus.VALUEONLY);
            RegenStatus = LevelStatus.VALUEONLY;
        }
    }

    public void OnEnergyMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            energyText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.EnergyLevel + 1}</color>] Energy";
            energyValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.PreviewStatIncrease(StatType.ENERGY)}</color>";
            EnergyStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(energyText, energyValue, LevelStatus.FULL);
            EnergyStatus = LevelStatus.FULL;
        }
    }

    public void OnEnergyMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (EnergyStatus == LevelStatus.PREVIEW) {
                energyText.text = $"[{playerRef.statHandler.EnergyLevel}] Energy";
                energyValue.text = $"{playerRef.statHandler.MaxEnergy}";
                EnergyStatus = LevelStatus.FULL;
            }
        }
        
        else if (EnergyStatus == LevelStatus.FULL) {
            AnimateStat(energyText, energyValue, LevelStatus.VALUEONLY);
            EnergyStatus = LevelStatus.VALUEONLY;
        }
    }

    public void OnDamageMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            damageText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.DamageLevel + 1}</color>] Damage";
            damageValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.PreviewStatIncrease(StatType.DAMAGE)}</color>x";
            DamageStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(damageText, damageValue, LevelStatus.FULL);
            DamageStatus = LevelStatus.FULL;
        }
    }

    public void OnDamageMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (DamageStatus == LevelStatus.PREVIEW) {
                damageText.text = $"[{playerRef.statHandler.DamageLevel}] Damage";
                damageValue.text = $"{playerRef.statHandler.Damage}x";
                DamageStatus = LevelStatus.FULL;
            }
        }
        
        else if (DamageStatus == LevelStatus.FULL) {
            AnimateStat(damageText, damageValue, LevelStatus.VALUEONLY);
            DamageStatus = LevelStatus.VALUEONLY;
        }
    }

    public void OnAttackSpeedMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            attackSpeedText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.AttackSpeedLevel + 1}</color>] Attack Speed";
            attackSpeedValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.PreviewStatIncrease(StatType.ATTACK_SPEED)}</color>x";
            AttackSpeedStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(attackSpeedText, attackSpeedValue, LevelStatus.FULL);
            AttackSpeedStatus = LevelStatus.FULL;
        }
    }

    public void OnAttackSpeedMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (AttackSpeedStatus == LevelStatus.PREVIEW) {
                attackSpeedText.text = $"[{playerRef.statHandler.AttackSpeedLevel}] Attack Speed";
                attackSpeedValue.text = $"{playerRef.statHandler.AttackSpeed}x";
                AttackSpeedStatus = LevelStatus.FULL;
            }
        }
        
        else if (AttackSpeedStatus == LevelStatus.FULL) {
            AnimateStat(attackSpeedText, attackSpeedValue, LevelStatus.VALUEONLY);
            AttackSpeedStatus = LevelStatus.VALUEONLY;
        }
    }

    public void OnCritMouseEnter(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            // Preview Level Up Values
            critChanceText.text = $"[<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.CritChanceLevel + 1}</color>] Crit Chance";
            critChanceValue.text = $"<color=#{ColorUtility.ToHtmlStringRGB(highlightColor)}>{playerRef.statHandler.PreviewStatIncrease(StatType.CRIT_CHANCE)}</color>%";
            CritChanceStatus = LevelStatus.PREVIEW;
        }
        else {
            // Show Current Level, Move Value Down
            AnimateStat(critChanceText, critChanceValue, LevelStatus.FULL);
            CritChanceStatus = LevelStatus.FULL;
        }
    }

    public void OnCritMouseExit(BaseEventData eventData) {
        if (RemainingSkillPoints > 0) {
            if (CritChanceStatus == LevelStatus.PREVIEW) {
                critChanceText.text = $"[{playerRef.statHandler.CritChanceLevel}] Crit Chance";
                critChanceValue.text = $"{playerRef.statHandler.CritChance}%";
                CritChanceStatus = LevelStatus.FULL;
            }
        }
        
        else if (CritChanceStatus == LevelStatus.FULL) {
            AnimateStat(critChanceText, critChanceValue, LevelStatus.VALUEONLY);
            CritChanceStatus = LevelStatus.VALUEONLY;
        }
    }
}

public enum LevelStatus {
    HIDDEN,
    VALUEONLY,
    FULL,
    PREVIEW
}