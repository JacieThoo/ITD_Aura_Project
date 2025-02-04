using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class CorrectionSocket : MonoBehaviour
{
    // Socket for the card
    public XRSocketInteractor correctionSocket;

    // Teaching script
    public Teaching teaching;

    // Evaluate socket script
    public EvaluateSocket evaluateSocket;

    // Database to update aura
    public MyDatabase myDatabase;

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Check whether the correction the player gave is correct
    /// </summary>
    public void CheckCorrection()
    {
        // If correction card is attached
        if (correctionSocket.interactablesSelected.Count > 0 && evaluateSocket.playerHasEvaluated == true)
        {
            XRBaseInteractable interactable = correctionSocket.interactablesSelected[0] as XRBaseInteractable;
            if (interactable != null)
            {
                // Check tag (1, 2, 3 or 4)
                GameObject correctionCard = interactable.gameObject;
                string correctionCardTag = correctionCard.tag;

                // Get the ACTUAL ans to the current qn
                int correctAnswer = teaching.GetCorrectAnswer();

                // Match tag to actual ans 
                // If correction is correct
                if (correctionCardTag == correctAnswer.ToString())
                {
                    Debug.Log("Player correctly corrected");
                    // Plus aura
                    myDatabase.UpdateAura(10);

                    teaching.ShowNextQuestion();
                    teaching.HideStudentSpeech();
                }
                else
                {
                    // Minus aura
                    myDatabase.UpdateAura(-5);

                    Debug.Log("Player wrongly corrected");
                    teaching.ChangeStudentSpeech("I don't think that's the answer...");
                }
            }
        }
        else
        {
            Debug.Log("No card placed.");
        }
    }
}
