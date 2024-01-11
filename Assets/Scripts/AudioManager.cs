using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioSource soundSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static void PlaySound(AudioClip clip) => instance.soundSource.PlayOneShot(clip);
    public static void PlaySound(AudioClip[] clip) => instance.soundSource.PlayOneShot(clip[Random.Range(0, clip.Length)]);
}
