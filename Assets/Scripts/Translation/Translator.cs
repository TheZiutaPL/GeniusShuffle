using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    public abstract class Translator : MonoBehaviour
    {
        [SerializeField] protected TranslationHandler usedTranslationHandler;
        [SerializeField] protected string translationKey;
        [SerializeField] protected bool subscribeToHandler;

        private void OnEnable()
        {
            if (!subscribeToHandler)
                return;

            usedTranslationHandler.SubscribeTranslator(GetTranslation);
        }

        private void OnDisable()
        {
            if (!subscribeToHandler)
                return;

            usedTranslationHandler.UnsubscribeTranslator(GetTranslation);
        }

        private void Start()
        {
            if (usedTranslationHandler == null) usedTranslationHandler = TranslationHandler.mainInstance;

            GetTranslation();
        }

        protected abstract void GetTranslation();
    }
}
