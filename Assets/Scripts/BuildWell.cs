using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWell : MonoBehaviour
{
    // Database to update aura
    public MyDatabase myDatabase;

    public void PlaceBrick()
    {
        myDatabase.UpdateAura(40);
    }
}
