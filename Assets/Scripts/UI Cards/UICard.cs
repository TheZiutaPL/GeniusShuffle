using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    private CardData cardData;

    [SerializeField] private RawImage cardImage;

    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;

        RefreshDisplay();
    }

    public void ClickAction()
    {
        MultiCardInspection.SelectDisplayedCard(cardData);
    }

    private void RefreshDisplay()
    {
        cardImage.texture = cardData.cardSprite;
    }
}
