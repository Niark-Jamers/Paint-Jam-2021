using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource backgroundSource;
    public AudioSource sfxBackground;
    public AudioSource pitch3;

    void Start()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip, float volume = 1)
    {
        sfxBackground.PlayOneShot(clip, volume);
    }
    public void PlaySFXPitch3(AudioClip clip, float volume = 1)
    {
        pitch3.PlayOneShot(clip, volume);
    }
}
