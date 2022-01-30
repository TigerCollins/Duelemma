using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    public bool randomPitch;

    public void PlayMenuSound(AudioClip desiredAudioClip)
    {
        if(!randomPitch)
        {
            AudioManager.instance.MenuAudioOneShot(desiredAudioClip);
        }

        else
        {
            AudioManager.instance.MenuAudioPitchedShot(desiredAudioClip);
        }
        
    }

    public void PlaySFXSound(AudioClip desiredAudioClip)
    {
        if (!randomPitch)
        {
            AudioManager.instance.SFXAudioOneShot(desiredAudioClip);
        }

        else
        {
            AudioManager.instance.SFXAudioPitchedOneShot(desiredAudioClip);
        }

    }
}
