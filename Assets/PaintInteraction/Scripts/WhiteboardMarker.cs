/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles marker interactions with the whiteboard.
 */

using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    // Serialize fields for marker's tip, size, and brush color tag
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize;
    [SerializeField] private string brushColorTag;

    /// <summary>
    /// Renderer for marker's tip
    /// </summary>
    private Renderer _renderer; 

    /// <summary>
    /// Array for storing the color of the brush
    /// </summary>
    private Color[] colors;

    /// <summary>
    /// Height of the tip
    /// </summary>
    private float tipHeight; // Height of the tip

    /// <summary>
    /// Reference to whiteboard object
    /// </summary>
    private Whiteboard whiteboard;

    /// <summary>
    /// Hit from raycasting
    /// </summary>
    private RaycastHit touch;

    /// <summary>
    /// Touch positions for drawing
    /// </summary>
    private Vector2 touchPos, lastTouchPos;

    /// <summary>
    /// Whether there was a touch on the previous frame
    /// </summary>
    private bool touchLastFrame;

    /// <summary>
    /// Rotation of the marker during last touch
    /// </summary>
    private Quaternion lastTouchRot;

    // Start is called before the first frame update
    void Start()
    {
        // Get renderer for the marker's tip
        _renderer = tip.GetComponent<Renderer>();

        // Create color array for the brush size
        colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();

        // Get the height of the marker tip
        tipHeight = tip.localScale.y; 
    }

    /// <summary>
    /// Call the draw function each frame
    /// </summary>
    void Update()
    {
        Draw();
    }

    /// <summary>
    /// Drawing Interaction
    /// </summary>
    private void Draw()
    {
        // Cast a ray upwards from the tip of the marker
        if (Physics.Raycast(tip.position, transform.up, out touch, tipHeight))
        {
            // Check if the ray hit the whiteboard
            if (touch.transform.CompareTag("Whiteboard"))
            {
                // If the whiteboard is null, get its reference
                if (whiteboard == null)
                {
                    whiteboard = touch.transform.GetComponent<Whiteboard>();
                }

                // Check for correct brush color and show warning
                if (whiteboard.requiredColorTag != brushColorTag)
                {
                    // Show warning if color doesn't match
                    GameManager.Instance.ShowWarning("");

                    return; // Exit if the color is incorrect
                }

                // Get texture coordinates of the touch
                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y); 

                // Calculate the starting point for drawing based on touch position
                var x = (int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2));
                var y = (int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2));

                // Clamp the starting point to stay within texture bounds
                x = Mathf.Clamp(x, 0, (int)whiteboard.textureSize.x - penSize);
                y = Mathf.Clamp(y, 0, (int)whiteboard.textureSize.y - penSize);

                // Paint on the texture with the brush size and color
                if (touchLastFrame)
                {
                    whiteboard.texture.SetPixels(x, y, penSize, penSize, colors);

                    // Smooth interpolation for drawing strokes
                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        // Interpolate between the current and last touch positions
                        var lerpPos = Vector2.Lerp(lastTouchPos, touchPos, f);
                        var lerpX = Mathf.Clamp((int)(lerpPos.x * whiteboard.textureSize.x), 0, (int)whiteboard.textureSize.x - penSize);
                        var lerpY = Mathf.Clamp((int)(lerpPos.y * whiteboard.textureSize.y), 0, (int)whiteboard.textureSize.y - penSize);

                        // Apply interpolated points to the texture
                        whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                    }

                    // Set the marker's rotation to the previous rotation
                    transform.rotation = lastTouchRot;

                    // Apply changes to the texture
                    whiteboard.texture.Apply();

                    // Let the whiteboard know that painting is complete
                    whiteboard.PaintCompleted(); 
                }

                // Update the last touch position
                lastTouchPos = touchPos;

                // Update the last touch rotation
                lastTouchRot = transform.rotation;

                // Set touchLastFrame to true
                touchLastFrame = true; 
                return;
            }
        }

        // Reset the whiteboard reference if no hit
        whiteboard = null;

        // Reset touchLastFrame if no interaction occurred
        touchLastFrame = false;
    }
}