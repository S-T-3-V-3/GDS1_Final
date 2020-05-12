using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public float updateSpeedSeconds;

    void Awake()
    {
        GetComponentInParent<Health>().OnHealthPercentChange += HandleHealthChange;
    }

    private void HandleHealthChange(float percent)
    {
        StartCoroutine(ChangeToPercent(percent));
    }

    IEnumerator ChangeToPercent(float newPercent)
    {
        float previousPercent = foregroundImage.fillAmount;
        float timeElapsed = 0f;

        //Lerp to the new health percent
        while (timeElapsed < updateSpeedSeconds)
        {
            timeElapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Lerp(previousPercent, newPercent, timeElapsed / updateSpeedSeconds);
            yield return null;
        }

        //Set image to the new health amount
        foregroundImage.fillAmount = newPercent;
    }

}
