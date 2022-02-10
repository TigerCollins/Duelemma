using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{
    [SerializeField] GameObject levelLoaderPrefab;
    [SerializeField] int targetBuildIndex = 3;

    public void LoadNextLevel()
    {
       GameObject newLoader = Instantiate(levelLoaderPrefab);
        if(newLoader.TryGetComponent(out LevelLoader levelLoader))
        {
            levelLoader.TargetSceneIndex = targetBuildIndex;
            levelLoader.LoadScene();
        }
    }
}
