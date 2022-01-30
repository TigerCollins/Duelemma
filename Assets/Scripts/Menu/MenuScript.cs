using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Events;

public class MenuScript : MonoBehaviour
{
    public enum OtherMenus
    {
        Main,
        Options,
        Credits,
        Other,
        Pause
    }

    [Header("Menu Script")]
    public OtherMenus menuGroup;
    public List<MMFeedbacks> allShowEvents;
    int listCount;
    public List<MMFeedbacks> allHideEvents;

    public MenuTransition transitionDetails;


    public void GoToDifferentMenu(OtherMenus newMenu)
    {
        //Switch to add/remove listener to the feels group complete
        switch (newMenu)
        {
            case OtherMenus.Main:
                UIManager.instance.menuScripts.CurrentMenuScript = UIManager.instance.menuScripts.mainMenuScript;
                break;
            case OtherMenus.Credits:
                UIManager.instance.menuScripts.CurrentMenuScript = UIManager.instance.menuScripts.creditsScript;
                //menuScripts.mainMenuScript.transitionDetails.baseMenuFeedbackGroup.hideFeedback.Events.OnComplete.AddListener(delegate { StartCoroutine(menuScripts.currentMenuScript.ShowMenu()); menuScripts.mainMenuScript.transitionDetails.baseMenuFeedbackGroup.hideFeedback.Events.OnComplete.RemoveAllListeners(); });
                break;
            case OtherMenus.Options:
                UIManager.instance.menuScripts.CurrentMenuScript = UIManager.instance.menuScripts.optionMenuScript;
                break;
            case OtherMenus.Other:
                UIManager.instance.menuScripts.CurrentMenuScript = UIManager.instance.menuScripts.otherScript;
                break;
            case OtherMenus.Pause:
                UIManager.instance.menuScripts.CurrentMenuScript = UIManager.instance.menuScripts.pauseMenuScript;
                break;
            default:
                break;
        }
        //Event to open next menu
        UnityAction eventAction = null;
        eventAction = new UnityAction(delegate () { UIManager.instance.menuScripts.CurrentMenuScript.ShowMenuFunction(); transitionDetails.menuCloseCompleted.RemoveListener(eventAction); });
        transitionDetails.menuCloseCompleted.AddListener(eventAction);

        //Close Menu
        CloseCurrentMenu();
    }
   
    public void GoToGameSpace()
    {
        CloseCurrentMenu();
    }

    void CloseCurrentMenu()
    {
        listCount = allHideEvents.Count;
        transitionDetails.menuCloseStarted.Invoke();
        UnityAction eventAction = null;
        
        foreach (MMFeedbacks item in allHideEvents)
        {
            eventAction = new UnityAction(delegate () { AttemptCallClosedEvent(); item.Events.OnComplete.RemoveListener(eventAction); });
            item.Events.OnComplete.AddListener(eventAction);
            item.Initialization();
            item.PlayFeedbacks();
        }
    }


    void ShowMenuFunction()
    {
        UnityAction eventAction = null;
        listCount = allShowEvents.Count;
        transitionDetails.menuEnterStarted.Invoke();
        foreach (MMFeedbacks item in allShowEvents)
        {
            eventAction = new UnityAction(delegate () { AttemptCallEnteredEvent(); item.Events.OnComplete.RemoveListener(eventAction); });
            item.Events.OnComplete.AddListener(eventAction);
        }
        StartCoroutine(ShowMenu());
    }
    public IEnumerator ShowMenu()
    {
        UIManager.instance.menuScripts.InitSwitch();

        yield return new WaitForSecondsRealtime(transitionDetails.transitionDelay);

        foreach (var item in allShowEvents)
        {
            item.Initialization();
            item.PlayFeedbacks();
        }

        transitionDetails.baseCanvasGroup.alpha = 1;
        transitionDetails.baseCanvasGroup.interactable = true;
        transitionDetails.baseCanvasGroup.blocksRaycasts = true;
    }

    void AttemptCallEnteredEvent()
    {
        listCount--;
        if(listCount<=0)
        {
            transitionDetails.menuEnterCompleted.Invoke();
        }
    }

    void AttemptCallClosedEvent()
    {
        listCount--;
        if (listCount <= 0)
        {
            transitionDetails.menuCloseCompleted.Invoke();
        }
    }

    public void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }

    public void MakeCanvasGroupInteractable(CanvasGroup canvasGroup)
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    [System.Serializable]
    public class MenuTransition
    {
        public float transitionDelay = .5f;
        public CanvasGroup baseCanvasGroup;
        public UnityEvent menuEnterStarted;
        public UnityEvent menuEnterCompleted;
        public UnityEvent menuCloseStarted;
        public UnityEvent menuCloseCompleted;
    }

    [System.Serializable]
    public class FeedbackGroup
    {
        public MMFeedbacks showFeedback;
        public MMFeedbacks hideFeedback;

    }
}
