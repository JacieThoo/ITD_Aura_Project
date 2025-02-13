/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles Complete Button Interaction
 */

using UnityEngine;
using TMPro;
using System.Collections;

public class CompleteButton : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to dialogue text

    public AudioSource completedClip; // Clip for the "What a piece of art!" voice
    public AudioSource incompleteClip; // Clip for the "Painting not done" voice

    public string incomplete; // Text for incomplete message
    public string complete; // Text for complete message

    public Whiteboard whiteboard; // Reference to Whiteboard script to check if painted

    public GameObject confettiEffect; // Reference to the confetti particle system
    public float confettiDuration = 10f; // Duration for confetti

    public void OnCompleteButtonPressed()
    {
        if (whiteboard != null)
        {
            if (whiteboard.isFullyPainted) // Check if the whiteboard is fully painted
            {
                StartCoroutine(CompletePainting());
            }
            else
            {
                DisplayIncompleteMessage();
            }
        }
        else
        {
            Debug.LogError("Whiteboard reference not set.");
        }
    }

    private IEnumerator CompletePainting()
    {
        // Display the complete message
        dialogueText.text = complete;

        // Play the completed sound clip
        if (completedClip != null)
        {
            completedClip.Play();
        }

        // Activate confetti effect
        if (confettiEffect != null)
        {
            confettiEffect.SetActive(true);
        }

        // Wait for the duration of the confetti effect
        yield return new WaitForSeconds(confettiDuration);

        // Deactivate confetti effect
        if (confettiEffect != null)
        {
            confettiEffect.SetActive(false);
        }

        // Clear the dialogue after the confetti effect
        dialogueText.text = "";
    }

    private void DisplayIncompleteMessage()
    {
        // Display incomplete message
        dialogueText.text = incomplete;

        // Play the incomplete sound clip
        if (incompleteClip != null)
        {
            incompleteClip.Play();
        }
    }
}
