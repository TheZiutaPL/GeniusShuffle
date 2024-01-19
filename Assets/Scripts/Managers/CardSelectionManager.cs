using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    private static CardSelectionManager instance;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private CardGraveyard cardGraveyard;
    private static CardObject[] cardSelection = new CardObject[2];

    [SerializeField] private float matchWaitTime = .5f;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void AddCardToSelection(CardObject cardObject)
    {
        if (cardObject == null)
            return;

        for (int i = 0; i < cardSelection.Length; i++)
        {
            if (cardSelection[i] != null)
                continue;

            cardSelection[i] = cardObject;

            cardObject.SetFlip(true);

            if (i < cardSelection.Length - 1)
                return;
        }

        //Check for winning condition (match)
        bool playerMatchedSuccessfully = IsCardInSelection(cardSelection[0].matchingCard);

        Debug.Log($"Player matched correctly: {playerMatchedSuccessfully}");

        instance.StartCoroutine(MatchCoroutine(playerMatchedSuccessfully));
    }

    public static bool IsCardInSelection(CardObject cardObject)
    {
        for (int i = 0; i < cardSelection.Length; i++)
        {
            if (cardSelection[i] == cardObject)
                return true;
        }

        return false;
    }

    private static void ClearSelection() => cardSelection = new CardObject[2];

    private static IEnumerator MatchCoroutine(bool success)
    {
        if (success)
        {
            yield return new WaitForSeconds(instance.matchWaitTime);

            for (int i = 0; i < cardSelection.Length; i++)
                cardSelection[i].successfullMatchParticles.Play();
        }

        yield return new WaitForSeconds(instance.matchWaitTime);

        instance.interactionManager.EnableInteractions(false);

        if (success)
        {
            for (int i = 0; i < cardSelection.Length; i++)
            {
                instance.cardGraveyard.AddCardToGraveyard(cardSelection[i]);

                instance.gameManager.RemoveCard(cardSelection[i]);
            };
        }
        else
            for (int i = 0; i < cardSelection.Length; i++)
                cardSelection[i].SetFlip(false);

        instance.interactionManager.EnableInteractions(true);

        ClearSelection();
    }
}
