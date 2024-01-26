using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// A simple on/off toggle button

public class MenuToggle : MenuElement
{
    public bool isOn = false;
    [SerializeField] GameObject display;

    [Space(10)]
    public UnityEvent<bool> onValueChanged;

    [Header("Click animation settings")]
    [SerializeField] PointBasedMovement buttonObject;
    [SerializeField] Transform onPosition;
    [SerializeField] Transform offPosition;


    public override void ClickAction(Vector3 hitPoint)
    {
        SetStatus(!isOn);
        base.ClickAction(hitPoint);
    }

    public void SetStatus(bool isOn)
    {
        this.isOn = isOn;

        onValueChanged?.Invoke(isOn);
        PlayClickSound();

        display?.SetActive(isOn);
        if (onPosition != null && onPosition != null)
            buttonObject?.ForceMoveTo(isOn ? onPosition : offPosition);
    }
}
