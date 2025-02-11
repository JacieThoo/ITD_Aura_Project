/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file applies textures onto assets in the unity scene
*/

using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Networking; 

public class ApplyShirtTexture : MonoBehaviour
{
    /// <summary>
    /// Shirt that displays the profile picture
    /// </summary>
    public GameObject shirt;

    /// <summary>
    /// Applies the image onto the texture
    /// </summary>
    /// <param name="currentUserId"></param>
    public async void ApplyTexture(string imageUrl)
    {
        if (imageUrl != "")
        {
            Debug.Log(imageUrl);

            Texture2D texture = await DownloadImage(imageUrl);
            if (shirt != null)
            {
                Renderer renderer = shirt.GetComponent<Renderer>(); // Get Renderer
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.mainTexture = texture; // Apply texture to material
                    Debug.Log("Texture applied to the shirt");
                }
                else
                {
                    Debug.LogError("Renderer or material not found on the shirt GameObject.");
                }
            }
            else
            {
                Debug.LogError("Shirt GameObject is null.");
            }
        }
    }
   
    /// <summary>
    /// Downloads the image for unity to access
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private async Task<Texture2D> DownloadImage(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Image downloaded successfully!");
                return DownloadHandlerTexture.GetContent(request);
            }
            else
            {
                Debug.LogError($"Failed to download image: {request.error}");
                return null;
            }
        }
    }
}
