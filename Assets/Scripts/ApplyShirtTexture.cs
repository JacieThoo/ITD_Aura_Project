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
    /// Supabase anon key
    /// </summary>
    public string supabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFhYnJjZ3phZnJ6YndydHJlenFjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzYzNTE1MjksImV4cCI6MjA1MTkyNzUyOX0.T7CTt9sVsRIg_zhrUokttmz_FDqeRT4Cocw9vDxqTfM"; 
    
    /// <summary>
    /// Supabase url
    /// </summary>
    public string supabaseUrl = "https://qabrcgzafrzbwrtrezqc.supabase.co"; 

    /// <summary>
    /// Reference to bucket name
    /// </summary>
    public string bucketName = "images"; // Supabase bucket name

    /// <summary>
    /// Reference to folder in bucket that stores the shirt textures
    /// </summary>
    public string uploadFolder = "profilePhotos"; // Folder in the bucket to upload files

    /// <summary>
    /// Shirt that displays the profile picture
    /// </summary>
    public GameObject shirt;

    public async void ApplyTexture(string currentUserId)
    {
        if (currentUserId != "")
        {
            string imageUrl = $"{supabaseUrl}/storage/v1/object/public/{bucketName}/{uploadFolder}/{currentUserId}";
            Debug.Log(imageUrl);


            Texture2D texture = await DownloadImage(ImageUrl);
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
    /// Gets the most updated link by getting the current user id
    /// </summary>
    public string ImageUrl
    {
        get
        {
            return $"{supabaseUrl}/storage/v1/object/public/{bucketName}/{uploadFolder}/{authManager.currentUserId}";
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
