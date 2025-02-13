/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles the whiteboard texture and checks if it is fully painted.
 */

using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 6144);

    public MyDatabase myDatabase;

    private int totalPixels;
    private int paintedPixels;
    public bool isFullyPainted = false;
    private const float threshold = 0.95f; // Coverage needed

    void Start()
    {
        if (myDatabase == null)
        {
            Debug.LogError("MyDatabase reference not set in Whiteboard script.");
            return; // Early exit to avoid further issues
        }

        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;

        totalPixels = (int)(textureSize.x * textureSize.y);
        paintedPixels = 0;
    }

    public void RegisterPaintedPixels(int newPixels)
    {
        if (isFullyPainted) return; // No need to check if already complete

        paintedPixels += newPixels;

        // Add aura points whenever paint touches the canvas
        myDatabase.UpdateAura(newPixels / 100);

        // Calculate current coverage
        float coverage = (float)paintedPixels / totalPixels;

        // If coverage is over the threshold, set the whiteboard as fully painted
        if (coverage >= threshold)
        {
            isFullyPainted = true;
            OnWhiteboardFullyPainted();
        }
    }

    private void OnWhiteboardFullyPainted()
    {
        // Additional aura points when fully painted
        int auraValue = 100;
        myDatabase.UpdateAura(auraValue);

        Debug.Log(gameObject.name + " is fully painted, +100 ");
    }
}