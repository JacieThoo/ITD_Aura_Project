/*
 * Author: Hoo Ying Qi Praise
 * Date: 
 * Description: 
 */

using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize;
    [SerializeField] private string brushColorTag;

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
            if (touch.transform.CompareTag("Whiteboard"))
            {
                if (whiteboard == null)
                {
                    whiteboard = touch.transform.GetComponent<Whiteboard>();
                }

                // Check for correct brush color
                if (whiteboard.requiredColorTag != brushColorTag)
                {
                    GameManager.Instance.ShowWarning("Are you sure it is the right color?");
                    return;
                }


                touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);

                // Calculate the starting point for drawing
                var x = (int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2));
                var y = (int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2));

                // Clamp the starting point to the valid texture bounds
                x = Mathf.Clamp(x, 0, (int)whiteboard.textureSize.x - penSize);
                y = Mathf.Clamp(y, 0, (int)whiteboard.textureSize.y - penSize);

                if (touchLastFrame)
                {
                    whiteboard.texture.SetPixels(x, y, penSize, penSize, colors);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = Mathf.Clamp((int)Mathf.Lerp(lastTouchPos.x, x, f), 0, (int)whiteboard.textureSize.x - penSize);
                        var lerpY = Mathf.Clamp((int)Mathf.Lerp(lastTouchPos.y, y, f), 0, (int)whiteboard.textureSize.y - penSize);
                        whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                    }

                    transform.rotation = lastTouchRot;

                    whiteboard.texture.Apply();
                    whiteboard.PaintCompleted();
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
