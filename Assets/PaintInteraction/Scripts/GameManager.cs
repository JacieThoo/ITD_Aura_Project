/*
 * Author: Hoo Ying Qi Praise
 * Date: 
 * Description: 
 */

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int score;

    [SerializeField] private Text whiteBoardText;
    [SerializeField] private GameObject WBPopUpCanvas;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Add Score
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score is" + score);
    }

    // Whiteboard UI Warning Texts
    public void ShowWarning(string message)
    {
        whiteBoardText.text = message;
        WBPopUpCanvas.SetActive(true);
    }

    public void HideWarning()
    {
        WBPopUpCanvas.SetActive(false);
    }
}
