/*
 * Author: Hoo Ying Qi Praise
 * Date:
 * Description: Handles the NPC voice and time
 */

using UnityEngine;
using TMPro;
using System.Collections;

public class PainterDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public AudioSource audioSource;

    public string painterDialogue;
    public float displayTime = 2f;
    public AudioClip voiceClip;
    public PainterDialogue nextDialogue;

    public void StartDialogue() // Call this from the UI button
    {
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        dialogueText.text = painterDialogue;

        if (voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();
        }

        yield return new WaitForSeconds(displayTime);

        if (nextDialogue != null)
        {
            nextDialogue.StartDialogue();
        }
        else
        {
            dialogueText.text = ""; // Clear dialogue after last line
        }
    }
}
