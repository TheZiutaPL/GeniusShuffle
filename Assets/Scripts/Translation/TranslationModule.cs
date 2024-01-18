using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [CreateAssetMenu(fileName = "New Translation Module", menuName = "Scriptable Objects/Translation Module")]
    public class TranslationModule : ScriptableObject
    {
        [HideInInspector] public string[] translatedLanguages = new string[] { "English" };

        public TranslationCell<string>[] textTranslations = new TranslationCell<string>[0];
        public TranslationCell<Sprite>[] spriteTranslations = new TranslationCell<Sprite>[0];
        public TranslationCell<AudioClip>[] audioTranslations = new TranslationCell<AudioClip>[0];

        public void AddEmptyTranslation<T>(ref T[] translationArray) where T : new()
        {
            TranslationAssetsUtility.ResizeTranslations(ref translationArray, translationArray.Length + 1, true);
            translationArray[translationArray.Length - 1] = new();

            SetLanguageCount(translatedLanguages.Length, false);
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
