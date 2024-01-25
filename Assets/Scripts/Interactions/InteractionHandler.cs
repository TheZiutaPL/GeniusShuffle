using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [HideInInspector] public bool isInteractable = true;

    [SerializeField] private UnityEvent clickActions;
    [SerializeField] private UnityEvent heldDownActions; // Continuously check if pointer is pressed
    [SerializeField] private UnityEvent<bool> hoverActions;

    public virtual void ClickAction(Vector3 hitPoint) => clickActions?.Invoke();

    public virtual void HeldDownAction(Vector3 hitPoint) => heldDownActions?.Invoke();

    public virtual void HoverAction(bool hover) => hoverActions?.Invoke(hover);

}
