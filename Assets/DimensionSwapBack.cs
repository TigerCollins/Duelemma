using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionSwapBack : MonoBehaviour
{
   [SerializeField] bool canTrigger = true;
    public void SwapDimensionBack()
    {
        if(canTrigger)
        {
            canTrigger = false;

            DimensionSwitcher.instance.ChangeDimensions();
            DimensionSwitcher.instance.onChangeFail.Invoke();
        }
    }

    private void OnDisable()
    {
        canTrigger = true;
    }
}
