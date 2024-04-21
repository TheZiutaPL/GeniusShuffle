using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [System.Serializable]
    public class TranslationCell<T>
    {
        public string key = "NewKey";

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

        public static T GetTranslation<T>(ref TranslationCell<T>[] translations, string translationKey, int languageIndex, T errorReturn)
        {
            if (languageIndex < 0)
                return errorReturn;

            foreach (TranslationCell<T> item in translations)
            {
                if (item.key != translationKey)
                    continue;

                if (languageIndex >= item.translations.Length)
                    return errorReturn;
                else
                    return item.translations[languageIndex];
            }

            return errorReturn;
        }

        public static void RemoveTranslation<T>(ref TranslationCell<T>[] translations, TranslationCell<T> translation)
        {
            TranslationCell<T>[] newTranslations = new TranslationCell<T>[translations.Length - 1];
            bool destroyed = false;
            for (int i = 0; i < translations.Length; i++)
            {
                if (!destroyed && translations[i] == translation)
                {
                    destroyed = true;
                    continue;
                }

                newTranslations[i - (destroyed ? 1 : 0)] = translations[i];
            }

            if (destroyed)
                translations = newTranslations;
            else
                Debug.LogError($"There is no {translation.key} in translations array");
        }
    }
}
