using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    public static GameOverHandler instance;
    [SerializeField] MMFeedbacks winFeedback;
    [SerializeField] MMFeedbacks loseFeedback;

    private void Awake()
    {
        instance = this;
    }

    public void TriggerGameOver(bool fromDeath)
    {
        if(fromDeath)
        {
            loseFeedback.PlayFeedbacks();
        }

        else
        {
            winFeedback.PlayFeedbacks();
        }
    }
}
