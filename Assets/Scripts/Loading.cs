/*
* Author: Tan Ting Yu Gwyneth 
* Date: 7/11/2024
* Description: This file is to handle loading screen after logging in or signing in
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Loading : MonoBehaviour
{
    /// <summary>
    /// Reference to loading screen
    /// </summary>
    public GameObject loadingScreen;

    /// <summary>
    /// Duration for the loading screen to be displayed
    /// </summary>
    public float loadingDuration = 1f; 

    /// <summary>
    /// Calls the screen to load
    /// </summary>
    public void ShowLoadingScreen()
    {
        StartCoroutine(ShowLoadingScreenCoroutine());
    }

    /// <summary>
    /// Loads the screen and handle the timing 
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowLoadingScreenCoroutine()
    {
        loadingScreen.SetActive(true); // Show loading screen
        float elapsedTime = 0f;
        while (elapsedTime < loadingDuration) // Simulate loading by waiting for the specified duration
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        loadingScreen.SetActive(false); // Hide loading screen after the simulated loading is complete
    }
}
