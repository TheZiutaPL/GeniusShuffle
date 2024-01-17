using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : InteractionHandler
{
    public bool isOn = false;

    public override void ClickAction()
    {
        SetStatus(!isOn);
        base.ClickAction();
    }

    public void SetStatus(bool isOn)
    {
        this.isOn = isOn;
    }
}
