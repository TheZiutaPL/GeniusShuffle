using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteTranslator : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        [SerializeField] private string translationKey;
        [SerializeField] private bool subscribeToHandler;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable() 
        {
            if (!subscribeToHandler)
                return;

            TranslationHandler.SubscribeTranslator(GetTranslation); 
        }

        private void OnDisable()
        {
            if (!subscribeToHandler)
                return;

            TranslationHandler.UnsubscribeTranslator(GetTranslation);
        }

        private void Start()
        {
            GetTranslation();
        }

        private void GetTranslation() => spriteRenderer.sprite = TranslationHandler.GetSpriteTranslation(translationKey);
    }
}
