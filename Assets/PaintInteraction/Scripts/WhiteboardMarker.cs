using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize = 5;
    private Renderer _renderer;
    private Color[] colors;
    private float tipHeight;
    private RaycastHit touch;
    private Whiteboard whiteboard;
    private Vector2 touchPos, lastTouchPos;
    private bool touchLastFrame;
    private Quaternion lastTouchRot;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = tip.GetComponent<Renderer>();
        colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        tipHeight = tip.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(tip.position, transform.up, out touch, tipHeight))
        {
            // Check if the object hit by the raycast has the "Whiteboard" tag
            if (touch.transform.CompareTag("Whiteboard"))
            {
                if (whiteboard == null)
                {
                    whiteboard = touch.transform.GetComponent<Whiteboard>();
                }

                // Get the names of the brush and the whiteboard
                string brushName = gameObject.name; // Brush name (e.g., GreenPaintBrush)
                string whiteboardName = touch.transform.name; // Whiteboard name (e.g., GreenBoard)

                // Compare the brush's name with the whiteboard's name (ignoring the "PaintBrush" part)
                if (!whiteboardName.Contains(brushName.Replace("PaintBrush", "")))
                {
                    GameManager.Instance.ShowWarning();
                    return; 
                }

                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);
                var x = (int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2));
                var y = (int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2));

                // Ensure the coordinates are within valid bounds
                x = Mathf.Clamp(x, 0, (int)whiteboard.textureSize.x - penSize);
                y = Mathf.Clamp(y, 0, (int)whiteboard.textureSize.y - penSize);

                // Prevent writing outside the texture area
                if (y < 0 || y > whiteboard.textureSize.y || x < 0 || x > whiteboard.textureSize.x)
                    return;

                if (touchLastFrame)
                {
                    whiteboard.texture.SetPixels(x, y, penSize, penSize, colors);
                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(lastTouchPos.y, y, f);

                        // Ensure the lerped values are within bounds
                        lerpX = Mathf.Clamp(lerpX, 0, (int)whiteboard.textureSize.x - penSize);
                        lerpY = Mathf.Clamp(lerpY, 0, (int)whiteboard.textureSize.y - penSize);

                        whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                    }
                    transform.rotation = lastTouchRot;
                    whiteboard.texture.Apply();
                }
                lastTouchPos = new Vector2(x, y);
                lastTouchRot = transform.rotation;
                touchLastFrame = true;
                return;
            }
        }
        whiteboard = null;
        touchLastFrame = false;
    }
}
