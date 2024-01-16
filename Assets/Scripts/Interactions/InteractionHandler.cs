using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    [SerializeField] private UnityEvent clickActions;
    [SerializeField] private UnityEvent<bool> hoverActions;

    public void ClickAction() => clickActions?.Invoke();

    public void HoverAction(bool hover) => hoverActions?.Invoke(hover);
}
