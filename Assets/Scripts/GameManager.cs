using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InteractionManager interactionManager;

    [Header("Game Settings")]
    [SerializeField] private bool startGameByDefault = true;
    [SerializeField, Min(1)] private int cardPairsCount = 1;

    [Header("Card Setup")]
    [SerializeField] private CardObject cardObjectPrefab;
    [SerializeField] private CardCollection cardCollection;
    [SerializeField] private float cardScale = .2f;

    [Header("Table Setup")]
    [SerializeField] private float cardY;
    [SerializeField] private int colToRowRatio = 3;
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private Transform topRightCorner;
    [SerializeField] private Transform bottomLeftCorner;

    private List<CardObject> cards = new List<CardObject>();

    [Header("Start Animation")]
    [SerializeField] private Transform startCardsPosition;
    [SerializeField] private float cardLayoutDelay = 0.15f;
    [SerializeField] private float cardTraversalTime = .25f;
    [SerializeField] private AudioClip[] startAnimationCardSound = new AudioClip[0];
    private Coroutine startAnimation;

    private void Start()
    {
        if(startGameByDefault) 
            StartGame();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        if (cardCollection == null)
            return;

        List<CardMatch> matches = GetGameMatches(cardCollection);

        ClearCards();

        foreach (CardMatch match in matches)
        {
            CardObject inventorCard = CreateCardObject(match.inventorCard);
            CardObject inventionCard = CreateCardObject(match.GetRandomInventionCard());

            inventorCard.SetMatchingCard(inventionCard);
            inventionCard.SetMatchingCard(inventorCard);

            cards.Add(inventorCard);
            cards.Add(inventionCard);
        }
        
        List<Vector2> cardPositions = new List<Vector2>(GetTablePositions(cards.Count));

        startAnimation = StartCoroutine(StartGameAnimation(cardPositions));
    }

    private IEnumerator StartGameAnimation(List<Vector2> cardPositions)
    {
        yield return new WaitForSeconds(0.25f);

        interactionManager.EnableInteractions(false);

        List<(CardObject, Vector3)> traversingCards = new List<(CardObject, Vector3)>();

        for (int i = 0; i < cards.Count; i++)
        {
            int posIndex = Random.Range(0, cardPositions.Count);

            traversingCards.Add((cards[i], new Vector3(cardPositions[posIndex].x, cardY, cardPositions[posIndex].y)));

            cards[i].transform.position = startCardsPosition.position;
            cards[i].transform.localScale = new Vector3(cardScale, cardScale, cardScale);

            cardPositions.RemoveAt(posIndex);
        }

        //Moving cards towards their destination
        float timer = 0;
        float animationTime = traversingCards.Count * cardLayoutDelay + cardTraversalTime;
        while (timer < animationTime)
        {
            for (int i = 0; i < traversingCards.Count; i++)
            {
                if (timer >= i * cardLayoutDelay)
                {
                    if (traversingCards[i].Item1.transform.position == startCardsPosition.position)
                        AudioManager.PlaySound(startAnimationCardSound);

                    float blend = Mathf.Clamp01((timer - i * cardLayoutDelay) / cardTraversalTime);
                    traversingCards[i].Item1.transform.position = Vector3.Lerp(startCardsPosition.position, traversingCards[i].Item2, blend);
                }
                else
                    break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        interactionManager.EnableInteractions(true);
    }

    [ContextMenu("Clear")]
    public void ClearCards()
    {
        if (startAnimation != null)
            StopCoroutine(startAnimation);

        for (int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i].gameObject);
        }

        cards.Clear();
    }

    public void RemoveCard(CardObject card)
    {
        if (cards.Contains(card))
            cards.Remove(card);

        if (cards.Count == 0)
            OnLevelFinished();
    }

    private void OnLevelFinished()
    {
        StartCoroutine(LevelFinished());
    }

    private IEnumerator LevelFinished()
    {
        yield return new WaitForSeconds(1.5f);
        StartGame();
    }

    #region Functions

    private Vector2[] GetTablePositions(int cardCount)
    {
        int rows = CalculateRows(cardCount);
        int cols = Mathf.CeilToInt((float)cardCount / rows);

        Debug.Log($"{cols} : {rows}");

        Vector2 tableBounds = new Vector2(topRightCorner.position.x - bottomLeftCorner.position.x, topRightCorner.position.z - bottomLeftCorner.position.z);
        Vector2 tableCenter = new Vector2(topRightCorner.position.x + bottomLeftCorner.position.x, topRightCorner.position.z + bottomLeftCorner.position.z) / 2;

        //Real offset for cards in a given count
        Vector2 offset = new Vector2(Mathf.Clamp(tableBounds.x / cols, 0, maxOffset.x), Mathf.Clamp(tableBounds.y / rows, 0, maxOffset.y));

        //Actual field taken by card placement
        Vector2 calculatedTable = new Vector2((cols - 1) * offset.x, (rows - 1) * offset.y);
        Vector2 halfTableBounds = calculatedTable / 2;

        Vector2[] cardPositions = new Vector2[cardCount];
        int positionIndex = 0;
        for (int i = 0; i < rows; i++)
        {
            float yPosition = Mathf.Lerp(halfTableBounds.y, -halfTableBounds.y, (float)i / Mathf.Max(1, rows - 1));
            for (int j = 0; j < cols; j++)
            {
                float xPosition = Mathf.Lerp(-halfTableBounds.x, halfTableBounds.x, (float)j / Mathf.Max(1, (cols - 1)));
                Vector2 newPos = new Vector2(xPosition, yPosition) + tableCenter;

                if (positionIndex < cardCount)
                    cardPositions[positionIndex] = newPos;
                positionIndex++;
            }
        }

        return cardPositions;
    }

    private int CalculateRows(int cards)
    {
        int rows = 1;
        int cardsProcessed = 0;
        for (int i = 0; i <= cards; i++)
        {
            if (cardsProcessed >= colToRowRatio * rows)
            {
                rows++;
                cardsProcessed = 0;
            }

            cardsProcessed++;
        }

        return rows;
    }

    private CardObject CreateCardObject(CardData cardData)
    {
        CardObject cardObject = Instantiate(cardObjectPrefab);
        cardObject.SetCardData(cardData);

        return cardObject;
    }

    private List<CardMatch> GetGameMatches(CardCollection cardCollection)
    {
        List<CardMatch> availableMatches = cardCollection.GetCardMatches();

        List<CardMatch> chosenMatches = new List<CardMatch>();

        for (int i = 0; i < cardPairsCount; i++)
        {
            if (availableMatches.Count == 0)
                break;

            int chosenIndex = Random.Range(0, availableMatches.Count);
            chosenMatches.Add(availableMatches[chosenIndex]);
            availableMatches.RemoveAt(chosenIndex);
        }

        return chosenMatches;
    }

    #endregion
}
