using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReaction : MonoBehaviour
{

    public Canvas speechBubble;
    public Canvas interactUI;
    public Canvas exclaimationMark;
    //public Animator animator; 

    /* void Start()
    {
        // Ensure the NPC starts in the Idle state
        animator.Play("Idle");
    } */

    private void Start()
    {
        speechBubble.gameObject.SetActive(false);
        interactUI.gameObject.SetActive(true);
        exclaimationMark.gameObject.SetActive(true);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("C") || collision.gameObject.CompareTag("P"))
        {
            // Trigger the transition to the React animation
            //animator.SetTrigger("PlayReact");

            // Destroy the object that touched the NPC
            StartCoroutine(DestroyAfterDelay(collision));

            speechBubble.gameObject.SetActive(true);
            interactUI.gameObject.SetActive(false);
            exclaimationMark.gameObject.SetActive(false);
        }
    }

    // Coroutine to destroy the object after a delay
    private IEnumerator DestroyAfterDelay(Collision collision)
    {
        // Wait for a brief period
        yield return new WaitForSeconds(0.1f);
        
        
        if (collision.gameObject != null) // To prevent the error
        {
            Destroy(collision.gameObject);
        }

        yield return new WaitForSeconds(2f); 

        // Hide the speech bubble
        speechBubble.gameObject.SetActive(false);
    }
}



