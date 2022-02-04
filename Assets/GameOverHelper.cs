using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHelper : MonoBehaviour
{
    public void GameoverFromDeath()
    {
        GameOverHandler.instance.TriggerGameOver(true);
    }

    public void GameoverFromWin()
    {
        GameOverHandler.instance.TriggerGameOver(false);
    }
}
