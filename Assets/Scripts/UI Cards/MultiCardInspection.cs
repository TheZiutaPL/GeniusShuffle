using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiCardInspection : MonoBehaviour
{
    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private Animator animator;
    private const string SHOW_ANIMATION_KEY = "show";

    private static MultiCardInspection instance;

    [Header("Pooling Settings")]
    [SerializeField] private Transform uiCardParent;
    [SerializeField] private UICard uiCardPrefab;
    [SerializeField] private int prePooledObjects;
    private List<UICard> uiCardObjects = new ();

    [Header("Inspected Cards Scroll")]
    [SerializeField] private ScrollRect scrollRect;

    [Header("Selected Card Display")]
    [SerializeField] private GameObject displayedCardGroup;
    [SerializeField] private Animator displayAnimator;
    [SerializeField] private UICard displayCard;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private GameObject descriptionPagesTooltip;
    [SerializeField] private TextMeshProUGUI descriptionPagesText;
    [SerializeField] private Button leftPageButton;
    [SerializeField] private Button rightPageButton;
    private int currentPage = 1;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        PoolObjects();

        leftPageButton.onClick.AddListener(PrevPage);
        rightPageButton.onClick.AddListener(NextPage);
    }

    public static void ShowMultiCardInspection(CardData[] cardDatas)
    {
        instance.UnselectDisplayedCard();
        instance.scrollRect.normalizedPosition = new Vector2(0, 0);

        instance.interactionManager.EnableInteractions(false);

        instance.animator.SetBool(SHOW_ANIMATION_KEY, true);

        int index = 0;
        while (index < cardDatas.Length)
        {
            if (index < instance.uiCardObjects.Count)
            {
                instance.uiCardObjects[index].gameObject.SetActive(true);
                instance.uiCardObjects[index].SetCardData(cardDatas[index]);
            }
            else
                CreateNewCard(cardDatas[index]);

            index++;
        }

        while (index < instance.uiCardObjects.Count)
        {
            instance.uiCardObjects[index].gameObject.SetActive(false);

            index++;
        }        
    }

    public static void HideMultiCardInspection()
    {
        instance.interactionManager.EnableInteractions(true);
        instance.animator.SetBool(SHOW_ANIMATION_KEY, false);
    }    

    #region Selected Card Display
    public static void SelectDisplayedCard(CardData cardData)
    {
        instance.displayCard.SetCardData(cardData);

        instance.nameText.SetText(cardData.GetCardName());
        instance.descriptionText.SetText(cardData.GetCardDescription());

        instance.StartCoroutine(InvokeNextFrame(() => instance.SetDescriptionPage()));

        instance.displayedCardGroup.SetActive(true);
        instance.displayAnimator.SetBool(SHOW_ANIMATION_KEY, true);
    }

    private void UnselectDisplayedCard()
    {
        instance.displayAnimator.SetBool(SHOW_ANIMATION_KEY, false);
        displayedCardGroup.SetActive(false);
    }
    #endregion

    #region Description Pages
    private void SetDescriptionPage(int page = 1)
    {
        int pageCount = descriptionText.textInfo.pageCount;
        currentPage = Mathf.Clamp(page, 1, pageCount);

        bool multiplePages = pageCount > 1;

        descriptionPagesTooltip.SetActive(multiplePages);

        if (!multiplePages)
            return;

        descriptionPagesText.SetText($"({currentPage}/{pageCount})");

        descriptionText.pageToDisplay = currentPage;

        leftPageButton.interactable = currentPage > 1;
        rightPageButton.interactable = currentPage < pageCount;
    }

    private void PrevPage() => SetDescriptionPage(currentPage - 1);

    private void NextPage() => SetDescriptionPage(currentPage + 1);
    #endregion

    #region Object Pooling
    private void PoolObjects()
    {
        ClearCardObjects();

        for (int i = 0; i < prePooledObjects; i++)
        {
            UICard temp = CreateNewCard(null);
            temp.gameObject.SetActive(false);
            uiCardObjects.Add(temp);
        }
    }

    private void ClearCardObjects()
    {
        for (int i = 0; i < uiCardObjects.Count; i++)
            Destroy(uiCardObjects[i].gameObject);

        uiCardObjects.Clear();
    }

    private static UICard CreateNewCard(CardData cardData)
    {
        UICard temp = Instantiate(instance.uiCardPrefab, instance.uiCardParent);

        if(cardData != null)
            temp.SetCardData(cardData);

        return temp;
    }
    #endregion

    private static IEnumerator InvokeNextFrame(System.Action action)
    {
        yield return new WaitForEndOfFrame();

        action?.Invoke();
    }
}
