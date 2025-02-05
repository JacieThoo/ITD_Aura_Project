using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chalkboard : MonoBehaviour
{
    // Show question
    public TextMeshProUGUI questionText;

    // Prefab for question ( Will appear in the optionsPanel)
    public TextMeshProUGUI optionTextPrefab;

    // Panel where options will be shown
    public Transform optionsPanel;

    /// <summary>
    /// Set question on the chalkboard
    /// </summary>
    /// <param name="question"></param>
    /// <param name="options"></param>
    public void SetQuestion(string question, List<string> options)
    {
        // Show qn
        questionText.text = question;

        // Delete previous qn
        foreach (Transform child in optionsPanel)
        {
            Destroy(child.gameObject);
        }

        // Show options
        foreach (string option in options)
        {
            TextMeshProUGUI optionText = Instantiate(optionTextPrefab, optionsPanel);
            optionText.text = option;
        }
    }
}
