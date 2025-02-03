using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Student : MonoBehaviour
{
    // Whether student can ans (raise hand and got qn to ans)
    public bool canAnswer;

    // To put student speech andd ans
    public TextMeshProUGUI speechBubble;

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
        speechBubble.text = generatedAnswer.ToString();

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
}
