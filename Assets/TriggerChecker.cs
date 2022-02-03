using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out DimensionSwapBack swapback))
        {
            swapback.SwapDimensionBack();
        }

    }
}
