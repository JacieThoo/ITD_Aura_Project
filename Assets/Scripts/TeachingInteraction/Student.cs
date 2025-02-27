/*
* Author: Jacie Thoo Yixuan
* Date: 31/1/2025
* Description: Handles student behaviour
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Student : MonoBehaviour
{
    // Whether student can ans (raise hand and got qn to ans)
    public bool canAnswer;

    // To put student speech and ans
    public TextMeshProUGUI speechText;

    // Speech bubble that holds speechText
    public GameObject speechBubble;

    // Student's answer (1-4 mcq)
    public int generatedAnswer;

    // Teaching script
    public Teaching teaching;

    // Evaluate script
    public EvaluateSocket evaluateSocket;

    /// <summary>
    /// Raise hand to indicate student wants to and can answer
    /// </summary>
    public void RaiseHand()
    {
        canAnswer = true;
        speechBubble.SetActive(true);
        speechText.text = "Me!";
        Debug.Log(gameObject.name + " raised their hand");
        // ADD ANIMATION OF STUDENT RAISING HAND etc
    }

    /// <summary>
    /// Raise hand to indicate player cannot answer
    /// </summary>
    public void LowerHand()
    {
        canAnswer = false;
        // ADD ANIMATION OF PLAYER PUTTING DOWN HAND etc
    }

    /// <summary>
    /// Selects student to answer the question
    /// </summary>
    public void SelectStudent()
    {
        if (canAnswer)
        {
            Debug.Log(gameObject.name + " selected");

            // Store THIS student as the last selected one
            if (teaching != null)
            {
                teaching.LastSelectedStudent(this);
            }

            speechBubble.SetActive(true);
            AnswerQuestion();

            // Check if ans correct
            if (teaching != null)
            {
                CheckAnswer(teaching.GetCorrectAnswer());
            }
        }
        else
        {
            Debug.Log("Student not raising hand or no question");
        }
    }

    /// <summary>
    /// Student generates an answer from 1-4
    /// </summary>
    void AnswerQuestion()
    {
        generatedAnswer = Random.Range(1, 5);
        speechText.text = generatedAnswer.ToString();

        Debug.Log(gameObject.name + " answered " + generatedAnswer);

        LowerHand();
    }

    /// <summary>
    /// Check if the student's ans is correct or not based on the ACTUAL correct ans 
    /// </summary>
    /// <param name="correctAnswer"></param>
    public void CheckAnswer(int correctAnswer)
    {
        if (generatedAnswer == correctAnswer)
        {
            Debug.Log(gameObject.name + " answered correctly");
            evaluateSocket.studentIsCorrect = true;
        }
        else
        {
            Debug.Log(gameObject.name + " answered wrongly");
            evaluateSocket.studentIsCorrect = false;
        }
    }

    /// <summary>
    /// Logic for changing student speech
    /// </summary>
    /// <param name="speech"></param>
    public void ChangeSpeech(string speech)
    {
        speechText.text = speech;
    }

    /// <summary>
    /// Logic for hiding student speech bubble (The whole thing including speech text)
    /// </summary>
    public void HideSpeech()
    {
        speechBubble.SetActive(false);
    }
}
