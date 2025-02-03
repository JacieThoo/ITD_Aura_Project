/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file stores all the UI for the database
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DatabaseUiManager : MonoBehaviour
{
    [Header("Login UI References")]
    /// <summary>
    /// Variable to store login panel
    /// </summary>
    public GameObject logInPanel;

    /// <summary>
    /// Variable to store login email 
    /// </summary>
    public TMP_InputField logInEmailInputField;

    /// <summary>
    /// Variable to store login password
    /// </summary>
    public TMP_InputField logInPasswordInputField;

    /// <summary>
    /// Variable to store error message in login panel
    /// </summary>
    public TextMeshProUGUI logInErrorMsgContent;

    [Header("Signup UI References")]
    /// <summary>
    /// Variable to store signup panel
    /// </summary>
    public GameObject signUpPanel;

    /// <summary>
    /// Variable to store signup username
    /// </summary>
    public TMP_InputField signUpUsernameInputField;

    /// <summary>
    /// Variable to store signup email
    /// </summary>
    public TMP_InputField signUpEmailInputField;

    /// <summary>
    /// Variable to store signup password
    /// </summary>
    public TMP_InputField signUpPasswordInputField;

    /// <summary>
    /// Variable to store error message in signup panel
    /// </summary>
    public TextMeshProUGUI signUpErrorMsgContent;

    [Header("Aura data shown in profile")]
    /// <summary>
    /// Aura shown in profile
    /// </summary>
    public TextMeshProUGUI auraText;
}
