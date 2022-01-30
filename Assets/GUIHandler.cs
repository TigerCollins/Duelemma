using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class GUIHandler : MenuScript
{
    [SerializeField] MMFeedbacks hideFeedback;
    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }
}
