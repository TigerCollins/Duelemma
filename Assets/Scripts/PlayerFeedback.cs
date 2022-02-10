using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;

public class PlayerFeedback : MonoBehaviour
{
    [SerializeField] MMFeedbacks attackFeedback;
    [SerializeField] MMFeedbacks healthLostFeedback;
    [SerializeField] MMFeedbacks deathFeedback;

    public void Start()
    {
        PlayerController.instance.characterEvents.onAttack.AddListener(delegate { attackFeedback.PlayFeedbacks(); });
        PlayerController.instance.StatsController.onHealthLost.AddListener(delegate { healthLostFeedback.PlayFeedbacks(); });
        PlayerController.instance.StatsController.onDeath.AddListener(delegate { deathFeedback.PlayFeedbacks(); });
    }

    
}
