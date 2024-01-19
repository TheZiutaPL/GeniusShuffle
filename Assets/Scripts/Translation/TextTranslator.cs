using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FlawareStudios.Translation;

[RequireComponent(typeof(TMP_Text))]
public class TextTranslator : MonoBehaviour
{
    private TMP_Text textHolder;
    [SerializeField] private string translationKey;

    private void Awake()
    {
        textHolder = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GetTranslation();
    }

    private void GetTranslation() => textHolder.SetText(TranslationHandler.GetTextTranslation(translationKey));
}
