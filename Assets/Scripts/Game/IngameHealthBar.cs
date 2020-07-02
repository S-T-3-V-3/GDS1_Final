using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameHealthBar : MonoBehaviour
{
    public SpriteRenderer background;
    public SpriteRenderer foreground;

    StatHandler stats;
    Coroutine fader;
    bool isHidden = true;

    void Start()
    {
        stats = this.transform.parent.GetComponent<Pawn>().statHandler;

        if (stats == null) {
            GameObject.Destroy(this.gameObject);
            return;
        }
        
        background.color.Equals(new Color(background.color.r, background.color.g, background.color.b, 0f));
        foreground.color.Equals(new Color(foreground.color.r, foreground.color.g, foreground.color.b, 0f));

        UpdateHealth();

        stats.OnHealthChanged.AddListener(UpdateHealth);
    }

    void Show() {
        if (fader != null)
            StopAllCoroutines();

        fader = StartCoroutine(Fade(0,1));
        isHidden = false;
    }

    void Hide() {
        if (fader != null)
            StopAllCoroutines();

        fader = StartCoroutine(Fade(1,0));
        isHidden = true;
    }

    void Update() {
        this.transform.LookAt(GameManager.Instance.mainCamera.transform, Vector3.up);
    }

    void UpdateHealth() {
        if (stats.CurrentHealth == stats.MaxHealth && !isHidden) {
            Hide();
        }
        else if (stats.CurrentHealth != stats.MaxHealth && isHidden) {
            Show();
        }

        float healthAlpha = stats.CurrentHealth / stats.MaxHealth;
        foreground.transform.localScale = new Vector3(healthAlpha, 1, 1);
        foreground.transform.localPosition = new Vector3((1-healthAlpha)/2,0,0.001f);
    }

    IEnumerator Fade(float startAlpha, float endAlpha) {
        startAlpha *= 255;
        endAlpha *= 255;
        
        float fadeTime = 0.3f;
        float currentAlpha = background.color.a/255;
        float elapsedTime = currentAlpha * fadeTime;

        while (elapsedTime < fadeTime) {
            currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime/fadeTime);
            background.color.Equals(new Color(background.color.r, background.color.g, background.color.b, currentAlpha));
            foreground.color.Equals(new Color(foreground.color.r, foreground.color.g, foreground.color.b, currentAlpha));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        background.color.Equals(new Color(background.color.r, background.color.g, background.color.b, endAlpha));
        foreground.color.Equals(new Color(foreground.color.r, foreground.color.g, foreground.color.b, endAlpha));
    }
}
