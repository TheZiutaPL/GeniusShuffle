using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlawareStudios.Translation
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioTranslator : Translator
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected override void GetTranslation() => audioSource.clip = usedTranslationHandler.GetAudioTranslation(translationKey);
    }
}
