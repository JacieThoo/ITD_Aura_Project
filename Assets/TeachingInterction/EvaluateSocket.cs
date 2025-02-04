using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class EvaluateSocket : MonoBehaviour
{
    // Socket for the card
    public XRSocketInteractor evaluateSocket;

    // Teaching script
    public Teaching teaching;

    // Whether student answered the question correctly (system evaluates)
    public bool studentIsCorrect;

    // PLAYER evaluation
    public bool playerSaidCorrect;

    // Whether player has evaluated the student's ans 
    public bool playerHasEvaluated;

    /// <summary>
    /// Checks tag of the card (wrong/correct)
    /// Match to current student's answer and actual answer
    /// </summary>
    public void CheckCardTag()
    {
        Student lastStudent = teaching.GetLastSelectedStudent();

        if (lastStudent == null)
        {
            Debug.Log("No student answered");
            return;
        }

        // If card is attached to socket
        // Edit interaction layer thing in inspector for only tick or cross cards
        if (evaluateSocket.interactablesSelected.Count > 0)
        {

            XRBaseInteractable interactable = evaluateSocket.interactablesSelected[0] as XRBaseInteractable;
            if (interactable != null)
            {
                // Get the tag of the card
                GameObject card = interactable.gameObject;
                string cardTag = card.tag;
                Debug.Log("Evaluation: " + cardTag);

                // Player evaluation 
                if (cardTag == "Correct")
                {
                    playerHasEvaluated = true;
                    playerSaidCorrect = true;
                }
                else
                {
                    playerHasEvaluated = true;
                    playerSaidCorrect = false;
                }

                // Match player evaluation to student's answer
                if (studentIsCorrect == playerSaidCorrect)
                {
                    Debug.Log("Player evaluated correctly");
                    // PLUS AURA

                    if (studentIsCorrect == false) // If student is wrong
                    {
                        Debug.Log("Correction needed");
                        teaching.ChangeStudentSpeech("Oh, then what is the correct answer?");
                    }
                    else // If student is correct
                    {
                        Debug.Log("Correction not needed");
                        teaching.ShowNextQuestion();
                        teaching.HideStudentSpeech();
                    }
                }
                else // If PLAYER is wrong
                {
                    // MINUS AURA
                    Debug.Log("Player evaluated wrongly");
                    teaching.ChangeStudentSpeech("What? I was right though.");
                }
            }
        }
        else
        {
            Debug.Log("No card placed.");
        }
    }
}
