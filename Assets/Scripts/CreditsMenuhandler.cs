using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MoreMountains.Feedbacks;

public class CreditsMenuhandler : MenuScript
{
    [Header("Credits Specific")]
    [SerializeField] MMFeedbacks hideFeedback;
    [SerializeField] List<CreditGroupLink> creditGroups = new List<CreditGroupLink>();
    [SerializeField] List<Button> creditButtons = new List<Button>();



    private void Start()
    {
        Init();
    }

    public void Init()
    {
        int id = 0;
        foreach (CreditsDetails creditInfo in UIManager.instance.credits)
        {
            //Icon
            creditGroups[id].iconImage.gameObject.SetActive(creditInfo.icon != null); //visibility
            creditGroups[id].iconImage.sprite = creditInfo.icon;

            //Name
            creditGroups[id].name.text = creditInfo.name;

            //Jobs
            creditGroups[id].job1.gameObject.SetActive(!string.IsNullOrEmpty(creditInfo.job1)); //visibility
            creditGroups[id].job2.gameObject.SetActive(!string.IsNullOrEmpty(creditInfo.job2)); //visibility
            creditGroups[id].job1.text = creditInfo.job1;
            creditGroups[id].job2.text = creditInfo.job2;

            //Link
            creditGroups[id].linkName.text = "Select to visit my " + creditInfo.webLink.displayName;
            creditButtons[id].onClick.AddListener(delegate { Application.OpenURL(creditInfo.webLink.link) ; });

            id++;

        }


    }

    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }
}

[System.Serializable]
public class CreditGroupLink
{
    public TextMeshProUGUI name;
    public Image iconImage;
    
    public TextMeshProUGUI job1;
    public TextMeshProUGUI job2;

    [Space(10)]

    public TextMeshProUGUI linkName;
    [SerializeField] string link;

    public string Link
    {
        get
        {
            return link;
        }

        set
        {
            link = value;
        }
    }
}
