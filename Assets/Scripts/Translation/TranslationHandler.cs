using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FlawareStudios.Translation
{
    public class TranslationHandler : MonoBehaviour
    {
        private static TranslationHandler instance;
        [SerializeField] private string currentLanguage;
        [SerializeField] private TranslationModule translationModule;

        private static Action subscribedTranslators;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void SetLanguage(string newLanguage) 
        { 
            currentLanguage = newLanguage;

            subscribedTranslators?.Invoke();
        }

        public static void SubscribeTranslator(Action retranslationAction) => subscribedTranslators += retranslationAction;
        public static void UnsubscribeTranslator(Action retranslationAction) => subscribedTranslators -= retranslationAction;

        public static string GetTextTranslation(string translationKey) => instance.translationModule.GetTextTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
        public static Sprite GetSpriteTranslation(string translationKey) => instance.translationModule.GetSpriteTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
        public static AudioClip GetAudioTranslation(string translationKey) => instance.translationModule.GetAudioTranslation(translationKey, instance.translationModule.GetLanguageIndex(instance.currentLanguage));
    }
}
