/*
* Author: Verlaine Ong Xin Yi
* Date: 31/1/2025
* Description: For Diging hole , hiding 3D plane
*/
using UnityEngine;

public class DigHole : MonoBehaviour
{
    public GameObject newTerrain; // Show the new terrain with the hole

    /// <summary>
    /// This function will check if the shovel touch the plane.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("VRObject")) 
        {
            SwapTerrain(); //hide the top terrain
        }
    }

    /// <summary>
    /// This function will hide the top terrain layer
    /// </summary>
    void SwapTerrain()
    {
        if (newTerrain != null)
        {
            newTerrain.SetActive(true);  // Enable the new terrain
            gameObject.SetActive(false); // Disable the old terrain
        }
    }
}
