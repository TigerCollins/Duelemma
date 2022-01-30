using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionObject : MonoBehaviour
{
    [SerializeField] DimensionVariables dimensionVariables;

    [Header("Spawn Info")]
    [SerializeField] bool spawnedAtRuntime;

    private void Start()
    {
        if(!spawnedAtRuntime)
        {
            Init();
        }
    }

    public void Init()
    {
        SetDimensionFilter(DimensionSwitcher.instance.CurrentDimension());
        DimensionSwitcher.instance.onDimensionChange.AddListener(delegate { SetDimensionFilter(DimensionSwitcher.instance.CurrentDimension()); });
    }

    void SetDimensionFilter(GlobalHelper.Dimensions newDimension)
    {
        switch (newDimension)
        {
            case GlobalHelper.Dimensions.dimensionA:
                dimensionVariables.HideAllDimensionObjects(dimensionVariables.dimensionBObjects);
                dimensionVariables.ShowAllDimensionObjects(dimensionVariables.dimensionAObjects);
                break;
            case GlobalHelper.Dimensions.dimensionB:
                dimensionVariables.HideAllDimensionObjects(dimensionVariables.dimensionAObjects);
                dimensionVariables.ShowAllDimensionObjects(dimensionVariables.dimensionBObjects);
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class DimensionVariables
{
    public List<GameObject> dimensionAObjects;
    public List<GameObject> dimensionBObjects;

    public void HideAllDimensionObjects(List<GameObject> listOfObjects)
    {
        foreach (GameObject item in listOfObjects)
        {
            item.SetActive(false);
        }
    }

    public void ShowAllDimensionObjects(List<GameObject> listOfObjects)
    {
        foreach (GameObject item in listOfObjects)
        {
            item.SetActive(true);
        }
    }
}
