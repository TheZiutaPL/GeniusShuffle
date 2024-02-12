using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteTranslator : Translator
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void GetTranslation() => spriteRenderer.sprite = usedTranslationHandler.GetSpriteTranslation(translationKey);
    }
}
