using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptHelper : MonoBehaviour
{
    [SerializeField] MenuScript currentMenu;
    [SerializeField] MenuScript.OtherMenus targetMenu;

    public void ChangeMenu()
    {
        currentMenu.GoToDifferentMenu(targetMenu);
    }

    public void ChangeSceneMainMenu()
    {
            UIManager.instance.GoToMainMenu();

    }

    public void ChangeSceneGame()
    {
        UIManager.instance.GoToGame();
    }
}
