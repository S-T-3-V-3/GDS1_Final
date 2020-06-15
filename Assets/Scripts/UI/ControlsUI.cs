using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    public void Return()
    {
        GameManager.Instance.PauseHUD.SetActive(true);
        gameObject.SetActive(false);
    }
}
