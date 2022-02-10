using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardLinker : MonoBehaviour
{
    public void ActivateCard1(InputAction.CallbackContext context)
    {
        CardDisplayLink.instance.cardObject1.ActivateAbility(context);
    }
    public void ActivateCard2(InputAction.CallbackContext context)
    {
        CardDisplayLink.instance.cardObject2.ActivateAbility(context);
    }
    public void ActivateCard3(InputAction.CallbackContext context)
    {
        CardDisplayLink.instance.cardObject3.ActivateAbility(context);
    }
}
