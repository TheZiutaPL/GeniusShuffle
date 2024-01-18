using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [System.Serializable]
    public class TranslationCell<T>
    {
        public string key = "NewKey";
        public T defaultTranslation;

        public T[] translations = new T[] { };

        public TranslationCell()
        {

        }
    }

    public static class TranslationAssetsUtility
    {
        public static void ResizeTranslations<T>(ref T[] translations, int newSize, bool destroyAdditional)
        {
            int size = destroyAdditional ? newSize : Mathf.Max(translations.Length, newSize);
            T[] newTranslations = new T[size];

            for (int i = 0; i < newTranslations.Length; i++)
            {
                if (i >= translations.Length)
                    break;
                
                newTranslations[i] = translations[i];
            }

            translations = newTranslations;
        }
    }
}
