using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    private bool canInteract;

    [Header("Setup")]
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private GameInputs inputAction;

    private Camera playerCamera;
    private Vector2 mouseScreenPosition;

    private InteractionHandler currentTarget;

    private Vector3 interactionHitPoint;

    private void Awake()
    {
        playerCamera = Camera.main;

        EnableInteractions(true);
    }

    public void EnableInteractions(bool enable) => canInteract = enable;

    public void UpdateMouseScreenPosition(InputAction.CallbackContext ctx)
    {
        mouseScreenPosition = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if (canInteract && Physics.Raycast(RectTransformUtility.ScreenPointToRay(playerCamera, mouseScreenPosition), out RaycastHit hit, interactionMask))
        {
            InteractionHandler interaction;
            if (hit.transform.gameObject == null || !hit.transform.gameObject.TryGetComponent(out interaction))
            {
                SwitchTarget();
                return;
            }

            if (!interaction.isInteractable) interaction = null;

            interactionHitPoint = hit.point;

            if (interaction != currentTarget)
                SwitchTarget(interaction);

            if (Pointer.current.press.isPressed)
                currentTarget.HeldDownAction(interactionHitPoint);
        }
        else if (currentTarget != null)
            SwitchTarget();
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled || currentTarget == null)
            return;

        currentTarget.ClickAction(interactionHitPoint);
    }

    private void SwitchTarget(InteractionHandler interaction = null)
    {
        if (currentTarget != null)
            currentTarget.HoverAction(false);

        if (interaction != null)
            interaction.HoverAction(true);

        currentTarget = interaction;
    }
}
