using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Options
{
    public List<string> optionList;  // List in list for options
}

public class Teaching : MonoBehaviour
{
    // Store chalkboard
    public Chalkboard chalkboard;

    // List of students (in the classroom)
    public List<Student> students;

    // List of questions to be shown
    public List<string> questions;

    // List of options
    public List<Options> options = new List<Options>();

    // List of correct answers
    public List<int> correctAnswers;

    // What question it is now
    private int currentQuestionIndex = 0;

    // Correct answer for current question shown
    private int currentCorrectAnswer;

    // Last student selected by player (needed to check ans)
    private Student lastSelectedStudent;

    // Whether at least 1 hand is raised
    private bool atLeastOneRaisedHand;

    // ************** Make sure element num for each qn/option/ans corresponds

    // Start is called before the first frame update
    void Start()
    {
        ShowNextQuestion();
    }

    /// <summary>
    /// Function to show question on chalkboard and make students raise hand
    /// </summary>
    public void ShowNextQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            // Get options for current question
            List<string> currentOptions = options[currentQuestionIndex].optionList;

            // Store correct answer for current qn
            currentCorrectAnswer = correctAnswers[currentQuestionIndex];

            // Show question and options on chalkboard
            chalkboard.SetQuestion(questions[currentQuestionIndex], currentOptions);

            atLeastOneRaisedHand = false;

            // 50% chance of raising hand for each student
            foreach (var student in students)
            {
                if (Random.Range(0, 2) == 1)
                {
                    student.RaiseHand();
                    atLeastOneRaisedHand = true;
                }
            }

            // If no one raises hand, force one to raise
            if (!atLeastOneRaisedHand && students.Count > 0)
            {
                int randomNum = Random.Range(0, students.Count);
                students[randomNum].RaiseHand();
            }

            // Next qn
            currentQuestionIndex++;
        }
        else
        {
            Debug.Log("No more questions");
        }
    }

    /// <summary>
    /// Change speech of the last selected
    /// </summary>
    /// <param name="speech"></param>
    public void ChangeStudentSpeech(string speech)
    {
        lastSelectedStudent.ChangeSpeech(speech);
    }

    /// <summary>
    /// Hide speech of the last selected
    /// </summary>
    public void HideStudentSpeech()
    {
        lastSelectedStudent.HideSpeech();
    }

    /// <summary>
    /// Function to set last selected student 
    /// </summary>
    /// <param name="student"></param>
    public void LastSelectedStudent(Student student)
    {
        lastSelectedStudent = student;

        // Lower hands of all other students so only 1 selected student can ans per qn
        foreach (var s in students)
        {
            if (s != student)
            {
                s.LowerHand();
            }
        }
    }

    // TO get last student selected by player (use in EvaluateSocket)
    public Student GetLastSelectedStudent()
    {
        return lastSelectedStudent;
    }

    // To get the correct answer for the current question
    public int GetCorrectAnswer()
    {
        return currentCorrectAnswer;
    }
}
