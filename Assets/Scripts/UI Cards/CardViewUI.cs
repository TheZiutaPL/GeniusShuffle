using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardViewUI : MonoBehaviour
{
    [SerializeField] private Animator cardViewUIAnim;
    [SerializeField] private Button cardViewCloseButton;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextPagesHandler descriptionHandler;
    private const string UI_SHOW_ANIM_KEY = "show";

    private void Awake()
    {
        cardViewCloseButton.onClick.AddListener(() => CardView.HideCardView());
    }

    public void SetCardViewUI(CardData cardData)
    {
        cardNameText.SetText(cardData.GetCardName());
        descriptionHandler.SetText(cardData.GetCardDescription());
    }

    public void ShowUI(bool show) => cardViewUIAnim.SetBool(UI_SHOW_ANIM_KEY, show);
}
