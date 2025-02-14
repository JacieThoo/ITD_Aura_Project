/*
* Author: Jacie Thoo Yixuan
* Date: 6/2/2025
* Description: Recycling interaction 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Recycling : MonoBehaviour
{
    /// <summary>
    /// Stores the category of the recycling bin (MUST BE SAME AS TAG)
    /// </summary>
    public string category;

    /// <summary>
    /// Database to update aura
    /// </summary>
    public MyDatabase myDatabase;

    public int itemsRecycled = 0;

    /// <summary>
    /// Checks tag of item that enters the trigger area
    /// Adds aura if tag matches category
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(category))
        {
            Debug.Log("Correct category");
            myDatabase.UpdateAura(40);
            itemsRecycled++;
            CheckCompletion.Instance.AddRecycledItem();

            // So the player cannot keep looping 
            other.gameObject.tag = "Untagged";
        }
        else
        {
            Debug.Log("Wrong category");
        }
    }

    public void CheckRecyclingStatus()
    {
        if (itemsRecycled == 8)
        {
            CheckCompletion.Instance.recyclingDone = true;
            CheckCompletion.Instance.DisableExclamation();
        }
    }
}
