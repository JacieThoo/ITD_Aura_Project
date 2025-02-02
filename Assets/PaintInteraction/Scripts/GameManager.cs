/*
 * Author: Hoo Ying Qi Praise
 * Date: 
 * Description: Manages game state and UI interactions
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// instance of the GameManager
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// Score variable
    /// </summary>
    private int score;

    /// <summary>
    /// Reference to the score text UI element
    /// </summary>
    private TMP_Text scoreText;

    /// <summary>
    /// Text to display on the whiteboard UI
    /// </summary>
    [SerializeField] private Text whiteBoardText;

    /// <summary>
    /// Pop-up canvas for the whiteboard UI
    /// </summary>
    [SerializeField] private GameObject WBPopUpCanvas;

    // Awake is called when the script is initialized
    public void Awake()
    {
        // Ensure that only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    /// <summary>
    /// Add Score and update UI 
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        score += amount; // Increment score by amount
        Debug.Log("Score is " + score); // Updated score

        scoreText.text = "Score: " + score; // Update score display
    }

    /// <summary>
    /// Show whiteboard UI warning messages
    /// </summary>
    /// <param name="message"></param>
    public void ShowWarning(string message)
    {
        whiteBoardText.text = message; // Set warning message
        WBPopUpCanvas.SetActive(true); // Display the warning pop-up
    }
}