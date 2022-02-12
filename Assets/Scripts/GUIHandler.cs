using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class GUIHandler : MenuScript
{
    public static GUIHandler instance;

    [SerializeField] MMFeedbacks hideFeedback;

    [Header("Health")]
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite healthLostSprite;
    [SerializeField] Image[] healthImages;
    [SerializeField] MMFeedbacks showHealthFeedback;
    [SerializeField] MMFeedbacks hideHealthFeedback;

    [Header("Abilities")]
    [SerializeField] Image sliderSideA;
    [SerializeField] Image sliderSideB;
    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void UpdateHealth(int healthValue)
    {
        int id = 0;
        foreach (var item in healthImages)
        {
            if(id>=healthValue)
            {
                item.sprite = healthLostSprite;
            }

            else
            {
                item.sprite = healthSprite;
            }

            id++;
        }
    }


    public void UpdateSideAFill(float fill)
    {
        sliderSideA.fillAmount = fill;
    }

    public void UpdateSideBFill(float fill)
    {
        sliderSideB.fillAmount = fill;
    }
}
