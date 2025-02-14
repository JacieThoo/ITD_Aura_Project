using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{


    private int enterCount = 0;

    public AudioSource audioSource;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && enterCount == 0) 
        {
            enterCount++;

            audioSource.Play();

        }
    }

}
