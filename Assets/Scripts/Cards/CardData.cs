using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlawareStudios.Translation;

[CreateAssetMenu(fileName = "New Card Data", menuName = "Scriptable Objects/Cards/Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private bool translateName = false;
    public string GetCardName() => translateName ? TranslationHandler.mainInstance.GetTextTranslation(cardName) : cardName;

    [SerializeField] private string cardDescription;
    [SerializeField] private bool translateDescription = true;
    public string GetCardDescription() => translateDescription ? TranslationHandler.mainInstance.GetTextTranslation(cardDescription) : cardDescription;

    public Texture2D cardTexture;
}
