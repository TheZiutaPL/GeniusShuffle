using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    private CardData cardData;
    private Color innerBackgroundColor;
    private Color outerBackgroundColor;

    [SerializeField] private RawImage cardBackground;
    [SerializeField] private RawImage cardImage;

    public void SetCardData(CardData cardData, Color innerColor, Color outerColor)
    {
        this.cardData = cardData;

        cardImage.texture = cardData.cardTexture;

        innerBackgroundColor = innerColor;
        outerBackgroundColor = outerColor;

        cardBackground.color = Color.Lerp(outerBackgroundColor, innerBackgroundColor, 0.5f);
    }

    public void ClickAction()
    {
        MultiCardInspection.SelectDisplayedCard((cardData, innerBackgroundColor, outerBackgroundColor));
    }
}
