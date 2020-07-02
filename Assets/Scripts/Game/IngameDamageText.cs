using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class IngameDamageText : MonoBehaviour
{
    public TextMeshPro damageText;
    public float maxLifeTime = 1f;
    public Vector3 offset;
    public Vector3 moveOffset;
    float elapsedTime = 0f;
    bool isFading = false;

    void Start() {
        this.transform.localPosition += offset;
        this.transform.DOLocalMove(this.transform.position + offset, 1f);    
    }

    void Update() {
        this.transform.LookAt(GameManager.Instance.mainCamera.transform, Vector3.up);

        if (GameManager.Instance.sessionData.isPaused) return;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 0.5f && !isFading)
            BeginFade();

        if (elapsedTime > maxLifeTime) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void BeginFade() {
        isFading = true;
        damageText.DOFade(0f,0.5f);
    }
}
