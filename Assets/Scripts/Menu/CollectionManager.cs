using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CampaignLevel
{
    [SerializeField] private string name;
    public GameObject levelObject;
    public List<CardCollection> cardCollections;
    public int cardPairsInGame;

    private PlayerStats levelStats = null;
    public PlayerStats LevelStats { get => levelStats; set => levelStats = value; }

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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ActivateLevels()
    {
        int finishedLevels = GetFinishedLevels();
        for (int i = 0; i < levels.Count; i++)
            levels[i].levelObject.SetActive(i < finishedLevels + 1);
    }

    public void SelectLevel(int level)
    {
        CampaignLevel campaignLevel = levels[level];
        gameManager.SetStartCollections(campaignLevel.cardCollections);
        gameManager.SetCardPairsCount(campaignLevel.GetPairCount());

        gameManager.levelIndex = level;

        if (campaignLevel.LevelStats != null)
            Debug.Log($"This level best time is {levels[level].LevelStats.playedTime}");
        else
            Debug.Log($"You have no best time yet");

        gameManager.InitializeGameStart(level);
    }

    public static void SetLevelStat(int levelIndex, PlayerStats playerStats) 
    {
        CampaignLevel campaignLevel = instance.levels[levelIndex];
        if (campaignLevel.LevelStats == null)
            campaignLevel.LevelStats = playerStats;
        else
            campaignLevel.LevelStats = playerStats.playedTime >= campaignLevel.LevelStats.playedTime ? campaignLevel.LevelStats : playerStats;

        SaveManager.Save(SaveManager.LevelsToData());
    }
    public static void SetLevelStats(PlayerStats[] playerStats)
    {
        for (int i = 0; i < playerStats.Length; i++)
            instance.levels[i].LevelStats = playerStats[i];
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
        cardPairsAmountSlider.SetMaxValue(pairs);

        if (collectionsAmountText != null)
            collectionsAmountText.text = selectedCollections.Count.ToString();
        if (pairsAmountText != null)
            pairsAmountText.text = pairs.ToString();
    }

    public int GetCollectionsAmount() => selectedCollections.Count;

    public int GetFinishedLevels()
    {
        int i;
        for (i = 0; i < levels.Count; i++)
        {
            if (levels[i].LevelStats == null)
                break;
        }

        return i;
    }
}
