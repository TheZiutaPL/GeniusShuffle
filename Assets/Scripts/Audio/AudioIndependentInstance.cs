using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioIndependentInstance : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips = new AudioClip[0];

    public void PlayAudio() => AudioManager.PlaySound(audioClips);
}
