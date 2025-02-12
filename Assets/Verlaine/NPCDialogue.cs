/*
* Author: Verlaine Ong Xin Yi
* Date: 31/1/2025
* Description: For NPC voice 
*/
using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Text UI for displaying dialogue
    public AudioSource audioSource; // AudioSource for playing voice clips

    public string npcDialogue; // Editable text in Inspector
    public AudioClip voiceClip; // NPC voice clip
    public float displayTime = 2f; // Time to display each line
    public NPCDialogue nextDialogue; 
    public void NpcDialogue() // Called by UI Button
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        // Display current dialogue
        dialogueText.text = npcDialogue;

        // Play voice clip if assigned
        if (voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();
        }

        // Wait for the displayTime before moving to next dialogue
        yield return new WaitForSeconds(displayTime);

        // If there's a next dialogue, show it
        if (nextDialogue != null)
        {
            nextDialogue.NpcDialogue(); // Trigger next dialogue
        }
        else
        {
            dialogueText.text = ""; // Clear text after last dialogue
        }
    }
}
