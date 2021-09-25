using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomJoystick : Joystick
{
    private Vector2 defaultPostion;

    private void Awake()
    {
        defaultPostion = background.anchoredPosition;
    }

    private void OnEnable()
    {
        background.anchoredPosition = defaultPostion;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);        
    }
}
