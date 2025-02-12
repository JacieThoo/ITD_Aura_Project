/*
* Author: Verlaine Ong Xin Yi
* Date: 31/1/2025
* Description: For player giving the crops to NPC and add aura  
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCReaction : MonoBehaviour
{

    public Canvas speechBubble; // Speech Bubble Ui
    public Canvas interactUI; // Interect button
    public Canvas exclaimationMark; // Quest UI
    //public Animator animator; 

    /* void Start()
    {
        // Ensure the NPC starts in the Idle state
        animator.Play("Idle");
    } */

    private int collisionCount = 0; //How many times player give the crops counter


    /// <summary>
    /// This function is to initially set UI that are ON or OFF
    /// </summary>
    private void Start()
    {
        speechBubble.gameObject.SetActive(false);
        interactUI.gameObject.SetActive(true);
        exclaimationMark.gameObject.SetActive(true);
    }

    /// <summary>
    /// This function is use to detect wether the crops touched the NPC or not, destroy after touched
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("C") || collision.gameObject.CompareTag("P"))
        {
            collisionCount++;
            // Trigger the transition to the React animation
            //animator.SetTrigger("PlayReact");

            // Destroy the object that touched the NPC
            StartCoroutine(DestroyAfterDelay(collision));

            speechBubble.gameObject.SetActive(true);
            interactUI.gameObject.SetActive(false);
            exclaimationMark.gameObject.SetActive(false);

           /* if (collisionCount == 1)
            {
                Debug.Log("First Collision");
                //If is first time giving the crops add aura
            } 
           */
        }
    }


    /// <summary>
    /// This Function is Coroutine to destroy the object after a delay
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    private IEnumerator DestroyAfterDelay(Collision collision)
    {
        // Wait for a brief period
        yield return new WaitForSeconds(0.1f);


  
        Destroy(collision.gameObject);
    

        // wait awhile then remove speech
        yield return new WaitForSeconds(2f); 

        // Hide the speech bubble
        speechBubble.gameObject.SetActive(false);
    }
}



