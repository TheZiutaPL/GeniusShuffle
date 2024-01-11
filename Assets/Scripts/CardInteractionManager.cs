using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardInteractionManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private bool canInteract;

    [Header("Setup")]
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private ParticleSystem selectionParticles;

    private CardObject currentTarget;
    private CardObject[] cardSelection = new CardObject[2];

    [SerializeField] private CardGraveyard cardGraveyard;
    [SerializeField] private float cardSwoopTime = .7f;
    [SerializeField] private AnimationCurve cardSwoopCurve;

    private void Awake()
    { 
        EnableCardInteractions(true);
    }

    public void EnableCardInteractions(bool enable) => canInteract = enable;

    public void CheckForTarget(InputAction.CallbackContext ctx)
    {
        Vector2 mouseScreenPosition = ctx.ReadValue<Vector2>();

        if(canInteract && Physics.Raycast(RectTransformUtility.ScreenPointToRay(Camera.main, mouseScreenPosition), out RaycastHit hit, interactionMask))
        {
            if (hit.transform.TryGetComponent(out CardObject cardObj) && !cardObj.isInteractable) cardObj = null;

            if (currentTarget == cardObj)
                return;

            if (cardObj == null)
                selectionParticles.Stop();
            else if (currentTarget != cardObj)
            {
                cardObj.SetHover(true);

                selectionParticles.transform.position = cardObj.transform.position;
                selectionParticles.Play();
            }

            if(currentTarget != null)
                currentTarget.SetHover(false);

            currentTarget = cardObj;
        }
        else if(currentTarget != null)
        {
            currentTarget.SetHover(false);

            currentTarget = null;
            selectionParticles.Stop();
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

            cardObject.SetFlip(true);

            if(i < cardSelection.Length - 1)
                return;
        }

        //Check for winning condition (match)
        bool playerMatchedSuccessfully = IsCardInSelection(cardSelection[0].matchingCard);

        Debug.Log($"Player matched correctly: {playerMatchedSuccessfully}");

        if(playerMatchedSuccessfully)
        {
            //Player can match
            for (int i = 0; i < cardSelection.Length; i++)
            {
                cardGraveyard.AddCardToGraveyard(cardSelection[i]);

                gameManager.RemoveCard(cardSelection[i]);
            };

            StartCoroutine(SwoopCards());
            //
        }
        else
        {
            //Player can unflip cards
            //TEMP (Wait amount will not be fixed or it will be on a button)
            StartCoroutine(UnflipCards());
        }
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

    #region Coroutines

    private IEnumerator SwoopCards()
    {
        canInteract = false;

        yield return new WaitForSeconds(.5f);

        Vector3[] startPositions = new Vector3[cardSelection.Length];
        for (int i = 0; i < cardSelection.Length; i++)
        {
            cardSelection[i].isInteractable = false;
            startPositions[i] = cardSelection[i].transform.position;
        }
        
        float timer = 0;
        while(timer < cardSwoopTime)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < cardSelection.Length; i++)
                cardSelection[i].transform.position = Vector3.Lerp(startPositions[i], cardGraveyard.transform.position, cardSwoopCurve.Evaluate(timer / cardSwoopTime));

            yield return null;
        }

        cardSelection = new CardObject[2];

        canInteract = true;
    }

    private IEnumerator UnflipCards()
    {
        canInteract = false;

        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < cardSelection.Length; i++)
        {
            cardSelection[i].SetFlip(false);
        }

        cardSelection = new CardObject[2];

        canInteract = true;
    }

    #endregion
}
