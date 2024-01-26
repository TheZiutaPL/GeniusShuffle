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

    private CardData selectedCard;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        PoolObjects();
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

    public static void SelectDisplayedCard(CardData cardData)
    {
        instance.selectedCard = cardData;
        instance.displayCard.SetCardData(cardData);

        instance.nameText.SetText(cardData.cardName);
        instance.descriptionText.SetText(cardData.cardDescription);

        instance.displayedCardGroup.SetActive(true);
        instance.displayAnimator.SetBool(SHOW_ANIMATION_KEY, true);
    }

    private void UnselectDisplayedCard()
    {
        instance.displayAnimator.SetBool(SHOW_ANIMATION_KEY, false);
        displayedCardGroup.SetActive(false);
    }

    private void ClearCardObjects()
    {
        for (int i = 0; i < uiCardObjects.Count; i++)
            Destroy(uiCardObjects[i].gameObject);

        uiCardObjects.Clear();
    }

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

    private static UICard CreateNewCard(CardData cardData)
    {
        UICard temp = Instantiate(instance.uiCardPrefab, instance.uiCardParent);

        if(cardData != null)
            temp.SetCardData(cardData);

        return temp;
    }
}
