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
        if (other.TryGetComponent(out GameOverHelper gameOver))
        {
            Time.timeScale = 0;
            gameOver.GameoverFromWin();
        }
    }
}
