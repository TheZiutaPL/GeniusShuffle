using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlawareStudios.Translation
{
    public class TranslationHandler : MonoBehaviour
    {
        [SerializeField] private bool isMainInstance;
        public static TranslationHandler mainInstance { get; private set; }
        private static List<TranslationHandler> instances = new List<TranslationHandler>();
        [SerializeField] private string startLanguage;
        private static string currentLanguage;
        [SerializeField] private TranslationModule translationModule;

        private Action subscribedTranslators;

        private const string CURRENT_LANG_KEY = "CURRENT_LANGUAGE";

        private void Awake()
        {
            if (isMainInstance)
            {
                if (mainInstance == null)
                {
                    mainInstance = this;

                    currentLanguage = PlayerPrefs.GetString(CURRENT_LANG_KEY, startLanguage);
                }
                else
                    Destroy(gameObject);
            }

            instances.Add(this);
        }

        public void SetLanguage(string newLanguage) 
        { 
            currentLanguage = newLanguage;

            PlayerPrefs.SetString(CURRENT_LANG_KEY, currentLanguage);

            RetranslateSubscribed();
        }

        public void RetranslateSubscribed()
        {
            for (int i = 0; i < instances.Count; i++)
                instances[i].subscribedTranslators?.Invoke();
        }

        public void SubscribeTranslator(Action retranslationAction) => subscribedTranslators += retranslationAction;
        public void UnsubscribeTranslator(Action retranslationAction) => subscribedTranslators -= retranslationAction;

        public string GetTextTranslation(string translationKey) => translationModule.GetTextTranslation(translationKey, translationModule.GetLanguageIndex(currentLanguage));
        public Sprite GetSpriteTranslation(string translationKey) => translationModule.GetSpriteTranslation(translationKey, translationModule.GetLanguageIndex(currentLanguage));
        public AudioClip GetAudioTranslation(string translationKey) => translationModule.GetAudioTranslation(translationKey, translationModule.GetLanguageIndex(currentLanguage));
    }
}
