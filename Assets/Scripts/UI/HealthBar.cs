using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Image foregroundImage;
    public float sensitivity = 0.01f;
    public float lerpSpeed = 0.3f; // Seconds

    Coroutine currentCoroutine;
    BasicPlayer playerRef;
    float timeSinceModified = 0;
    public TextMeshProUGUI healthNumberText;
    float displayedCurrentHealth = 0f;

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

    public void UpdateHealth()
    {
        DOTween.To(
            () => displayedCurrentHealth,
            x => {
                displayedCurrentHealth = x;
                healthNumberText.text = $"{string.Format("{0:#0.0}",displayedCurrentHealth)}/{string.Format("{0:#0.0}",playerRef.statHandler.MaxHealth)}";
                foregroundImage.fillAmount = displayedCurrentHealth / playerRef.statHandler.MaxHealth;
            },
            Mathf.Max(playerRef.statHandler.CurrentHealth, 0),
            0.75f
        ).SetUpdate(UpdateType.Fixed);
    }

}
