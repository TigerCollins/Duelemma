
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GlobalHelper : MonoBehaviour
{
    public Color normalColour;
    public Color overGrowthColor;

    public static GlobalHelper instance;
    float universalTimeScale = 1f;
    float playerTimeScale = 1f;
    public UnityEvent onLevelChange;



    public void DimensionColour(Dimensions newDimension)
    {
        switch (newDimension)
        {
            case Dimensions.dimensionA:
                RenderSettings.fogColor = normalColour;
                break;
            case Dimensions.dimensionB:
                RenderSettings.fogColor = overGrowthColor;
                break;
            default:
                break;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
      
    }

    public enum Dimensions
    {
        dimensionA,
        dimensionB
    }

    public enum CardSide
    {
        aSide,
        bSide
    }

    public void ResetTimeScales()
    {
        universalTimeScale = 1f;
        playerTimeScale = 1f;
    }

    public float UniversalTimeScale
    {
        get
        {
            return universalTimeScale;
        }

        set
        {
            universalTimeScale = value;
        }
    }

    public float PlayerTimeScale
    {
        get
        {
            return playerTimeScale;
        }

        set
        {
            playerTimeScale = value;
        }
    }

    public bool IsPaused
    {
        get
        {
            bool value = false;
            if (UIManager.instance != null)
            {
                value = UIManager.instance.IsPaused;
                if(AudioManager.instance != null)
                {
                    AudioManager.instance.IsSoundtrackLoweredVolume(value);
                }
               
            }
            return value;
        }
    }

    public static bool InMainMenu()
    {
        bool value = true;

        int sceneID = SceneManager.GetActiveScene().buildIndex;
        if(sceneID >1)
        {
            value = false;
        }

        return value;
    }

}
