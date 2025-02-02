/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles the whiteboard texture and checks if it is fully painted.
 */

using UnityEngine;
using System.Linq;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);
    // Start is called before the first frame update
    void Start()
    {
        var r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
}