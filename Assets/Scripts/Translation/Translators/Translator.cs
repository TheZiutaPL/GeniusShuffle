using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    public abstract class Translator : MonoBehaviour
    {
        [SerializeField] protected TranslationHandler usedTranslationHandler;
        private bool alreadyStarted;
        
        [SerializeField] protected string translationKey;
        [SerializeField] protected bool subscribeToHandler;

        private void OnEnable()
        {
            if (!subscribeToHandler || !alreadyStarted)
                return;

            usedTranslationHandler.SubscribeTranslator(GetTranslation);
        }

        private void OnDisable()
        {
            if (!subscribeToHandler || !alreadyStarted)
                return;

            usedTranslationHandler.UnsubscribeTranslator(GetTranslation);
        }

        private void Start()
        {
            if (usedTranslationHandler == null) usedTranslationHandler = TranslationHandler.mainInstance;
            alreadyStarted = true;

            OnEnable();

            GetTranslation();
        }

        protected abstract void GetTranslation();
    }
}
