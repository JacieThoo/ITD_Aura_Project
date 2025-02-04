using UnityEngine;

public class DigHole : MonoBehaviour
{
    public GameObject newTerrain; // Assign TerrainB in Inspector

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("VRObject")) // Check the tag of the GameObject
        {
            SwapTerrain();
        }
    }

    void SwapTerrain()
    {
        if (newTerrain != null)
        {
            newTerrain.SetActive(true);  // Enable the new terrain
            gameObject.SetActive(false); // Disable the old terrain
        }
    }
}
