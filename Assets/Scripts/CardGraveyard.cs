using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGraveyard : MonoBehaviour
{
    private List<CardData> cardGraveyard = new List<CardData>();
    [SerializeField] private float travelTime = .3f;
    [SerializeField] private AnimationCurve cardTravelCurve;
    [SerializeField] private InteractionHandler graveyardInteraction;

    [Header("Visuals")]
    [SerializeField] private Animator graveyardAnimator;
    [SerializeField] private Transform graveyardMeshTransform;
    [SerializeField] private float sizePerCard = 0.1f;
    private float graveyardMeshScale = 0;

    private const string GRAVEYARD_CLEAR_ANIM = "EmptyGraveyard";

    private void Awake()
    {
        graveyardMeshTransform.gameObject.SetActive(graveyardMeshScale != 0);
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

    private void SetGraveyardScale(float scale = 0)
    {
        graveyardMeshScale = scale;

        graveyardMeshTransform.gameObject.SetActive(graveyardMeshScale != 0);
    }

    public void AddCardToGraveyard(CardObject card) 
    {
        cardGraveyard.Add(card.cardData);

        card.GetComponent<InteractionHandler>().isInteractable = false;

        StartCoroutine(GoToGraveyard(card.transform));
    }

    public void ClearGraveyard()
    {
        cardGraveyard.Clear();

        graveyardAnimator.Play(GRAVEYARD_CLEAR_ANIM);
    }

    public void EnableGraveyard() => graveyardInteraction.isInteractable = true;
    public void DisableGraveyard() => graveyardInteraction.isInteractable = false;

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

        AddGraveyardMeshSize();

        Destroy(card.gameObject);
    }
}
