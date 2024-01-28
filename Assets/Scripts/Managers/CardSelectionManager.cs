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

    private bool playerMatched;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private UnityEngine.UI.Button nextTurnButton;
    private bool success;

    private void Start()
    {
        nextTurnButton.onClick.AddListener(() => NextTurn(success));
        nextTurnButton.gameObject.SetActive(false);
    }

    public static void AddCardToSelection(CardObject cardObject)
    {
        if (cardObject == null && !instance.playerMatched)
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

        if (instance.playerMatched)
            return;

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
        instance.playerMatched = true;

        yield return new WaitForSeconds(instance.matchWaitTime);        

        if (success)
            for (int i = 0; i < cardSelection.Length; i++)
                cardSelection[i].successfullMatchParticles.Play();

        instance.success = success;

        instance.nextTurnButton.gameObject.SetActive(true);
    }

    public static void NextTurn(bool success)
    {
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

        instance.nextTurnButton.gameObject.SetActive(false);

        ClearSelection();
        instance.playerMatched = false;
    }
}
