using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; // For UI Image
using UnityEngine.Networking; // For UnityWebRequest

public class ApplyTextureOnAppStart : MonoBehaviour
{
    public string imageUrl = ""; // URL of the image to download
    public string supabaseUrl = ""; // Supabase URL
    public string supabaseAnonKey = ""; // Supabase Anon Key
    public string bucketName = ""; // Supabase bucket name
    public string uploadFolder = "images"; // Folder in the bucket to upload files

    public Image targetImageUI; // UI Image to display the downloaded image

    private const string UploadType = "image/png";

    private async void Start()
    {
        // Step 1: Download the image
        Texture2D texture = await DownloadImage(imageUrl);
        if (texture != null)
        {
            // Display the image in the UI Image
            if (targetImageUI != null)
            {
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                targetImageUI.sprite = sprite;
            }

            // Step 2: Encode and upload the image
            string fileName = $"Image_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            await UploadImageToSupabase(texture, fileName);
        }
    }

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

    private async Task UploadImageToSupabase(Texture2D texture, string fileName)
    {
        try
        {
            // Encode texture to PNG
            byte[] fileData = texture.EncodeToPNG();

            // Create Supabase upload URL
            string uploadUrl = $"{supabaseUrl}/storage/v1/object/{bucketName}/{uploadFolder}/{fileName}";

            Debug.Log($"Uploading to URL: {uploadUrl}");

            // Create a multipart form to upload the file
            WWWForm form = new WWWForm();
            form.AddBinaryData("file", fileData, fileName, UploadType);

            using (UnityWebRequest request = UnityWebRequest.Post(uploadUrl, form))
            {
                // Set required headers
                request.SetRequestHeader("Authorization", $"Bearer {supabaseAnonKey}");

                // Send the request
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"File uploaded successfully \u2705: {fileName}");
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
}