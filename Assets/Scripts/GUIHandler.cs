using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;

public class GUIHandler : MenuScript
{
    [SerializeField] MMFeedbacks hideFeedback;

    [Header("Health")]
    [SerializeField] Sprite healthSprite;
    [SerializeField] Sprite healthLostSprite;
    [SerializeField] Image[] healthImages;
    [SerializeField] MMFeedbacks showHealthFeedback;
    [SerializeField] MMFeedbacks hideHealthFeedback;
    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }

    public void UpdateHealth(int healthValue)
    {
        int id = 0;
        foreach (var item in healthImages)
        {
            if(id>healthValue)
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


}
