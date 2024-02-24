using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public CardData cardData { get; private set; }
    public CardObject matchingCard { get; private set; }

    public Color innerBackgroundColor { get; private set; }
    public Color outerBackgroundColor { get; private set; }

    [SerializeField] private Animator cardAnimator;
    [SerializeField] private Transform realCardTransform;
    private const string CARD_HOVER_KEY = "hover";
    private const string CARD_FLIP_KEY = "flip";

    [SerializeField] private ParticleSystem selectionParticles;
    public ParticleSystem successfullMatchParticles;

    [Header("Display")]
    [SerializeField] private MeshRenderer cardMeshRenderer;
    [SerializeField] private int faceMaterialIndex = 2;

    public const string FACE_TEXTURE_KEY = "_MainTex";
    public const string BACKGROUND_INNER_COLOR_KEY = "_BgInnerColor";
    public const string BACKGROUND_OUTER_COLOR_KEY = "_BgOuterColor";

    [Header("Sounds")]
    [SerializeField] private AudioGroup audioGroup;
    //[SerializeField] private AudioClip cardHoverClip;
    //[SerializeField] private AudioClip cardUnHoverClip;
    //[Space(5)]
    //[SerializeField] private AudioClip cardFlipClip;

    public Transform GetRealCardTransform() => realCardTransform;

    public void SetCardData(CardData cardData, Color innerColor, Color outerColor)
    {
        this.cardData = cardData;

        cardMeshRenderer.materials[faceMaterialIndex].SetTexture(FACE_TEXTURE_KEY, cardData.cardTexture);

        innerBackgroundColor = innerColor;
        outerBackgroundColor = outerColor;

        cardMeshRenderer.materials[faceMaterialIndex].SetColor(BACKGROUND_INNER_COLOR_KEY, innerColor);
        cardMeshRenderer.materials[faceMaterialIndex].SetColor(BACKGROUND_OUTER_COLOR_KEY, outerColor);
    }

    public void SetMatchingCard(CardObject matchingCard) => this.matchingCard = matchingCard;

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

        if (hover)
        {
            selectionParticles.Play();
            audioGroup.PlayAudio(0);
        }
        else
        {
            selectionParticles.Stop();
            audioGroup.PlayAudio(1);
        }
    }

    public void SetFlip(bool flip)
    {
        cardAnimator.SetBool(CARD_FLIP_KEY, flip);

        audioGroup.PlayAudio(2);
    }

    public void EnableRenderer(bool enable) => cardMeshRenderer.enabled = enable;
    #endregion
}
