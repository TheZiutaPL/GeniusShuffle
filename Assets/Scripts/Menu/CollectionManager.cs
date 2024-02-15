using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    [Header("Campaign")]
    [SerializeField] List<CardCollection> levels;
    [SerializeField] Transform levelSelectionHolder;
    List<GameObject> levelSelectionButtons = new List<GameObject>();
    public int finishedLevels = 0;
    public static string FINISHED_LEVELS_KEY="FINISHED_LEVLES";

    [Header("Free Play")]
    [SerializeField] List<CardCollection> selectedCollections = new List<CardCollection>();

    [Space(10)]
    [SerializeField] MenuSlider cardPairsAmountSlider;
    [SerializeField] GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform t in levelSelectionHolder)
        {
            levelSelectionButtons.Add(t.gameObject);
        }

        finishedLevels = PlayerPrefs.GetInt(FINISHED_LEVELS_KEY, 0);

        RefreshUnlockedLevels();
    }

    public void SelectLevel(int level)
    {
        gameManager.SetStartCollections(levels[level]);
        gameManager.SetCardPairsCount(levels[level].GetCardMatches().Count);
    }

    [ContextMenu("Apply Collections")]
    public void ApplyCollections()
    {
        gameManager.SetStartCollections(selectedCollections);
        UpdateFreePlayCardPairs();
    }

    public void SelectCollection(CardCollection collection) => selectedCollections.Add(collection);
    public void UnselectCollection(CardCollection collection) => selectedCollections.Remove(collection);

    [ContextMenu("Refresh Unlocked Levels")]
    void RefreshUnlockedLevels()
    {
        for (int i = 0; i < levelSelectionButtons.Count; i++)
        {
            levelSelectionButtons[i].SetActive(i <= finishedLevels);
        }
    }

    void UpdateFreePlayCardPairs()
    {
        int pairs = 0;
        foreach (CardCollection collection in selectedCollections)
        {
            pairs += collection.GetCardMatches().Count;
        }
        cardPairsAmountSlider.SetMaxValue(pairs);
    }

    public int GetCollectionsAmount() => selectedCollections.Count;
}
