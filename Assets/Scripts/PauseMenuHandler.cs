using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PauseMenuHandler : MenuScript
{
    [Header("Pause Specific")]
    [SerializeField] MMFeedbacks hideFeedback;
    [SerializeField] MMFeedbacks showFeedback;

    [Space(10)]

    [SerializeField] GUIHandler guiScript;

   public bool IsAPauseMenu
    {
        get
        {
            return GlobalHelper.instance.IsPaused;
        }
    }

    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }
}
