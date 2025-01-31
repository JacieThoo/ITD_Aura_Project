using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Polaroid : MonoBehaviour
{
    public GameObject photoPrefab = null;
    public MeshRenderer screenRenderer = null;
    public Transform spawnLocation = null;

    public string supabaseUrl = "https://qabrcgzafrzbwrtrezqc.supabase.co";
    public string supabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InFhYnJjZ3phZnJ6YndydHJlenFjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzYzNTE1MjksImV4cCI6MjA1MTkyNzUyOX0.T7CTt9sVsRIg_zhrUokttmz_FDqeRT4Cocw9vDxqTfM";
    public string bucketName = "images";

    private const string CAMERA_FOLDER = "PolaroidPhotos";
    private const string UPLOAD_FOLDER = "cameraPhotos";

    private string polaroidsFolder;

    private Camera renderCamera = null;

    private void Awake()
    {
        renderCamera = GetComponentInChildren<Camera>();
    }

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

    private void CreateRenderTexture()
    {
        RenderTexture newTexture = new RenderTexture(256, 256, 32, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
        newTexture.antiAliasing = 4;

        renderCamera.targetTexture = newTexture;
        screenRenderer.material.mainTexture = newTexture;
    }

    public async void TakePhoto()
    {
        Photo newPhoto = CreatePhoto();
        Texture2D photoTexture = RenderCameraToTexture(renderCamera);
        newPhoto.SetImage(photoTexture);

        // Save and upload the image
        await SaveAndUploadPhoto(photoTexture);
    }

    private Photo CreatePhoto()
    {
        GameObject photoObject = Instantiate(photoPrefab, spawnLocation.position, spawnLocation.rotation, transform);
        return photoObject.GetComponent<Photo>();
    }

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

    private Texture2D RenderCameraToTexture(Camera camera)
    {
        camera.Render();
        RenderTexture.active = camera.targetTexture;

        Texture2D photo = new Texture2D(256, 256, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        photo.Apply();

        return photo;
    }

    public void TurnOn()
    {
        renderCamera.enabled = true;
        screenRenderer.material.color = Color.white;
    }

    public void TurnOff()
    {
        renderCamera.enabled = false;
        screenRenderer.material.color = Color.black;
    }
}
