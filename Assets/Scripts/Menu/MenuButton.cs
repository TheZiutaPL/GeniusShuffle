using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MenuElement
{
    [SerializeField] UnityEvent onClick;

    public override void OnPointerHover()
    {
        Debug.Log("This is a button");
    }

    public override void OnClicked()
    {
        onClick?.Invoke();
    }
}
