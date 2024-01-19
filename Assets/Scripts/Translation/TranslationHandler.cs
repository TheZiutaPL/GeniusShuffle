using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlawareStudios.Translation;

public class TranslationHandler : MonoBehaviour
{
    private static TranslationHandler instance;
    [SerializeField] private string currentLanguage;
    [SerializeField] private TranslationModule translationModule;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static string GetTextTranslation(string translationKey) => instance.translationModule.GetTextTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
    public static Sprite GetSpriteTranslation(string translationKey) => instance.translationModule.GetSpriteTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
    public static AudioClip GetAudioTranslation(string translationKey) => instance.translationModule.GetAudioTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
}
