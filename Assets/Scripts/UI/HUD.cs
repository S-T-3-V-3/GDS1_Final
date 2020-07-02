using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour, IPausable
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI abilityText;
    public GameObject PauseHUD;
    public GameObject mouseIndicator;
    public WeaponStatsUI weaponStats;
    public Image redVignette;
    
    //////////////// TODO: DELETE THIS
    public Image EnergyImage;
    public BasicPlayer playerRef;

    void Update() {
        if (playerRef == null) return;

        float energyPerfect = playerRef.statHandler.Energy / playerRef.statHandler.MaxEnergy;
        EnergyImage.fillAmount = energyPerfect;
    }

    public IEnumerator FadeImpact(){
        
        bool isAdding = true;
        float currentAlpha = 0;
        
        while (currentAlpha >= 0) {

            if(currentAlpha >= 1) isAdding = false;

            if (currentAlpha <= 1 && isAdding) {
                currentAlpha += 0.2f;
                redVignette.color = new Color(redVignette.color.r, redVignette.color.g, redVignette.color.b, currentAlpha);
            } else {
                currentAlpha -= 0.08f;
                redVignette.color = new Color(redVignette.color.r, redVignette.color.g, redVignette.color.b, currentAlpha);
            }

            yield return null;
        }
    }

    public void SetAbilityText(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.DASH:
                abilityText.text = $"Ability [Right Mouse]\n <b><size=28>TELEPORT</size></b>";
                break;
            case AbilityType.RAPIDHEAL:
                abilityText.text = $"Ability [Right Mouse]\n <b><size=28>RAPID REGEN</size></b>";
                break;
            case AbilityType.RAPIDFIRE:
                abilityText.text = $"Ability [Right Mouse]\n <b><size=28>RAPIDFIRE</size></b>";
                break;
            case AbilityType.INVISIBILITY:
                abilityText.text = $"Ability [Right Mouse]\n <b><size=28>INVINCIBILITY</size></b>";
                break;
        }
        
    }

    public void Pause()
    {
        mouseIndicator.SetActive(false);
    }

    public void UnPause()
    {
        mouseIndicator.SetActive(true);
    }
}
