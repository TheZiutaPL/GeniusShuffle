using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [CreateAssetMenu(fileName = "New Translation Module", menuName = "Scriptable Objects/Translation Module")]
    public class TranslationModule : ScriptableObject
    {
        [HideInInspector] public string[] translatedLanguages = new string[] { "English" };
        public int GetLanguageIndex(string language)
        {
            for (int i = 0; i < translatedLanguages.Length; i++)
            {
                if (translatedLanguages[i] == language)
                    return i;
            }

            Debug.LogError($"{name} does not contain translation for {language} languange!");
            return -1;
        }

        [HideInInspector] public TranslationCell<string>[] textTranslations = new TranslationCell<string>[0];
        public string errorText = "TRANSLATION_NOT_FOUND";
        public string GetTextTranslation(string translationKey, int languageIndex) 
            => TranslationAssetsUtility.GetTranslation(ref textTranslations, translationKey, languageIndex, errorText);

        [HideInInspector] public TranslationCell<Sprite>[] spriteTranslations = new TranslationCell<Sprite>[0];
        public Sprite errorSprite;
        public Sprite GetSpriteTranslation(string translationKey, int languageIndex)
            => TranslationAssetsUtility.GetTranslation(ref spriteTranslations, translationKey, languageIndex, errorSprite);

        [HideInInspector] public TranslationCell<AudioClip>[] audioTranslations = new TranslationCell<AudioClip>[0];
        public AudioClip errorAudio;
        public AudioClip GetAudioTranslation(string translationKey, int languageIndex)
            => TranslationAssetsUtility.GetTranslation(ref audioTranslations, translationKey, languageIndex, errorAudio);


        public void AddEmptyTranslation<T>(ref T[] translationArray) where T : new()
        {
            TranslationAssetsUtility.ResizeTranslations(ref translationArray, translationArray.Length + 1, true);
            translationArray[translationArray.Length - 1] = new();

            SetLanguageCount(translatedLanguages.Length, false);
        }

        public void DeleteEmptyKeys()
        {
            if (textTranslations.Length > 0)
            {
                for (int i = textTranslations.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(textTranslations[i].key))
                        TranslationAssetsUtility.RemoveTranslation(ref textTranslations, textTranslations[i]);
                }
            }

            if (spriteTranslations.Length > 0)
            {
                for (int i = spriteTranslations.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(spriteTranslations[i].key))
                        TranslationAssetsUtility.RemoveTranslation(ref spriteTranslations, spriteTranslations[i]);
                }
            }

            if (audioTranslations.Length > 0)
            {
                for (int i = audioTranslations.Length - 1; i >= 0; i--)
                {
                    if (string.IsNullOrWhiteSpace(audioTranslations[i].key))
                        TranslationAssetsUtility.RemoveTranslation(ref audioTranslations, audioTranslations[i]);
                }
            }
        }

        public void SetLanguageCount(int count, bool optimize = false)
        {
            TranslationAssetsUtility.ResizeTranslations(ref translatedLanguages, count, true);

            for (int i = 0; i < textTranslations.Length; i++)
                TranslationAssetsUtility.ResizeTranslations(ref textTranslations[i].translations, translatedLanguages.Length, optimize);

            for (int i = 0; i < spriteTranslations.Length; i++)
                TranslationAssetsUtility.ResizeTranslations(ref spriteTranslations[i].translations, translatedLanguages.Length, optimize);

            for (int i = 0; i < audioTranslations.Length; i++)
                TranslationAssetsUtility.ResizeTranslations(ref audioTranslations[i].translations, translatedLanguages.Length, optimize);            
        }
    }
}
