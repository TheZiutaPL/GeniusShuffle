using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple on/off toggle button

public class MenuToggle : InteractionHandler
{
    public bool isOn = false;
    [SerializeField] GameObject display;

    [Header("Click animation settings")]
    [SerializeField] PointBasedMovement buttonObject;
    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;

    public override void ClickAction()
    {
        SetStatus(!isOn);
        base.ClickAction();
    }

    public void SetStatus(bool isOn)
    {
        this.isOn = isOn;

        display?.SetActive(isOn);
        if (onPosition != null && onPosition != null)
            buttonObject?.ForceMoveTo(isOn ? onPosition : offPosition);
    }
}
