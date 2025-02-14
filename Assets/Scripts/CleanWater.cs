using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanWater : MonoBehaviour
{
    public GameObject dirtyWater;
    public GameObject cleanWater;

    public int trashCleaned = 0;

    public MyDatabase myDatabase;

    public void Start()
    {
        dirtyWater.SetActive(true);
        cleanWater.SetActive(false);
    }

    public void WaterCleaned()
    {
        dirtyWater.SetActive(false);
        cleanWater.SetActive(true);
        CheckCompletion.Instance.cleanWaterDone = true;
        CheckCompletion.Instance.DisableExclamation();
        myDatabase.UpdateAura(80);
    }

    private void CheckRiverStatus()
    {
        if (trashCleaned == 3)
        {
            WaterCleaned();
        }
        else
        {
            Debug.Log("Need to clean more trash to clean river.");
        }
    }

    public void CleanTrash()
    {
        if (trashCleaned < 3)
        {
            trashCleaned++;
            CheckRiverStatus();
        }
    }
}
