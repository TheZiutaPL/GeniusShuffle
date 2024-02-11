using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    private CardData cardData;
    private Color innerBackgroundColor;
    private Color outerBackgroundColor;

    [SerializeField] private RawImage cardImage;

    private void Awake()
    {
        cardImage.material = Instantiate(cardImage.material);
    }

    public void SetCardData(CardData cardData, Color innerColor, Color outerColor)
    {
        this.cardData = cardData;

        cardImage.texture = cardData.cardTexture;

        cardImage.material.SetTexture(CardObject.FACE_TEXTURE_KEY, cardData.cardTexture);

        innerBackgroundColor = innerColor;
        outerBackgroundColor = outerColor;

        cardImage.material.SetColor(CardObject.BACKGROUND_INNER_COLOR_KEY, innerColor);
        cardImage.material.SetColor(CardObject.BACKGROUND_OUTER_COLOR_KEY, outerColor);
    }

    public void ClickAction()
    {
        MultiCardInspection.SelectDisplayedCard((cardData, innerBackgroundColor, outerBackgroundColor));
    }
}
