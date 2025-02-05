using UnityEngine;

public class DigHole : MonoBehaviour
{
    public GameObject newTerrain; 

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("VRObject")) 
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
