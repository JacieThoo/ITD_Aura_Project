/*
 * Author: Hoo Ying Qi Praise
 * Date: 
 * Description: Manages game state and UI interactions
 */

using TMPro;
using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TMP_Text warningText;
    [SerializeField] private GameObject WBPopUpCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowWarning()
    {
        if (warningText == null || WBPopUpCanvas == null) return;

        warningText.text = "Wrong color used! ";
        WBPopUpCanvas.SetActive(true);
        StartCoroutine(HideShowWarning(2f));
    }

    private IEnumerator HideShowWarning(float delay)
    {
        yield return new WaitForSeconds(delay);
        WBPopUpCanvas.SetActive(false);
    }
}