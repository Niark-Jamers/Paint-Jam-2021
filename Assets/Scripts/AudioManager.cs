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

    public void StopBackgroundMusic()
    {
        StartCoroutine(StopBackgroundMusic2());
    }

    IEnumerator StopBackgroundMusic2()
    {

        float fadeTime = 1f;

        float originalVolume = backgroundSource.volume;

        float t = Time.time;
        while (Time.time - t < fadeTime)
        {
            float d = 1 - Mathf.Clamp01((Time.time - t) / fadeTime);
            backgroundSource.volume = originalVolume * d;
            yield return new WaitForEndOfFrame();
        }
    }
}
