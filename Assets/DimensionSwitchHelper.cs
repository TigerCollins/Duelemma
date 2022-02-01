using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DimensionSwitchHelper : MonoBehaviour
{
    public void ChangeDimension(InputAction.CallbackContext context)
    {

            DimensionSwitcher.instance.ChangeDimensionViaInput(context);
    }
}
