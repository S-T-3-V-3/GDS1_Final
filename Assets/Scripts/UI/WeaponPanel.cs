using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    public DroppedState dropIndicator;
    public string weaponType;
    public RectTransform panelTransform;
    public Text panelText;
    
    //Include Stats
    void Awake(){
        panelTransform = GetComponent<RectTransform>();
    }

    void Start(){
        this.gameObject.SetActive(false);
    }

    void Update(){
        if(dropIndicator == null) {
            this.gameObject.SetActive(false);
            return;
        }

        Vector2 uiPosition = GameManager.Instance.mainCamera.WorldToScreenPoint(dropIndicator.gameObject.transform.position);
        panelTransform.anchoredPosition = uiPosition;
    }

    public void PanelSetup(WeaponType weaponType, GameObject indicator){
        panelText.text = weaponType.ToString();
        dropIndicator = indicator.GetComponent<DroppedState>();
    }

    public void OnIndicatorHover(){
        this.gameObject.SetActive(true);
    }

    public void OnButtonPress(){
        dropIndicator.EquipSelected();
        gameObject.SetActive(false);
    }
}
