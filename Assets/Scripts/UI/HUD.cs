using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour, IPausable
{
    public ScoreManager scoreBoard;
    public GameObject PauseHUD;
    public GameObject mouseIndicator;
    public WeaponPanel weaponPanel;
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

    public void Pause()
    {
        mouseIndicator.SetActive(false);
    }

    public void UnPause()
    {
        mouseIndicator.SetActive(true);
    }
}
