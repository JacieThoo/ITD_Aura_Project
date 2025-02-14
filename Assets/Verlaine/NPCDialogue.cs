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

    public Canvas dialogueCanvas;
    public TextMeshProUGUI dialogueText; // Text UI for displaying dialogue
    public AudioSource audioSource; // AudioSource for playing voice clips

    public string npcDialogue; // Editable text in Inspector
    public AudioClip voiceClip; // NPC voice clip
    public NPCDialogue nextDialogue;

    public Canvas exclamationMark; // Quest UI


    public void Start()
    {
        dialogueCanvas.gameObject.SetActive(false);
    }

    public void NpcDialogue() // Called by UI Button
    {
        StartCoroutine(DisplayDialogue());
     
    }

    private IEnumerator DisplayDialogue()
    {
        // Display current dialogue
        dialogueText.text = npcDialogue;

        float waitTime = 2f;

        // Play voice clip if assigned
        if (voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();
            waitTime = voiceClip.length; //Text last until the audio ends
            dialogueCanvas.gameObject.SetActive(true);
            exclamationMark.gameObject.SetActive(false);
        }

        // Wait for the displayTime before moving to next dialogue
        yield return new WaitForSeconds(waitTime);

        // If there's a next dialogue, show it
        if (nextDialogue != null)
        {
            nextDialogue.NpcDialogue(); // Trigger next dialogue
        }
        else
        {
            dialogueCanvas.gameObject.SetActive(false); // Clear text after last dialogue
            exclamationMark.gameObject.SetActive(true);
        }
    }
}
