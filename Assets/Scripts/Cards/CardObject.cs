using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    private CardData cardData;
    public CardObject matchingCard { get; private set; }

    [HideInInspector] public bool isInteractable = true;

    [SerializeField] private Animator cardAnimator;
    private const string CARD_HOVER_KEY = "hover";
    private const string CARD_FLIP_KEY = "flip";

    [SerializeField] private SpriteRenderer cardSpriteRenderer;

    public void SetCardData(CardData cardData)
    {
        this.cardData = cardData;

        RefreshCardDisplay();
    }

    public void SetMatchingCard(CardObject matchingCard) => this.matchingCard = matchingCard;

    public void RefreshCardDisplay()
    {
        cardSpriteRenderer.sprite = cardData.cardSprite;
    }

    #region Visual

    public void SetHover(bool hover) => cardAnimator.SetBool(CARD_HOVER_KEY, hover);

    public void SetFlip(bool flip) => cardAnimator.SetBool(CARD_FLIP_KEY, flip);

    #endregion
}
