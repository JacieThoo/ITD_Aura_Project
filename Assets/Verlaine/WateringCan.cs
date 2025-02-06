using UnityEngine;

public class WateringCanController : MonoBehaviour
{
    public ParticleSystem waterEffect;  // particle system
    private bool isOnGround = false; // Track if it's on the ground

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground1")) 
        {
            isOnGround = true;
            StopWater();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground1"))
        {
            isOnGround = false;
            StartWater();
        }
    }

    void StartWater()
    {
        if (!waterEffect.isPlaying)
        {
            waterEffect.Play();
        }
    }

    void StopWater()
    {
        if (waterEffect.isPlaying)
        {
            waterEffect.Stop();
        }
    }
}

