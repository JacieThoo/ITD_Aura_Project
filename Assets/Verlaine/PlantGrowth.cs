using UnityEngine;
using System.Collections;

public class PlantGrowth : MonoBehaviour
{
    public GameObject vegObject;
    public float growTime = 10f; // 10 secs for now only
    private bool planted = false;
    private bool watered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!planted && collision.gameObject.CompareTag("Ground"))
        {
            planted = true;
            Debug.Log("Seed has been planted");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && planted && !watered)
        {
            watered = true;
            Debug.Log("Plant has been watered! Growth starting");
            StartCoroutine(Grow());
        }
    }


    private IEnumerator Grow()
    {
        Debug.Log("waiting for " + growTime + " seconds to grow");
        yield return new WaitForSeconds(growTime);
        GameObject newPlant = Instantiate(vegObject, transform.position, Quaternion.identity);
        Debug.Log("Plant has grown");
        Destroy(gameObject);
    }
}