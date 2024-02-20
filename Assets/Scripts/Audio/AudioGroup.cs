using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Group", menuName = "Scriptable Objects/Audio/Audio Group")]
public class AudioGroup : ScriptableObject
{
    [SerializeField] private AudioClip[] audioClips = new AudioClip[0];

    public void PlayAudio()
    {
        if (NoClipError()) return;

        AudioManager.PlaySound(audioClips);
    }

    public void PlayAudio(int clipIndex)
    {
        if (NoClipError()) return;

        clipIndex = Mathf.Clamp(clipIndex, 0, audioClips.Length);

        AudioManager.PlaySound(audioClips[clipIndex]);
    }

    private bool NoClipError()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning($"There is no AudioClip in {name} AudioGroup");
            return true;
        }

        return false;
    }
}
