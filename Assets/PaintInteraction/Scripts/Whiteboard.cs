/*
 * Author: Hoo Ying Qi Praise
 * Date: 
 * Description: 
 */

using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    private int paintedPixels;
    private int totalPixels;

    public string requiredColorTag;

    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;

        totalPixels = (int)(textureSize.x * textureSize.y);
    }

    public void PaintCompleted()
    {
        paintedPixels = 0;
        var pixels = texture.GetPixels();

        foreach (var pixel in pixels)
        {
            if (pixel.a > 0)
            {
                paintedPixels++;
            }
        }

        if (paintedPixels >= totalPixels)
        {
            Debug.Log("Whiteboard painted!");
            GameManager.Instance.AddScore(100);
        }
    }
}
