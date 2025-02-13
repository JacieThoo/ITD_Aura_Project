/*
* Author: Verlaine Ong Xin Yi
* Date: 31/1/2025
* Description: This script will turn the water effect on and off depending on wether is on the ground or not.
*/
using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    public ParticleSystem waterEffect;  // water particle system
    private bool isOnGround = false; // Track if it's on the ground

    /// <summary>
    /// This function will stop the water vfx from playing if the watering can is on the ground
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground1")) 
        {
            isOnGround = true; // watering can on the ground
            StopWater(); //stop playing
        }
    }
    /// <summary>
    /// This function will play the water vfx if the watering can is off the ground
    /// </summary>
    /// <param name="collision"></param> 
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isOnGround = false; // watering can not on the ground
            StartWater(); // start playe+ing
        }
    }
     /// <summary>
     /// This function playing the water vfx 
     /// </summary>
    void StartWater()
    {
        if (!waterEffect.isPlaying)
        {
            waterEffect.Play();
        }
    }
    /// <summary>
    /// This function stop playing the water vfx
    /// </summary>
    void StopWater()
    {
        if (waterEffect.isPlaying)
        {
            waterEffect.Stop();
        }
    }
}

