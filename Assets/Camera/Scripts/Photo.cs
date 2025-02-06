/*
* Author: Jacie Thoo Yixuan
* Date: 26/1/2025
* Description: Apply texture to the polaroid photo, handle physics 
* From Unity Create with VR: Unit 3, Section 2.2
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    /// <summary>
    /// Renderer for photo texture
    /// </summary>
    public MeshRenderer imageRenderer = null;

    /// <summary>
    /// Collider for the photo
    /// </summary>
    private Collider currentCollider = null;

    /// <summary>
    /// Reference to applyPhysics script
    /// </summary>
    private ApplyPhysics applyPhysics = null;

    /// <summary>
    /// Initialise the collider and physics script
    /// </summary>
    private void Awake()
    {
        currentCollider = GetComponent<Collider>();
        applyPhysics = GetComponent<ApplyPhysics>();
    }

    /// <summary>
    /// Starts the printing effect of the photo 
    /// </summary>
    private void Start()
    {
        StartCoroutine(EjectOverSeconds(1.5f));
    }

    /// <summary>
    /// Move photo forward like a printing effect
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public IEnumerator EjectOverSeconds(float seconds)
    {
        applyPhysics.DisablePhysics();
        currentCollider.enabled = false;

        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += transform.forward * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentCollider.enabled = true;
    }

    /// <summary>
    /// Set texture of the photo
    /// </summary>
    /// <param name="texture"></param>
    public void SetImage(Texture2D texture)
    {
        imageRenderer.material.color = Color.white;
        imageRenderer.material.mainTexture = texture;
    }

    /// <summary>
    /// Function to enable the physics 
    /// </summary>
    public void EnablePhysics()
    {
        applyPhysics.EnablePhysics();
        transform.parent = null;
    }
}
