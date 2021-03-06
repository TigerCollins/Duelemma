using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class DimensionSwitcher : MonoBehaviour
{
    internal static DimensionSwitcher instance;

    [SerializeField] bool debugMode;
    [SerializeField] GlobalHelper.Dimensions currentDimension;
    GlobalHelper.Dimensions lastDimension;
    [SerializeField] DimensionEnvironmentHandler environmentHandler;
    [Space(10)]
    public UnityEvent onDimensionChange;
    public UnityEvent onChangeFail;
    public UnityEvent onChangeSucces;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("This is the second DimensionSwitch, it is on '" + gameObject.name + "'. The original (the one used for functionality) is on '" + instance.gameObject.name + "', the other has now been destroyed.");

            instance.environmentHandler.dimensionAObjects.Clear();
            instance.environmentHandler.dimensionBObjects.Clear();
            foreach (GameObject item in environmentHandler.dimensionAObjects)
            {
                instance.environmentHandler.dimensionAObjects.Add(item);
            }

            foreach (GameObject item in environmentHandler.dimensionBObjects)
            {
                instance.environmentHandler.dimensionBObjects.Add(item);
            }
            instance.environmentHandler.ChangeEnvironmentDimension();
            GlobalHelper.instance.DimensionColour(currentDimension);
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        onDimensionChange.AddListener(delegate {environmentHandler.ChangeEnvironmentDimension(); });
        onDimensionChange.AddListener(delegate { GlobalHelper.instance.DimensionColour(currentDimension); });
    }

  

    // Update is called once per frame
    void Update()
    {
        UpdateTickDimensionCheck();
    }

    //THIS IS ONLY FOR DEBUG. NEEDS TO BE REMOVED
    void UpdateTickDimensionCheck()
    {
        
        if (currentDimension != lastDimension && debugMode)
        {
            lastDimension = currentDimension;
            onDimensionChange.Invoke();
        }

        
    }

    public GlobalHelper.Dimensions CurrentDimension()
    {
        return currentDimension;
    }


    public void ChangeDimensions()
    {
        if (currentDimension != GlobalHelper.Dimensions.dimensionA)
        {
            currentDimension = GlobalHelper.Dimensions.dimensionA;
        }

        else
        {
            currentDimension = GlobalHelper.Dimensions.dimensionB;
        }

        onDimensionChange.Invoke();
    }


    public void ChangeDimensionViaInput(InputAction.CallbackContext input)
    {
        if(input.performed)
        {
            if (currentDimension != GlobalHelper.Dimensions.dimensionA)
            {
                currentDimension = GlobalHelper.Dimensions.dimensionA;
            }

            else
            {
                currentDimension = GlobalHelper.Dimensions.dimensionB;
            }
           
            onDimensionChange.Invoke();
        }
       
    }

}

[System.Serializable]
public class DimensionEnvironmentHandler
{
    public List<DimensionObject> environmentDimensionObjects;
    public List<GameObject> dimensionAObjects;
    public List<GameObject> dimensionBObjects;
    public int environmentObjectsActive;

    public void AddToEnvironmentList(DimensionObject newObject)
    {
        environmentDimensionObjects.Add(newObject);
        environmentObjectsActive++;
    }

    public void RemoveFromEnvironmentList(DimensionObject objectToDelete)
    {
        int id = 0;
        foreach (DimensionObject item in environmentDimensionObjects)
        {
            if(item == objectToDelete)
            {
                environmentDimensionObjects.RemoveRange(id, 1);
            }
            id++;
        }
        environmentObjectsActive--;
    }

    public void ChangeEnvironmentDimension()
    {
        switch (DimensionSwitcher.instance.CurrentDimension())
        {
            case GlobalHelper.Dimensions.dimensionA:
                foreach (GameObject item in dimensionAObjects)
                {
                    if(item != null)
                    {
                        item.SetActive(true);
                    }
                    
                }

                foreach (GameObject item in dimensionBObjects)
                {
                    if (item != null)
                    {
                        item.SetActive(false);
                    }
                }
                break;
            case GlobalHelper.Dimensions.dimensionB:
                foreach (GameObject item in dimensionAObjects)
                {
                    if (item != null)
                    {
                        item.SetActive(false);
                    }
                }

                foreach (GameObject item in dimensionBObjects)
                {
                    if (item != null)
                    {
                        item.SetActive(true);
                    }
                }
                break;
            default:
                break;
        }
    }
}
