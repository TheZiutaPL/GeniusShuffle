using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CampaignLevel
{
    [SerializeField] private string name;

    [SerializeField] internal GameObject levelObject;
    [SerializeField] internal TextMeshPro levelTimeText;
    [SerializeField] internal SpriteRenderer levelMedal;

    [Header("Level Settings")]
    public List<CardCollection> cardCollections;
    public int cardPairsInGame;

    private PlayerStats levelStats = null;
    public PlayerStats LevelStats { get => levelStats; set => levelStats = value; }

    private PlayerStats challengeStats = null;
    public PlayerStats ChallengeStats { get => challengeStats; set => challengeStats = value; }

    public int GetPairCount()
    {
        return cardPairsInGame > 0 ? cardPairsInGame : GetTotalPairCount();
    }

    public int GetTotalPairCount()
    {
        int pairCount = 0;

        for (int i = 0; i < cardCollections.Count; i++)
            pairCount += cardCollections[i].GetPairCount();

        return pairCount;
    }
}

public class CollectionManager : MonoBehaviour
{
    private static CollectionManager instance;

    [Header("Campaign")]
    [SerializeField] List<CampaignLevel> levels;

    [Header("Free Play")]
    [SerializeField] List<CardCollection> selectedCollections = new List<CardCollection>();

    [Space(10)]
    [SerializeField] MenuSlider cardPairsAmountSlider;
    [SerializeField] TMP_Text collectionsAmountText;
    [SerializeField] TMP_Text pairsAmountText;
    [SerializeField] GameManager gameManager;

    private enum LevelMode 
    {
        Standard,
        Challenge,
    }
    private LevelMode levelMode;
    public void SetLevelMode(int modeIndex)
    {
        levelMode = (LevelMode)modeIndex;
        ActivateLevels();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void ActivateLevels()
    {
        int finishedLevels = GetFinishedLevels();
        for (int i = 0; i < instance.levels.Count; i++)
        {
            CampaignLevel campaignLevel = instance.levels[i];
            switch (instance.levelMode)
            {
                case LevelMode.Standard:
                    campaignLevel.levelObject.SetActive(i > finishedLevels);
                    campaignLevel.levelTimeText.SetText(campaignLevel.LevelStats != null ? campaignLevel.LevelStats.playedTime.ToString("#.#s") : "");
                    campaignLevel.levelMedal.sprite = campaignLevel.LevelStats != null ? PlayerScoreManager.GetMedalSprite(campaignLevel.LevelStats.GetMedalIndex()) : PlayerScoreManager.GetEmptyMedalSprite();
                    break;

                case LevelMode.Challenge:
                    campaignLevel.levelObject.SetActive(i >= finishedLevels);
                    campaignLevel.levelTimeText.SetText(campaignLevel.ChallengeStats != null ? campaignLevel.ChallengeStats.playedTime.ToString("#.#s") : "");
                    campaignLevel.levelMedal.sprite = campaignLevel.ChallengeStats != null ? PlayerScoreManager.GetMedalSprite(campaignLevel.ChallengeStats.GetMedalIndex()) : PlayerScoreManager.GetEmptyMedalSprite();
                    break;
            }
        } 
    }

    public void SelectLevel(int level)
    {
        if (levelMode switch
        {
            LevelMode.Standard => level > GetFinishedLevels(),
            LevelMode.Challenge => level >= GetFinishedLevels(),
            _ => throw new System.NotImplementedException(),
        })
            return;

        CampaignLevel campaignLevel = levels[level];
        gameManager.SetStartCollections(campaignLevel.cardCollections);
        gameManager.SetCardPairsCount(campaignLevel.GetPairCount());

        gameManager.levelIndex = level;

        if (campaignLevel.LevelStats != null)
            Debug.Log($"This level best time is {levels[level].LevelStats.playedTime}");
        else
            Debug.Log($"You have no best time yet");

        gameManager.InitializeGameStart(level, (int)levelMode);
    }

    public static void SetLevelStat(int levelIndex, int levelMode, PlayerStats playerStats) 
    {
        CampaignLevel campaignLevel = instance.levels[levelIndex];

        PlayerStats currentStats = levelMode switch
        {
            0 => campaignLevel.LevelStats,
            1 => campaignLevel.ChallengeStats,
            _ => throw new System.NotImplementedException(),
        };

        if (currentStats == null)
            currentStats = playerStats;
        else
            currentStats = playerStats.playedTime >= campaignLevel.LevelStats.playedTime ? campaignLevel.LevelStats : playerStats;

        switch (levelMode)
        {
            case 0:
                campaignLevel.LevelStats = currentStats;
                break;

            case 1:
                campaignLevel.ChallengeStats = currentStats;
                break;
        }

        SaveManager.Save(SaveManager.LevelsToData());
    }
    public static void SetLevelStats(PlayerStats[] levelStats, PlayerStats[] challangeStats)
    {
        for (int i = 0; i < instance.levels.Count; i++) 
        { 
            if(i < levelStats.Length)
                instance.levels[i].LevelStats = levelStats[i];

            if(i < challangeStats.Length)
                instance.levels[i].ChallengeStats = challangeStats[i];
        }
    }
    public static List<CampaignLevel> GetLevels() => instance.levels;

    [ContextMenu("Apply Collections")]
    public void ApplyCollections()
    {
        gameManager.SetStartCollections(selectedCollections);
        UpdateFreePlayCardPairs();
    }

    public void SelectCollection(CardCollection collection) => selectedCollections.Add(collection);
    public void UnselectCollection(CardCollection collection) => selectedCollections.Remove(collection);

    void UpdateFreePlayCardPairs()
    {
        int pairs = 0;
        foreach (CardCollection collection in selectedCollections)
        {
            pairs += collection.GetPairCount();
        }
        cardPairsAmountSlider.SetMaxValue(Mathf.Min(pairs, GameManager.MAX_CARDS_IN_GAME));

        if (collectionsAmountText != null)
            collectionsAmountText.text = selectedCollections.Count.ToString();
        if (pairsAmountText != null)
            pairsAmountText.text = pairs.ToString();
    }

    public int GetCollectionsAmount() => selectedCollections.Count;

    public static int GetFinishedLevels()
    {
        int i;
        for (i = 0; i < instance.levels.Count; i++)
        {
            if (instance.levels[i].LevelStats == null)
                break;
        }

        return i;
    }
}
