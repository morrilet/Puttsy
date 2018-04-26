using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource musicSource;
    public AudioSource effectSource;
    public AudioSource sustainedEffectSource;
    [Space]
    public AudioClip[] audio;
    
    public bool playingSustainedEffect = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void PlayEffect(string audioName, float volume = 1.0f)
    {
        effectSource.PlayOneShot(GetAudioFromName(audioName), volume);
    }
    
    public void PlaySustainedEffect(string audioName, float volume = 1.0f)
    {
        sustainedEffectSource.clip = GetAudioFromName(audioName);
        sustainedEffectSource.volume = volume;
        sustainedEffectSource.loop = true;
        sustainedEffectSource.Play();
    }

    public void StopSustainedEffect()
    {
        sustainedEffectSource.Stop();
    }

    public void PlayMusic(string audioName, float volume = 1.0f)
    {
        musicSource.clip = GetAudioFromName(audioName);
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    private AudioClip GetAudioFromName(string name)
    {
        for(int i = 0; i < audio.Length; i++)
        {
            if(audio[i].name == name)
            {
                return audio[i];
            }
        }
        Debug.LogWarning("Audio clip \"" + name + "\" not found.");
        return null;
    }
}
