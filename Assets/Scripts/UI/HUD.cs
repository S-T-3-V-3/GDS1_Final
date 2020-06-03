using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public ScoreManager scoreBoard;
    
    //////////////// TODO: DELETE THIS
    public Image EnergyImage;
    public BasicPlayer playerRef;

    void Update() {
        if (playerRef == null) return;

        float energyPerfect = playerRef.statHandler.Energy / playerRef.statHandler.MaxEnergy;
        EnergyImage.fillAmount = energyPerfect;
    }
}
