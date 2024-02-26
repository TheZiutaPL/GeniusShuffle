using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private PointBasedMovement cameraMovement;
    [SerializeField] private Transform gamePositionTransform;
    [SerializeField] private Transform levelSelectionTransform;
    [SerializeField] private Animator restartObject;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private AudioGroup incorrectChoiceSounds;

    private const string RESTART_OBJECT_SHOW_KEY = "show";

    [Header("Game Settings")]
    [SerializeField] private bool startGameByDefault = true;
    [SerializeField, Min(1)] private int cardPairsCount = 1;
    public void SetCardPairsCount(int newCardPairs) => cardPairsCount = newCardPairs;
    public void SetCardPairsCount(float newCardPairs) => cardPairsCount = Mathf.RoundToInt(newCardPairs);

    [HideInInspector] public int levelIndex = -1;
    [HideInInspector] public int levelMode = -1;

    [Header("Card Setup")]
    [SerializeField] private CardObject cardObjectPrefab;
    [SerializeField] private List<CardCollection> cardCollections;
    [SerializeField] private Color defaultInnerColor;
    [SerializeField] private Color defaultOuterColor;
    public void SetStartCollections(List<CardCollection> newCardCollections) 
    { 
        cardCollections = newCardCollections;
        ClearCards();
    }
    public void SetStartCollections(CardCollection newCardCollections)
    {
        cardCollections.Clear();
        cardCollections.Add(newCardCollections);
        ClearCards();
    }
    [SerializeField] private float cardScale = .2f;

    [Header("Table Setup")]
    [SerializeField] private float cardY;
    [SerializeField] private int colToRowRatio = 3;
    [SerializeField] private Vector2 maxOffset;
    [SerializeField] private Transform topRightCorner;
    [SerializeField] private Transform bottomLeftCorner;
    [SerializeField] private CardGraveyard cardGraveyard;

    private List<CardObject> cards = new List<CardObject>();
    public bool IsGameStarted() => cards.Count > 0;

    [Header("Start Animation")]
    [SerializeField] private Transform startCardsPosition;
    [SerializeField] private float cardLayoutDelay = 0.15f;
    [SerializeField] private float cardTraversalTime = .25f;
    [SerializeField] private AudioGroup cardDrawAudioGroup;
    private Coroutine startAnimation;

    private void Start()
    {
        if(startGameByDefault) 
            StartGame();
    }

    public void InitializeGameStart() => InitializeGameStart(-1, 0);

    public void InitializeGameStart(int level = -1, int mode = -1)
    {
        if (cardCollections == null || cardCollections.Count <= 0 || cardPairsCount <= 0)
        {
            Debug.LogWarning("Game cannot start, because cardCollections is empty");
            incorrectChoiceSounds.PlayAudio();
            return;
        }

        cameraMovement.MoveTo(gamePositionTransform, () => StartGame(level, mode));
    }

    public void StartGame() => StartGame(levelIndex, levelMode);

    [ContextMenu("Start Game")]
    public void StartGame(int level = -1, int mode = -1)
    {
        restartObject.SetBool(RESTART_OBJECT_SHOW_KEY, false);

        levelIndex = level;
        levelMode = mode;
        

        List<CardMatch> matches = GetGameMatches(cardCollections);

        CardSelectionManager.SetGame();
        cardGraveyard.ClearGraveyard();

        ClearCards();

        foreach (CardMatch match in matches)
        {
            Color innerColor = levelMode == 0 ? match.innerBackgroundColor : defaultInnerColor;
            Color outerColor = levelMode == 0 ? match.outerBackgroundColor : defaultOuterColor;

            CardObject inventorCard = CreateCardObject(match.inventorCard, innerColor, outerColor);
            CardObject inventionCard = CreateCardObject(match.GetRandomInventionCard(), innerColor, outerColor);

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

        InteractionManager.instance.EnableInteractions(false);

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
                        cardDrawAudioGroup.PlayAudio();

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

        PlayerScoreManager.StartGame();

        InteractionManager.instance.EnableInteractions(true);

        tutorialManager.ShowTutorial();

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
        //TODO
        PlayerStats stats = PlayerScoreManager.EndGame();

        if(levelIndex >= 0)
            CollectionManager.SetLevelStat(levelIndex, levelMode, stats);

        Debug.Log($"Earned {stats.GetMedalIndex()} medal in {stats.playedTime} seconds");

        yield return new WaitForSeconds(1f);
        cardGraveyard.ClearGraveyard();

        if (levelIndex >= 0)
        {
            cameraMovement.MoveTo(levelSelectionTransform);
            CollectionManager.ActivateLevels();
        }
        else
            restartObject.SetBool(RESTART_OBJECT_SHOW_KEY, true);
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

    private CardObject CreateCardObject(CardData cardData, Color innerColor, Color outerColor)
    {
        CardObject cardObject = Instantiate(cardObjectPrefab);
        cardObject.SetCardData(cardData, innerColor, outerColor);

        return cardObject;
    }

    private List<CardMatch> GetGameMatches(List<CardCollection> cardCollections)
    {
        List<CardMatch> availableMatches = new List<CardMatch>();
        foreach (CardCollection cardCollection in cardCollections)
            availableMatches.AddRange(cardCollection.GetCardMatches());

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
