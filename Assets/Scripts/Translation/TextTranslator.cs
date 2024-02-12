using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FlawareStudios.Translation
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextTranslator : Translator
    {
        private TMP_Text textHolder;

        private void Awake()
        {
            textHolder = GetComponent<TMP_Text>();
        }

        protected override void GetTranslation() => textHolder.SetText(usedTranslationHandler.GetTextTranslation(translationKey));
    }
}
