using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCompletion : MonoBehaviour
{
    public static CheckCompletion Instance;

    public Canvas cleanWaterExclamation;
    public Canvas buildWellExclamation;
    public Canvas recyclingExclamation;

    public bool cleanWaterDone = false;
    public bool buildWellDone = false;
    public bool recyclingDone = false;

    public int totalItemsRecycled = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisableExclamation()
    {
        if (cleanWaterDone)
        {
            cleanWaterExclamation.gameObject.SetActive(false);
        }
        else if (buildWellDone)
        {
            buildWellExclamation.gameObject.SetActive(false);
        }
        else if (recyclingDone)
        {
            recyclingExclamation.gameObject.SetActive(false);
        }
    }
    public void AddRecycledItem()
    {
        totalItemsRecycled++;

        if (totalItemsRecycled >= 8)
        {
            recyclingDone = true;
            DisableExclamation();
        }
    }

}
