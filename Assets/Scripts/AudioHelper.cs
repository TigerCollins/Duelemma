using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public bool randomPitch;
    public float volumeOveride;

    public void PlayMenuSound(AudioClip desiredAudioClip)
    {
        if(!randomPitch)
        {
            AudioManager.instance.MenuAudioOneShot(desiredAudioClip, volumeOveride);
        }

        else
        {
            AudioManager.instance.MenuAudioPitchedShot(desiredAudioClip, volumeOveride);
        }
        
    }

    public void PlaySFXSound(AudioClip desiredAudioClip)
    {
        if (!randomPitch)
        {
            AudioManager.instance.SFXAudioOneShot(desiredAudioClip, volumeOveride);
        }

        else
        {
            AudioManager.instance.SFXAudioPitchedOneShot(desiredAudioClip, volumeOveride);
        }

    }
}
