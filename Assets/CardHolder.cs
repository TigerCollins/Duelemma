using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
//using kFramework;

public class CardHolder : MonoBehaviour
{
    [SerializeField] MMFeedbacks cardInfoFeedback;
    [SerializeField] CardObject[] cardInfoObjects;



    private void Start()
    {
        DimensionSwitcher.instance.onDimensionChange.AddListener(delegate { PlayCardInfoFeedback(); });
    }

    public void PlayCardInfoFeedback()
    {
        foreach (CardObject item in cardInfoObjects)
        {
            item.SetCardTextures();
            item.SetCardText();
            item.SetCardTextures();
        }
        cardInfoFeedback.PlayFeedbacks();
    }

}
