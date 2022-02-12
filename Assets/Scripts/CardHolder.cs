using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
//using kFramework;

public class CardHolder : MonoBehaviour
{
    [SerializeField] CardObject[] cardInfoObjects;

    [SerializeField] float levelLoadDelay = 3;

    private void Start()
    {
        DimensionSwitcher.instance.onDimensionChange.AddListener(delegate { PlayCardInfoFeedback(); });
        GlobalHelper.instance.onLevelChange.AddListener(delegate { StartCoroutine(ShowCards()); });
    }

    public void PlayCardInfoFeedback()
    {
        if(!GlobalHelper.InMainMenu())
        {
            foreach (CardObject item in cardInfoObjects)
            {
              //  item.SetCardTextures();
                item.SetCardText();
               // item.SetCardTextures();

                    item.parentTransform.gameObject.SetActive(!item.BothSidesLocked());
            
            }
            CardDisplayLink.instance.cardFeedback.PlayFeedbacks();
        }
    }

    public IEnumerator ShowCards()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        PlayCardInfoFeedback();
    }


   
}
