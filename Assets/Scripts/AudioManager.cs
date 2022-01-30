using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Space(20)]

    public AudioSource menuAudioSource;
    public AudioSource sfxAudioSource;
    public AudioSource sountrackAudioSource;
    float originalVolume;
    float originalPitch;
    [Header("Pitcher")]
    [SerializeField]
    Vector2 pitchRange;
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
            originalPitch = sfxAudioSource.volume;
            DontDestroyOnLoad(gameObject);
           
        }
    }

    public void SFXAudioOneShot(AudioClip audioClip)
    {
        sfxAudioSource.pitch = originalPitch;
        sfxAudioSource.clip = audioClip;
        sfxAudioSource.PlayOneShot(audioClip);
    } public void SFXAudioPitchedOneShot(AudioClip audioClip)
    {
        sfxAudioSource.pitch = RandomPitch;
        sfxAudioSource.clip = audioClip;
        sfxAudioSource.PlayOneShot(audioClip);
    }

    public void MenuAudioOneShot(AudioClip audioClip)
    {
        menuAudioSource.pitch = originalPitch;
        menuAudioSource.clip = audioClip;
        menuAudioSource.PlayOneShot(audioClip);
    }

    public void MenuAudioPitchedShot(AudioClip audioClip)
    {
        menuAudioSource.pitch = RandomPitch;
        menuAudioSource.clip = audioClip;
        menuAudioSource.PlayOneShot(audioClip);
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

