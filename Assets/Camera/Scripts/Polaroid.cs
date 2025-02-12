/*
* Author: Jacie Thoo Yixuan
* Date: 26/1/2025
* Description: Script for polaroid camera to take photos and uploading to supabase
* From Unity Create with VR: Unit 3, Section 2.2
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Polaroid : MonoBehaviour
{
    /// <summary>
    /// Prefab for the photo frame 
    /// </summary>
    public GameObject photoPrefab = null;

    /// <summary>
    /// Renderer for the camera screen
    /// </summary>
    public MeshRenderer screenRenderer = null;

    /// <summary>
    /// The spawn location of the printed photo
    /// </summary>
    public Transform spawnLocation = null;

    /// <summary>
    /// Supabase link
    /// </summary>
    public string supabaseUrl = "https://qabrcgzafrzbwrtrezqc.supabase.co";

    /// <summary>
    /// Supabase anon key (public)
    /// </summary>
    public string supabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFhYnJjZ3phZnJ6YndydHJlenFjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzYzNTE1MjksImV4cCI6MjA1MTkyNzUyOX0.T7CTt9sVsRIg_zhrUokttmz_FDqeRT4Cocw9vDxqTfM";
    
    /// <summary>
    /// Bucket name for where the images are stored
    /// </summary>
    public string bucketName = "images";

    /// <summary>
    /// Folder name (Local)
    /// </summary>
    private const string CAMERA_FOLDER = "PolaroidPhotos";

    /// <summary>
    /// Folder name (in supabase bucket)
    /// </summary>
    private const string UPLOAD_FOLDER = "cameraPhotos";

    /// <summary>
    /// Path for saving image (local)
    /// </summary>
    private string polaroidsFolder;

    /// <summary>
    /// Camera (viewfinder)
    /// </summary>
    private Camera renderCamera = null;

    /// <summary>
    /// Reference to MyDatabase script
    /// </summary>
    public MyDatabase myDatabase;

    /// <summary>
    /// Initialise the camera 
    /// </summary>
    private void Awake()
    {
        renderCamera = GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// Turns off camera at the start, gets folder for saving photos
    /// </summary>
    private void Start()
    {
        CreateRenderTexture();
        TurnOff();

        polaroidsFolder = Path.Combine(Application.persistentDataPath, "PolaroidScreenshots");
        if (!Directory.Exists(polaroidsFolder))
        {
            Directory.CreateDirectory(polaroidsFolder);
            Debug.Log($"Created folder: {polaroidsFolder}");
        }
    }

    /// <summary>
    /// Create render texture
    /// </summary>
    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(797, 448, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    /// <summary>
    /// Takes photo with the camera in the scene
    /// </summary>
    public async void TakePhoto()
    {
        Photo newPhoto = CreatePhoto();
        Texture2D photoTexture = RenderCameraToTexture(renderCamera);
        newPhoto.SetImage(photoTexture);

        // Save and upload the image
        await SaveAndUploadPhoto(photoTexture);
    }

    /// <summary>
    /// Spawns photo at the spawn location
    /// </summary>
    /// <returns></returns>
    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        return photoObject.GetComponent<Photo>();
    }

    /// <summary>
    /// Save photo to local storage and upload to supabase
    /// From DDA Wk14 slides
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    private async Task SaveAndUploadPhoto(Texture2D texture)
    {
        string fileName = $"Polaroid_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string filePath = Path.Combine(polaroidsFolder, fileName);

        // Save as png file
        byte[] imageData = texture.EncodeToPNG();
        await File.WriteAllBytesAsync(filePath, imageData);
        Debug.Log($"Polaroid photo saved to: {filePath}");

        // Upload to Supabase
        await UploadFileUsingPost(filePath);
    }

    /// <summary>
    /// Upload file to supabase
    /// From DDA Wk14 slides
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public async Task UploadFileUsingPost(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"File does not exist: {filePath}");
            return;
        }
        byte[] fileData = File.ReadAllBytes(filePath);
        string fileName = Path.GetFileName(filePath);
        string uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{UPLOAD_FOLDER}/{fileName}";

        try
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", fileData, fileName, "image/png");
            using (UnityWebRequest request = UnityWebRequest.Post(uploadUrl, form))
            {
                request.SetRequestHeader("Authorization", $"Bearer {supabaseAnonKey}");
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"File uploaded successfully: {fileName}");

                    // Public url
                    string publicUrl = $"{supabaseUrl}/storage/v1/object/public/{bucketName}/{UPLOAD_FOLDER}/{fileName}";

                    // Save url to supabase
                    myDatabase.AddImage(publicUrl);
                }
                else
                {
                    Debug.LogError($"Upload failed: {request.error}");
                    Debug.LogError($"Response: {request.downloadHandler.text}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error uploading file: {ex.Message}");
        }
    }

    /// <summary>
    /// Renders whatever the camera in the scene shows as Texture2D
    /// </summary>
    /// <param name="camera"></param>
    /// <returns></returns>
    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(797, 448, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 797, 448), 0, 0);
        photo.Apply();

        return photo;
    }

    /// <summary>
    /// Turns on camera and shows the screen
    /// </summary>
    public void TurnOn()
    {
        renderCamera.enabled = true;
        screenRenderer.material.color = Color.white;
    }

    /// <summary>
    /// Turns off camera and disable the screen
    /// </summary>
    public void TurnOff()
    {
        renderCamera.enabled = false;
        screenRenderer.material.color = Color.black;
    }
}
