using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MouseIndicator : MonoBehaviour
{
    public RectTransform indicatorTransform;
    public Canvas parentCanvas;
    RectTransform parentTransform;
    Image indicatorImage;
    Animator indicatorAnim;

    // Start is called before the first frame update
    void Start()
    {
        indicatorTransform = GetComponent<RectTransform>();
        parentTransform = parentCanvas.GetComponent<RectTransform>();
        indicatorAnim = GetComponent<Animator>();
        indicatorImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //indicatorTransform.anchoredPosition += Vector2.right * 2;
    }

    private void MoveByMouse(InputValue value){
        Debug.Log(value.Get<Vector2>());
    }

    public void SetIndicatorPosition(Vector2 mousePos){
        //Debug.Log(mousePos);
        //Debug.Log(parentCanvas);
        //indicatorTransform.anchoredPosition = parentTransform.TransformPoint(mousePos);

        float actualWidth = Screen.width;
        float actualHeight = Screen.height;

        float scaledMultiplierNormalisedX = (mousePos.x / actualWidth) * 1;
        float scaledMultiplierNormalisedY = (mousePos.y / actualHeight) * 1;
        //Debug.Log(actualWidth + ", " + actualHeight);
        //Debug.Log(scaledMultiplierNormalised);

        float scaledWidth = parentTransform.rect.width * scaledMultiplierNormalisedX;
        float scaledHeight = parentTransform.rect.height * scaledMultiplierNormalisedY;

        Vector2 scaledMousePosition = new Vector2(scaledWidth,scaledHeight);
        indicatorTransform.anchoredPosition = scaledMousePosition;
    }

    public void SetTransitionState(bool isActive){
        if(isActive) {
            indicatorAnim.SetBool("isActive", true);
            indicatorImage.color = new Color(1f, 0f, 0f, 1f);
        } else {
            indicatorAnim.SetBool("isActive", false);
            indicatorImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
    }



}
