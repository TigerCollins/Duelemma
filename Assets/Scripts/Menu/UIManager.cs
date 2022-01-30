using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public MenuScripts menuScripts;
    public List<CreditsDetails> credits;

    [SerializeField] float timeScale;
    [SerializeField] bool paused;
    [SerializeField] InputAction pauseInput;
    [SerializeField] UnityEvent<bool> pauseStateChanged;

    [Space(10)]

    public GameObject levelLoaderToMainMenuPrefab;
    public GameObject levelLoaderToGamePrefab;
    
    public bool IsPaused
    {
        get
        {
            return paused;
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PauseGame(!paused);
        }    
    }

    public void ForcePauseGameToFunction(bool newState)
    {
        if (newState != paused)
        {
            if (newState)
            {
                Time.timeScale = 0;
            }

            else
            {
                Time.timeScale = 1;
            }
            timeScale = Time.timeScale;
            paused = newState;
            ChangeFeedbackPlayed(newState);
            pauseStateChanged.Invoke(paused);
        }
    }

    public void PauseGame(bool newState)
    {
        if(SceneManager.GetActiveScene().buildIndex != 1)
        {
            if (newState != paused)
            {
                if (newState)
                {
                    Time.timeScale = 0;
                }

                else
                {
                    Time.timeScale = 1;
                }
                timeScale = Time.timeScale;
                paused = newState;
                ChangeFeedbackPlayed(newState);
                pauseStateChanged.Invoke(paused);
            }
        }
        
      
    }

   
    public void ChangeLevel(int target)
    {
        GameObject newObject = Instantiate(levelLoaderToGamePrefab);
        if (newObject.TryGetComponent(out LevelLoader loader))
        {
            loader.TargetSceneIndex = target;
            loader.LoadScene();
        }
    }

   public void GoToGame()
    {
       GameObject newObject = Instantiate(levelLoaderToGamePrefab);
        if(newObject.TryGetComponent(out LevelLoader loader))
        {
            loader.LoadScene();
        }
    }

    public void GoToMainMenu()
    {
        GameObject newObject = Instantiate(levelLoaderToMainMenuPrefab);
        if (newObject.TryGetComponent(out LevelLoader loader))
        {
            loader.LoadScene();
        }
    }

    public void ChangeFeedbackPlayed(bool pauseState)
    {

        if (pauseState == true)
        {
           
            menuScripts.otherScript.GoToDifferentMenu(MenuScript.OtherMenus.Pause);
        }

        else if(pauseState == false)
        {
            menuScripts.pauseMenuScript.GoToDifferentMenu(MenuScript.OtherMenus.Other);
        }
    }


    private void Awake()
    {
        Time.timeScale = 1;
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        else
        {
            Destroy(gameObject);
            //Destroy(instance.gameObject);
           // instance = this;
           // DontDestroyOnLoad(gameObject);
        }

        pauseInput.performed += PauseGame;
        pauseInput.Enable();
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if(menuScripts.GetMenuScriptsAsList().Count != 0)
        {
            menuScripts.defaultMenu.GoToDifferentMenu(menuScripts.defaultMenu.menuGroup);
            menuScripts.currentMenu = menuScripts.defaultMenu;
            menuScripts.ForceCloseOtherMenuScripts();
            menuScripts.InitSwitch();
        }

        
    }


    public void Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void ResumeGame()
    {
     //   GameController.instance.gamePhase = GameController.instance.resumeGamePhase;
    }

   
}

[System.Serializable]
public class MenuScripts
{
    public MenuScript defaultMenu;
    public MenuScript currentMenu;

    [Space(15)]

    public MenuScript mainMenuScript;
    public MenuScript creditsScript;
    public MenuScript optionMenuScript;
    public MenuScript otherScript;
    public MenuScript pauseMenuScript;


    public MenuScript CurrentMenuScript
    {
        set
        {
            currentMenu = value;
        }

        get
        {
            return currentMenu;
        }
    }

    public List<MenuScript> GetMenuScriptsAsList()
    {
        List<MenuScript> list = new List<MenuScript>();
        if (mainMenuScript != null)
        {
            list.Add(mainMenuScript);
        }
        
        if (creditsScript != null)
        {
            list.Add(creditsScript);
        }
        
        if (optionMenuScript != null)
        {
            list.Add(optionMenuScript);
        }

        /* if (saveMenuScript != null)
         {
             list.Add(saveMenuScript);
         }
        */

        if (otherScript != null)
        {
            list.Add(otherScript);
        }

        if (pauseMenuScript != null)
        {
            list.Add(pauseMenuScript);
        }

        return list;
    }

    public MainMenuHandler MainMenuScript()
    {
        return mainMenuScript as MainMenuHandler;
    }

    public CreditsMenuhandler CreditsMenuHandlerScript()
    {
        return creditsScript as CreditsMenuhandler;
    }
   
    public GUIHandler GeneralGUIScript()
    {
        return otherScript as GUIHandler;
    }

    public PauseMenuHandler PauseMenuScript()
    {
        return pauseMenuScript as PauseMenuHandler;
    }


    public void ForceCloseOtherMenuScripts()
    {
        foreach (MenuScript item in GetMenuScriptsAsList())
        {
            if(item != currentMenu)
            {
                if(item == mainMenuScript)
                {
                    MainMenuScript().CloseWholeMenuFunction();
                }
               if(item == creditsScript)
                {
                    CreditsMenuHandlerScript().CloseWholeMenuFunction();
                }
               if(item == otherScript)
                {
                    GeneralGUIScript().CloseWholeMenuFunction();
                }
                if (item == pauseMenuScript)
                {
                    PauseMenuScript().CloseWholeMenuFunction();
                }
            }

            
        }
    }

    public void InitSwitch()
    {
        switch (currentMenu.menuGroup)
        {
            case MenuScript.OtherMenus.Main:
                UIManager.instance.menuScripts.MainMenuScript().Init();
                break;
            case MenuScript.OtherMenus.Options:
              //  UIManager.instance.menuScripts.LobbyHandlerScript().Init();
                break;
            case MenuScript.OtherMenus.Credits:
                break;
            case MenuScript.OtherMenus.Other:
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class CreditsDetails
{
    public string name;
    public string job1;
    public string job2;
    public Sprite icon;
    public LinksInfo webLink;
}

[System.Serializable]
public class LinksInfo
{
    public string displayName;
    public string link;
}
