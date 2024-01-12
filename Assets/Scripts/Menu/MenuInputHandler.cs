using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputHandler : MonoBehaviour
{
    [SerializeField] LayerMask interactionMask;

    bool active = true;
    Vector2 mousePosition = new Vector2();

    MenuElement highlightedElement = null;

    public void SetActive(bool active) => this.active = active;

    public void OnPointerMove(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();

        if (active && Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit, interactionMask))
        {
            if (hit.collider.TryGetComponent(out highlightedElement))
                highlightedElement.OnPointerHover();
        }
    }

    public void OnClick(InputAction.CallbackContext ctx) 
    {
        highlightedElement?.OnClicked();
    }
}
