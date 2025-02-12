/*
* Author: Jacie Thoo Yixuan
* Date: 6/2/2025
* Description: Build well interaction 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWell : MonoBehaviour
{
    /// <summary>
    /// Database to update aura
    /// </summary>
    public MyDatabase myDatabase;

    public int bricksPlaced = 0;

    /// <summary>
    /// Updates aura when player places the brick
    /// Assign and call function in xr grab manager event
    /// </summary>
    public void PlaceBrick()
    {
        if (bricksPlaced < 3)
        {
            myDatabase.UpdateAura(40);
            bricksPlaced++;
        }
        else
        {
            Debug.Log("Already placed 3 bricks");
        }
    }
}
