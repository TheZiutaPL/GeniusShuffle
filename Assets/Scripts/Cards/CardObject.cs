using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public CardData cardData { get; private set; }
    public CardObject matchingCard { get; private set; }

    [SerializeField] private Animator cardAnimator;
    [SerializeField] private Transform realCardTransform;
    private const string CARD_HOVER_KEY = "hover";
    private const string CARD_FLIP_KEY = "flip";

    [SerializeField] private SpriteRenderer cardSpriteRenderer;

    [Header("Sounds")]
    [SerializeField] private AudioClip cardHoverClip;
    [SerializeField] private AudioClip cardUnHoverClip;
    [Space(5)]
    [SerializeField] private AudioClip cardFlipClip;

    public Transform GetRealCardTransform() => realCardTransform;

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

    public void ClickAction()
    {
        if (CardSelectionManager.IsCardInSelection(this))
        {
            //Show card view
            Debug.Log("Card View");
            CardView.ShowCardView(this);

            return;
        }
        else
            CardSelectionManager.AddCardToSelection(this);
    }

    #region Visual

    public void SetHover(bool hover)
    {
        cardAnimator.SetBool(CARD_HOVER_KEY, hover);

        AudioManager.PlaySound(hover ? cardHoverClip : cardUnHoverClip);
    }

    public void SetFlip(bool flip)
    {
        cardAnimator.SetBool(CARD_FLIP_KEY, flip);

        AudioManager.PlaySound(cardFlipClip);
    }
    #endregion
}
