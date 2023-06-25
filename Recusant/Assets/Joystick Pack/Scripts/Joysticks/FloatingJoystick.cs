using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
        
        // Get the RectTransform component of the current object
        rectTransform = GetComponent<RectTransform>();

        // Get the RectTransform component of the parent object
        parentRectTransform = transform.parent.GetComponent<RectTransform>();

        // Set the anchor presets to stretch both horizontally and vertically
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

        // Set the anchor position to the center of the parent
        rectTransform.anchoredPosition = Vector2.zero;

        // Set the size of the RectTransform to match the parent's size
        rectTransform.sizeDelta = Vector2.zero;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}