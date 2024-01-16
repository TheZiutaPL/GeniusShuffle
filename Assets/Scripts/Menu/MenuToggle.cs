using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuToggle : InteractionHandler
{
    public bool isOn = false;

    public override void ClickAction()
    {
        isOn = !isOn;
        base.ClickAction();
    }
}
