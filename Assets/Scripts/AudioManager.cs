using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Space(20)]

    public AudioSource[] menuAudioSource;
    public AudioSource[] sfxAudioSource;
    public AudioSource sountrackAudioSource;
    float originalVolume;
    [Header("Pitcher")]
    [SerializeField] Vector2 pitchRange;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            originalVolume = sountrackAudioSource.volume;
            DontDestroyOnLoad(gameObject);
           
        }
    }

    public void SFXAudioOneShot(AudioClip audioClip, float volume)
    {
        foreach (AudioSource item in sfxAudioSource)
        {
            if (!item.isPlaying)
            {
                //override volume if different to current volume and not 0
                if (volume != 0 || item.volume != volume)
                {
                    item.volume = volume;
                }

                item.pitch = 1;
                item.clip = audioClip;
                item.PlayOneShot(audioClip);
                break;
            }
        }
    } 
    
    public void SFXAudioPitchedOneShot(AudioClip audioClip, float volume)
    {
        foreach (AudioSource item in sfxAudioSource)
        {
            if (!item.isPlaying)
            {
                //override volume if different to current volume and not 0
                if (volume != 0 || item.volume != volume)
                {
                    item.volume = volume;
                }

                item.pitch = RandomPitch;
                item.clip = audioClip;
                item.PlayOneShot(audioClip);
                break;
            }
        }
    }

    public void MenuAudioOneShot(AudioClip audioClip, float volume)
    {
        foreach (AudioSource item in menuAudioSource)
        {
            if(!item.isPlaying)
            {
                //override volume if different to current volume and not 0
                if (volume != 0 || item.volume != volume)
                {
                    item.volume = volume;
                }
                item.pitch = 1;
                item.clip = audioClip;
                item.PlayOneShot(audioClip);
                break;
            }
        }
    }

    public void MenuAudioPitchedShot(AudioClip audioClip, float volume)
    {
        foreach (AudioSource item in menuAudioSource)
        {
            if (!item.isPlaying)
            {
                //override volume if different to current volume and not 0
                if (volume != 0 || item.volume != volume)
                {
                    item.volume = volume;
                }
               
                item.pitch = RandomPitch;
                item.clip = audioClip;
                item.PlayOneShot(audioClip);
                break;
            }
        }
    }
    public void IsSoundtrackLoweredVolume(bool value)
    {
        if(value)
        {
            sountrackAudioSource.volume = originalVolume / 3;
        }

        else
        {
            sountrackAudioSource.volume = originalVolume;
        }
       
    }
    public float RandomPitch
    {
        get
        {
            return Random.Range(pitchRange.x, pitchRange.y);
        }
    }

  

}

