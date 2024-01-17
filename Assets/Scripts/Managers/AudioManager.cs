using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private AudioSource soundSource;

    public static string MASTER_KEY = "MasterVolume";
    public static string SFX_KEY = "SFXVolume";
    public static string MUSIC_KEY = "MusicVolume";

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
