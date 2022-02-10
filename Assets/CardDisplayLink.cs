using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class CardDisplayLink : MonoBehaviour
{
    public static CardDisplayLink instance;
    public CardObject cardObject1;
    public CardObject cardObject2;
    public CardObject cardObject3;

    public MMFeedbacks cardFeedback;

    private void Awake()
    {
        instance = this;
    }
}
