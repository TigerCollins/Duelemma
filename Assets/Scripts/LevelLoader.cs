using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    [SerializeField] bool loadOnAwake;

    [SerializeField] int targetSceneIndex;
    [SerializeField] int currentSceneIndex;

    [Space(15)]

    [SerializeField] bool transitionOutOnly;
        [SerializeField] float delayOnceLoaded;
    [SerializeField] MMFeedbacks transitionIn;
    [SerializeField] MMFeedbacks transitionOut;
    [SerializeField] float progress;
    Coroutine loadCoroutine;

    [Space(15)]

    [SerializeField] UnityEvent onTransitionStart;
    [SerializeField] UnityEvent onLoadStart;
    [SerializeField] UnityEvent onLoadFinished;
    [SerializeField] UnityEvent onTransitionFinished;

    private void Awake()
    {
        Init();
        AttemptLoadOnAwake();
    }

    private void Init()
    {
        MaintainBetweenScenes();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public int TargetSceneIndex
    {
        set
        {
            targetSceneIndex = value;
        } 
    }

    public void CreateLoader()
    {
      //  GameObject newLoader = Instantiate(UIManager.instance.levelLoaderToMainMenuPrefab);
      //  if (newLoader.TryGetComponent(out LevelLoader levelLoader))
      //  {
         //   UIManager.instance.pauseQuitButton.onClick.AddListener(delegate { CreateLoader(); });
    //    }
    }

    void MaintainBetweenScenes()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangePause(bool value)
    {
        UIManager.instance.ForcePauseGameToFunction(value); 
    }

    public void ChangeMenuToMainMenu()
    {
       UIManager.instance.menuScripts.otherScript.GoToDifferentMenu(MenuScript.OtherMenus.Main);
    }

    void AttemptLoadOnAwake()
    {
        if(loadOnAwake)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        if(loadCoroutine == null)
        {
            if (transitionOutOnly)
            {
                loadCoroutine = StartCoroutine(LoadOutOnly());
            }
            else
            {
                loadCoroutine = StartCoroutine(LoadInAndOut());

            }
        }
        
       
    }

    IEnumerator LoadOutOnly()
    {
        yield return null;
        onTransitionStart.Invoke();
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetSceneIndex, LoadSceneMode.Single);
 
        onLoadStart.Invoke();
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            progress= asyncOperation.progress * 100;
            yield return null;
        }

        onLoadFinished.Invoke();
        progress = asyncOperation.progress * 100;

        yield return new WaitForSecondsRealtime(delayOnceLoaded);
 
        transitionOut.Events.OnComplete.AddListener(delegate { onTransitionFinished.Invoke(); Destroy(gameObject);  });
        transitionOut.Initialization();
        transitionOut.PlayFeedbacks();

    }
    
    IEnumerator LoadInAndOut()
    {
        onTransitionStart.Invoke();
        bool inAnimationComplete = false;
        transitionIn.Events.OnComplete.AddListener(delegate { inAnimationComplete = true; });
        transitionIn.Initialization();
        transitionIn.PlayFeedbacks();

        while (inAnimationComplete == false)
        {
            yield return null;
        }
        onLoadStart.Invoke();
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(targetSceneIndex, LoadSceneMode.Single);

        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            progress= asyncOperation.progress * 100;
            yield return null;
        }
        onLoadFinished.Invoke();
        progress = asyncOperation.progress * 100;

        yield return new WaitForSecondsRealtime(delayOnceLoaded);

        transitionOut.Events.OnComplete.AddListener(delegate { onTransitionFinished.Invoke(); Destroy(gameObject); });
        transitionOut.Initialization();
        transitionOut.PlayFeedbacks();
        
    }


    public MMFeedbacks TransitionOutFeedback
    {
        get
        {
            return transitionOut;
        }
    }

    public MMFeedbacks TransitionInFeedback
    {
        get
        {
            return transitionIn;
        }
    }
}
