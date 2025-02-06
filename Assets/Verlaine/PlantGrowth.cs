/*
* Author: Verlaine Ong Xin Yi
* Date: 31/1/2025
* Description: For growing plant 
*/
using UnityEngine;
using TMPro;
using System.Collections;

public class PlantGrowth : MonoBehaviour
{
    public GameObject vegObject; // The plant that will grow
    public float growTime = 10f; // Time until growth
    public TextMeshProUGUI timerUI; // UI text to show countdown

    private bool planted = false; //not planted initially
    private bool watered = false; // not watered initailly
    private float timeRemaining; // the time remaining to grow

    /// <summary>
    /// Function to count the remaining time for the crop to grow
    /// </summary>
    private void Start()
    {
        timeRemaining = growTime;
    }

    /// <summary>
    /// Function to show the seed touch the ground object
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (!planted && collision.gameObject.CompareTag("Ground"))
        {
            planted = true;
            Debug.Log("Seed planted");
        }
    }

    /// <summary>
    /// Function to show the water touch the seed
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && planted && !watered)
        {
            watered = true;
            Debug.Log("Watered and Growth started.");
            StartCoroutine(GrowPlant());
        }
    }

    private IEnumerator GrowPlant()
    {
        while (timeRemaining > 0)
        {
            UpdateTimerUI();
            yield return new WaitForSeconds(1f); //Update every sec
            timeRemaining--;
        }

        // Growth complete
        timerUI.text = "Ready!";
        Instantiate(vegObject, transform.position, Quaternion.identity); //Spawn the plant 
        Debug.Log("Plant has grown!");
        Destroy(gameObject); //Destory the seed
    }

    /// <summary>
    /// Function to show the remaining time to groq crop
    /// </summary>
    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerUI.text = $"{minutes:D2}:{seconds:D2}";
    }
}

