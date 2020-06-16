using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayedAction : MonoBehaviour
{
    public UnityAction delayedAction;
    public float maxDelayTime = 0f;
    public bool repeating = false;
    float timeElapsed = 0f;
    bool hasInvoked = false;

    void Update()
    {
        if (maxDelayTime == 0f) return;
        if (GameManager.Instance.sessionData.isPaused) return;

        if (timeElapsed <= maxDelayTime) {
            timeElapsed += Time.deltaTime;
        }
        else if (!hasInvoked) {
            if (delayedAction != null)
                delayedAction.Invoke();

            if (repeating)
                Reset();
            else
                GameObject.Destroy(this.gameObject);
        }
    }

    public void Reset() {
        hasInvoked = false;
        timeElapsed = 0f;
    }
}
