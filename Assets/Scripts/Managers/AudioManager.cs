using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioSource soundSource;

    public static string MASTER_KEY = "MASTER_VOLUME";
    public static string SFX_KEY = "SFX_VOLUME";
    public static string MUSIC_KEY = "MUSIC_VOLUME";

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
