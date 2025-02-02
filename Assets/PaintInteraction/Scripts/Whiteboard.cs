/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles the whiteboard texture and checks if it is fully painted.
 */

using UnityEngine;
using System.Linq;

public class Whiteboard : MonoBehaviour
{
    /// <summary>
    /// Texture used for drawing on the whiteboard
    /// </summary>
    public Texture2D texture;

    /// <summary>
    /// // Size of the texture
    /// </summary>
    public Vector2 textureSize = new Vector2(2048, 2048);

    /// <summary>
    /// Number of pixels that have been painted
    /// </summary>
    private int paintedPixels;

    /// <summary>
    /// Total number of pixels in the texture
    /// </summary>
    private int totalPixels;

    /// <summary>
    /// The required color tag for the marker
    /// </summary>
    public string requiredColorTag;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Renderer component attached to the object
        var renderer = GetComponent<Renderer>();

        // Create a new texture
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);

        // Initialize the texture with a white background
        Color[] fillPixels = Enumerable.Repeat(Color.white, (int)(textureSize.x * textureSize.y)).ToArray();
        texture.SetPixels(fillPixels); // Set all pixels to white
        texture.Apply(); // Apply changes to the texture

        renderer.material.mainTexture = texture; // Assign the texture to the whiteboard material
        totalPixels = (int)(textureSize.x * textureSize.y); // Calculate the total number of pixels
    }

    /// <summary>
    /// Method to check if the whiteboard is completely painted
    /// </summary>
    public void PaintCompleted()
    {
        paintedPixels = 0; // Reset painted pixel count
        var pixels = texture.GetPixels(); // Get all pixels from the texture

        // Loop through each pixel to check if it's painted
        foreach (var pixel in pixels)
        {
            if (pixel.a > 0 && pixel != Color.white) // Ignore fully transparent or unpainted white pixels
            {
                paintedPixels++; // Increment painted pixel count
            }
        }

        // If at least 99% of the board is painted, consider it completed
        if (paintedPixels >= totalPixels * 0.99f)
        {
            Debug.Log("Whiteboard fully painted!"); // Print completion message to console
            GameManager.Instance.AddScore(100); // Award 100 points to the player
        }
    }
}