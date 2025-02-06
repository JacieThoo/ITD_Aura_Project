/*
* Author: Jacie Thoo Yixuan
* Date: 26/1/2025
* Description: Deals with the physics for the photo frame
* From Unity Create with VR: Unit 3, Section 2.2
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPhysics : MonoBehaviour
{
    /// <summary>
    /// Reference to rigidbody 
    /// </summary>
    private Rigidbody rigidBody = null;

    /// <summary>
    /// Stores original mode of the collision detection  (so can restore)
    /// </summary>
    private CollisionDetectionMode originalMode = CollisionDetectionMode.Discrete;

    /// <summary>
    /// Initialisation of rigidbody and original collision detection mode
    /// </summary>
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        originalMode = rigidBody.collisionDetectionMode;
    }

    /// <summary>
    /// Enable physics, enable gravity and non kinematic so the photo will fall down
    /// For after player takes the photo out of the printer hole
    /// </summary>
    public void EnablePhysics()
    {
        rigidBody.collisionDetectionMode = originalMode;
        rigidBody.useGravity = true;
        rigidBody.isKinematic = false;
    }

    /// <summary>
    /// Disables gravity and makes it kinematic so it won't fall
    /// For when the picture is being printed
    /// </summary>
    public void DisablePhysics()
    {
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }
}
