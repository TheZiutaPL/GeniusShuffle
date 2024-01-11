using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardInteractionManager : MonoBehaviour
{
    private bool canInteract;

    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private ParticleSystem selectionParticles;

    private CardObject currentTarget;
    private CardObject[] cardSelection = new CardObject[2];

    private void Awake()
    {
        EnableCardInteractions(true);
    }

    public void EnableCardInteractions(bool enable) => canInteract = enable;

    public void CheckForTarget(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPosition = ctx.ReadValue<Vector2>();

        Ray ray = RectTransformUtility.ScreenPointToRay(Camera.main, mouseScreenPosition);

        if(Physics.Raycast(ray, out RaycastHit hit, interactionMask))
        {
            if (hit.transform.gameObject == null)
            {
                currentTarget = null;
                selectionParticles.Stop();
                return;
            }

            CardObject cardObj = hit.transform.GetComponent<CardObject>();

            if (cardObj == null)
                selectionParticles.Stop();
            else if (currentTarget != cardObj)
            {
                selectionParticles.transform.position = cardObj.transform.position;
                selectionParticles.Play();
            }

            currentTarget = cardObj;
        }
    }

    public void InteractWithCard(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled || currentTarget == null)
            return;

        Debug.Log($"Using {currentTarget.gameObject.name} at {currentTarget.transform.position}");


        if (IsCardInSelection(currentTarget))
        {
            //Show card view

            return;
        }
        else
            AddCardToSelection(currentTarget);
    }

    private void AddCardToSelection(CardObject cardObject)
    {
        for (int i = 0; i < cardSelection.Length; i++)
        {
            if (cardSelection[i] != null)
                continue;

            cardSelection[i] = cardObject;

            //Flip a card
            //

            if(i < cardSelection.Length - 1)
                return;
        }

        //Check for winning condition (match)
        bool playerMatchedSuccessfully = IsCardInSelection(cardSelection[0].matchingCard);

        Debug.Log($"Player matched correctly: {playerMatchedSuccessfully}");

        if(playerMatchedSuccessfully)
        {
            //Player can match
            //
        }
        else
        {
            //Player can unflip cards
            //            
        }

        //TO DELETE
        cardSelection = new CardObject[2];
    }

    private bool IsCardInSelection(CardObject cardObject)
    {
        for (int i = 0; i < cardSelection.Length; i++)
        {
            if (cardSelection[i] == cardObject)
                return true;
        }

        return false;
    }
}
