using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public float sensitivity = 0.01f;
    public float lerpSpeed = 1f; // Seconds

    Coroutine currentCoroutine;
    BasicPlayer playerRef;
    float timeSinceModified = 0;

    public Transform foregroundImageTransform;
    public Vector3 foregroundImageOriginalPos = new Vector3();

    void Start()
    {
        StartCoroutine(Initialize());
        foregroundImageOriginalPos = foregroundImageTransform.position;
    }

    private void UpdateHealth()
    {
        float targetHealthPercent = playerRef.playerStats.currentHealth / playerRef.playerStats.maxHealth;

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

           AdjustHealthBarPos(currentHealthPercent); 
        }

        //Set image to the new health amount
        foregroundImage.fillAmount = targetHealthPercent;

        AdjustHealthBarPos(targetHealthPercent); 
        
    }

    IEnumerator Initialize() {
        while (playerRef == null) {
            if (GameManager.Instance.playerController != null)
                playerRef = GameManager.Instance.playerController.GetComponent<BasicPlayer>();

            yield return null;
        }

        // TODO: Explore other methods, as health regen now runs every frame that we aren't full HP!
        playerRef.OnHealthChanged.AddListener(UpdateHealth);
        UpdateHealth();
    }

    private void AdjustHealthBarPos(float targetHealthPercent)
    {
        foregroundImageTransform.position = new Vector2(
             targetHealthPercent * (foregroundImageOriginalPos.x+110) - 110,
             foregroundImageTransform.position.y);
    }
}
