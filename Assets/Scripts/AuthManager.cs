/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file is for when user to interact with the UI 
*/

using System;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Database;
using System.Threading.Tasks;
using System.IO;
using Unity.VisualScripting;

public class AuthManager : MonoBehaviour
{
    /// <summary>
    /// Reference to UI shown on screen
    /// </summary>
    public DatabaseUiManager databaseUiManager;

    /// <summary>
    /// Reference to main database
    /// </summary>
    public MyDatabase myDatabase;

    /// <summary>
    /// Reference to loading page 
    /// </summary>
    public Loading loading;

    /// <summary>
    /// Variable to store current user id 
    /// </summary>
    public string currentUserId;

    /// <summary>
    /// Reference to auth 
    /// </summary>
    private FirebaseAuth _auth;

    /// <summary>
    /// Reference to user
    /// </summary>
    private FirebaseUser _user;

    private void Start()
    {
        // Initialize Firebase Authentication
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                _auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Auth initialized successfully.");

                _auth.StateChanged += AuthOnStateChanged;

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    /// <summary>
    /// Checks it if user authenticated
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AuthOnStateChanged(object sender, EventArgs e)
    {
        Debug.Log("Auth state changed");

        if (_auth.CurrentUser == null || !_auth.CurrentUser.IsValid())
        {
            Debug.Log("User Not Authenticated");
            currentUserId = "";
            ShowSignupPanel();
        }
        else
        {
            Debug.Log("Current User is: " + _auth.CurrentUser.UserId);
            currentUserId = _auth.CurrentUser.UserId;
            myDatabase.DisplayAura();
            HideAllPanels();
        }
    }

    /// <summary>
    /// Hides login and sign up panels
    /// </summary>
    public void HideAllPanels()
    {
        databaseUiManager.logInPanel.SetActive(false);
        databaseUiManager.signUpPanel.SetActive(false);
        ResetLogInInput(); // To not store previous login text when user tries to login again
        ResetSignUpInput(); // To not store previous signup text when user tries to login again
    }

    /// <summary>
    /// Clears any text from login input
    /// </summary>
    public void ResetLogInInput()
    {
        databaseUiManager.logInEmailInputField.text = "";
        databaseUiManager.logInPasswordInputField.text = "";
        databaseUiManager.logInErrorMsgContent.text = "";
    }

    /// <summary>
    /// Clears any text from sign up input
    /// </summary>
    public void ResetSignUpInput()
    {
        databaseUiManager.signUpUsernameInputField.text = "";
        databaseUiManager.signUpEmailInputField.text = "";
        databaseUiManager.signUpPasswordInputField.text = "";
        databaseUiManager.signUpErrorMsgContent.text = "";
    }

    /// <summary>
    /// Shows sign up panel and hides the log in panel
    /// </summary>
    public void ShowSignupPanel()
    {
        databaseUiManager.logInPanel.SetActive(false);
        databaseUiManager.signUpPanel.SetActive(true);
        ResetSignUpInput();
    }

    /// <summary>
    /// Shows login panel and hides the sign up panel
    /// </summary>
    public void ShowLogInPanel()
    {
        databaseUiManager.logInPanel.SetActive(true);
        databaseUiManager.signUpPanel.SetActive(false);
        ResetLogInInput();
    }

    /// <summary>
    /// Logout the user account
    /// </summary>
    public void Logout()
    {
        myDatabase.UpdateUserOnline(currentUserId, false);
        _auth.SignOut();
        ShowLogInPanel();
    }

    /// <summary>
    /// Refer to sign up function
    /// </summary>
    public void UserSignUp()
    {
        SignUp();
    }

    /// <summary>
    /// Allows user to signup 
    /// </summary>
    /// <returns></returns>
    public async Task<FirebaseUser> SignUp()
    {
        string username = databaseUiManager.signUpUsernameInputField.text;
        string email = databaseUiManager.signUpEmailInputField.text;
        string password = databaseUiManager.signUpPasswordInputField.text;
        
        FirebaseUser newPlayer= null;

        // Sign up a new user
        await _auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    if (task.Exception != null)
                    {
                        string errorMsg = this.HandleAuthError(task);
                        databaseUiManager.signUpErrorMsgContent.text = errorMsg;
                        Debug.Log("Error in Sign Up:" + errorMsg);
                    }
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("User signed up successfully");
                    databaseUiManager.logInErrorMsgContent.text = ""; //Reset error message in case user wants to signup again
                    HideAllPanels();
                    myDatabase.CreateNewUser(currentUserId,username, email, true, 0, "https://pnghq.com/wp-content/uploads/pnghq.com-default-pfp-png-with-vibr-4.png"); // Creates user data in firebase
                    myDatabase.DisplayAura();
                    loading.ShowLoadingScreen();
                }
            });
        return newPlayer;
    }

    /// <summary>
    /// Allows user to login
    /// </summary>
    public void Login()
    {
         string email = databaseUiManager.logInEmailInputField.text;
         string password = databaseUiManager.logInPasswordInputField.text;

        // Log in an existing user
        _auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    if (task.Exception != null)
                    {
                        
                        string errorMsg = this.HandleAuthError(task);
                        databaseUiManager.logInErrorMsgContent.text = errorMsg;
                        Debug.Log("Error in Log in:" + errorMsg);
                    }
                }

                else if (task.IsCompleted)
                {
                    databaseUiManager.logInErrorMsgContent.text = ""; //Reset error message in case user wants to login again
                    Debug.Log("User logged in successfully");
                    myDatabase.DisplayAura();
                    loading.ShowLoadingScreen();
                    HideAllPanels();
                }
            });
    }

    /// <summary>
    /// Decide and determine the error when user tries to sign up or log in
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public string HandleAuthError(Task<AuthResult> task)
    {
        string errorMsg = "";
        if (task.Exception!=null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) firebaseEx.ErrorCode;
            errorMsg = "";
            switch (errorCode) 
            {
                case AuthError.MissingEmail: 
                    errorMsg += "Email is missing";
                    break;
                case AuthError.EmailAlreadyInUse: // For sign in only
                    errorMsg += "Email is already in use";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "Password is missing";
                    break;
                case AuthError.WrongPassword: // For log in only
                    errorMsg += "Wrong password used";
                    break;
                case AuthError.WeakPassword: // For sign in only
                    errorMsg += "Password needs at least 6 characters";
                    break;
                case AuthError.InvalidEmail: 
                    errorMsg += "Invalid email used";
                    break;
                case AuthError.UserNotFound:  // For login error
                    errorMsg += "Account does not exist";
                    break;
                default:
                    errorMsg += "Issues in authentication";
                    break;
            }
        }
        return errorMsg;
    }
}