using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [field: SerializeField] public bool isInteractable { get; private set; } = true;

    [Space(10)]

    [SerializeField] private UnityEvent clickActions;
    [SerializeField] private UnityEvent heldDownActions; // Continuously check if pointer is pressed
    [SerializeField] private UnityEvent<bool> hoverActions;
    [SerializeField] private UnityEvent<bool> settingInteractableActions;

    public void SetInteractable(bool enable)
    {
        isInteractable = enable;
        settingInteractableActions?.Invoke(enable);
    }

    public virtual void ClickAction(Vector3 hitPoint) => clickActions?.Invoke();

    public virtual void HeldDownAction(Vector3 hitPoint) => heldDownActions?.Invoke();

    public virtual void HoverAction(bool hover) => hoverActions?.Invoke(hover);

}
