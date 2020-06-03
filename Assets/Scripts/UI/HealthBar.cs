using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public float sensitivity = 0.01f;
    public float lerpSpeed = 0.3f; // Seconds

    Coroutine currentCoroutine;
    BasicPlayer playerRef;
    float timeSinceModified = 0;
    public TextMeshProUGUI healthNumberText;

    void Start()
    {
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();

            yield return null;
        }

        playerRef.statHandler.OnHealthChanged.AddListener(UpdateHealth);
        //// TODO: Delete below line
        GameManager.Instance.hud.playerRef = playerRef;
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float targetHealthPercent = playerRef.statHandler.CurrentHealth / playerRef.statHandler.MaxHealth;

        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        currentCoroutine = StartCoroutine(UpdateImage(targetHealthPercent));
    }

    IEnumerator UpdateImage(float targetHealthPercent)
    {
        float currentHealthPercent = foregroundImage.fillAmount;
        float startHealthPercent = currentHealthPercent;
        timeSinceModified = 0f;

        //Lerp to the new health percent
        while (Mathf.Abs(currentHealthPercent - targetHealthPercent) > sensitivity)
        {
            yield return new WaitForEndOfFrame();

            timeSinceModified += Time.deltaTime;
            currentHealthPercent = Mathf.Lerp(startHealthPercent, targetHealthPercent, timeSinceModified / lerpSpeed);
            
            foregroundImage.fillAmount = currentHealthPercent;

            UpdateText();
        }

        //Set image to the new health amount
        foregroundImage.fillAmount = targetHealthPercent;
        UpdateText();
    }

    public void UpdateText()
    {
        int displayedCurrentHealth = (int)Mathf.Ceil(playerRef.statHandler.CurrentHealth);
        
        if(displayedCurrentHealth < 0)
            displayedCurrentHealth = 0;

        healthNumberText.text = $"{displayedCurrentHealth}/{playerRef.statHandler.MaxHealth}";
    }

}
