using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    private CardData cardData;
    public CardObject matchingCard { get; private set; }

    [SerializeField] private SpriteRenderer cardSpriteRenderer;

    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;

        RefreshCardDisplay();
    }

    public void SetMatchingCard(CardObject matchingCard)
    {
        this.matchingCard = matchingCard;
    }

    public void RefreshCardDisplay()
    {
        cardSpriteRenderer.sprite = cardData.cardSprite;
    }
}
