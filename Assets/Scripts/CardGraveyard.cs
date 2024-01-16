using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGraveyard : MonoBehaviour
{
    private List<CardData> cardGraveyard = new List<CardData>();
    [SerializeField] private float travelTime = .3f;
    [SerializeField] private AnimationCurve cardTravelCurve;
    public void AddCardToGraveyard(CardObject card) 
    {
        cardGraveyard.Add(card.cardData);

        card.GetComponent<InteractionHandler>().isInteractable = false;

        StartCoroutine(GoToGraveyard(card.transform));
    }

    IEnumerator GoToGraveyard(Transform card)
    {
        Vector3 startPos = card.position;

        float timer = 0;
        while(timer < travelTime)
        {
            timer += Time.deltaTime;

            card.position = Vector3.Lerp(startPos, transform.position, cardTravelCurve.Evaluate(timer / travelTime));

            yield return null;
        }

        Destroy(card.gameObject);
    }
}
