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
}