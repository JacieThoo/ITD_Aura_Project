/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles the paint
 */

using UnityEngine;
using System.Linq;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform tip;
    [SerializeField] private int penSize;

    private Renderer _renderer;
    private Color[] colors;
    private float tipHeight;
    private RaycastHit touch;
    private Whiteboard whiteboard;
    private Vector2 touchPos, lastTouchPos;
    private bool touchLastFrame;
    private Quaternion lastTouchRot;

    void Start()
    {
        if (tip == null)
        {
            Debug.LogError("Tip transform is not assigned in " + gameObject.name);
            return;
        }

        _renderer = tip.GetComponent<Renderer>();
        if (_renderer == null || _renderer.material == null)
        {
            Debug.LogError("Renderer or material missing " + gameObject.name);
            return;
        }

        colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
        tipHeight = tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (!Physics.Raycast(tip.position, transform.up, out touch, tipHeight))
        {
            whiteboard = null;
            touchLastFrame = false;
            return;
        }

        whiteboard = touch.transform.GetComponent<Whiteboard>();
        if (whiteboard == null) return; // Ensure it's a valid whiteboard

        touchPos = new Vector2(touch.textureCoord.x, touch.textureCoord.y);
        int x = Mathf.Clamp((int)(touchPos.x * whiteboard.textureSize.x - (penSize / 2)), 0, (int)whiteboard.textureSize.x - penSize);
        int y = Mathf.Clamp((int)(touchPos.y * whiteboard.textureSize.y - (penSize / 2)), 0, (int)whiteboard.textureSize.y - penSize);

        if (touchLastFrame)
        {
            int newPixels = penSize * penSize;
            whiteboard.texture.SetPixels(x, y, penSize, penSize, colors);

            for (float f = 0.01f; f < 1.00f; f += 0.01f)
            {
                int lerpX = Mathf.Clamp((int)Mathf.Lerp(lastTouchPos.x, x, f), 0, (int)whiteboard.textureSize.x - penSize);
                int lerpY = Mathf.Clamp((int)Mathf.Lerp(lastTouchPos.y, y, f), 0, (int)whiteboard.textureSize.y - penSize);

                whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, colors);
                whiteboard.RegisterPaintedPixels(newPixels);

                newPixels += penSize * penSize;
            }

            whiteboard.RegisterPaintedPixels(newPixels);
            whiteboard.texture.Apply();
        }

        lastTouchPos = new Vector2(x, y);
        lastTouchRot = transform.rotation;
        touchLastFrame = true;
    }
}
