using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGraveyard : MonoBehaviour
{
    private List<(CardData, Color, Color)> cardGraveyard = new List<(CardData, Color, Color)>();
    [SerializeField] private float travelTime = .3f;
    [SerializeField] private AnimationCurve cardTravelCurve;
    [SerializeField] private InteractionHandler graveyardInteraction;

    [Header("Visuals")]
    [SerializeField] private Animator graveyardAnimator;
    [SerializeField] private Transform graveyardMeshTransform;
    [SerializeField] private float startSize = 0.1f;
    [SerializeField] private float sizePerCard = 0.1f;
    private float graveyardMeshScale = 0;

    private const string GRAVEYARD_CLEAR_ANIM = "EmptyGraveyard";

    [SerializeField] private float indentYOffsetAmount = 0.01f;
    private int cardYOffsetIndent;

    private void Awake()
    {
        graveyardMeshTransform.gameObject.SetActive(graveyardMeshScale != 0);

        SetGraveyardScale();
    }

    public void OpenGraveyardInspection()
    {
        if (cardGraveyard.Count == 0)
            return;

        MultiCardInspection.ShowMultiCardInspection(cardGraveyard.ToArray());
    }

    private void AddGraveyardMeshSize()
    {
        graveyardMeshScale += sizePerCard;

        graveyardMeshTransform.localScale = new Vector3(graveyardMeshTransform.localScale.x, graveyardMeshTransform.localScale.y, graveyardMeshScale);

        graveyardMeshTransform.gameObject.SetActive(graveyardMeshScale != 0);
    }

    private void SetGraveyardScale(float scale = -1)
    {
        if (scale < 0) scale = startSize;

        graveyardMeshScale = scale;
        graveyardMeshTransform.localScale = new Vector3(graveyardMeshTransform.localScale.x, graveyardMeshTransform.localScale.y, graveyardMeshScale);

        graveyardMeshTransform.gameObject.SetActive(graveyardMeshScale != 0);
    }

    public void AddCardToGraveyard(CardObject card) 
    {
        cardGraveyard.Add((card.cardData, card.innerBackgroundColor, card.outerBackgroundColor));

        card.GetComponent<InteractionHandler>().SetInteractable(false);

        StartCoroutine(GoToGraveyard(card.transform));
    }

    public void ClearGraveyard()
    {
        cardGraveyard.Clear();

        graveyardAnimator.Play(GRAVEYARD_CLEAR_ANIM);
    }

    public void EnableGraveyard() => graveyardInteraction.SetInteractable(true);
    public void DisableGraveyard() => graveyardInteraction.SetInteractable(false);

    IEnumerator GoToGraveyard(Transform card)
    {
        float indentYOffset = cardYOffsetIndent * indentYOffsetAmount;
        cardYOffsetIndent++;

        Vector3 startPos = card.position;
        Vector3 endPos = graveyardMeshTransform.position + new Vector3(0, indentYOffset, 0);

        float timer = 0;
        while(timer < travelTime)
        {
            timer += Time.deltaTime;

            card.position = Vector3.Lerp(startPos, endPos, cardTravelCurve.Evaluate(timer / travelTime));

            yield return null;
        }

        AddGraveyardMeshSize();

        Destroy(card.gameObject);
        cardYOffsetIndent--;
    }
}
