/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file is for when user creates and update data
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Extensions;
using Unity.VisualScripting;
using System;
using TMPro;

public class MyDatabase : MonoBehaviour
{
    /// <summary>
    /// Reference to main entire database
    /// </summary>
    private DatabaseReference dbRef;

    /// <summary>
    /// Reference to user path
    /// </summary>
    private DatabaseReference userRef; 

    /// <summary>
    /// Variable to store current date in unix format
    /// </summary>
    private long unixTimestamp = new DateTimeOffset(DateTime.Today).ToUnixTimeSeconds();

    /// <summary>
    /// Variable to store current aura
    /// </summary>
    private int currentAura=0;

    /// <summary>
    /// Variable to store the current user id 
    /// </summary>
    public string currentUserId = "";

    /// <summary>
    /// Reference to UI shown on screen
    /// </summary>
    public DatabaseUiManager databaseUiManager;

    /// <summary>
    /// Reference to auth script for userId
    /// </summary>
    public AuthManager authManager;

    /// <summary>
    /// Reference to apply shirt texture script 
    /// </summary>
    public ApplyShirtTexture applyShirtTexture;

    /// <summary>
    /// Handles logic when apps start
    /// </summary>
    public void Awake()
    {
        //initialize our variable
        dbRef =FirebaseDatabase.DefaultInstance.RootReference;

        //player reference
        userRef = FirebaseDatabase.DefaultInstance.GetReference("users");
        userRef.ValueChanged += HandleValueChanged;

        //Read data
        GetAllPlayers();
    }

    /// <summary>
    /// Checks if any value is changed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.Log(args.DatabaseError.Message);
            return;
        }

        if (args.Snapshot.Exists)
        {
            Debug.Log("Data updated");
            Debug.Log("args.Snapshot");
        }
    }

    /// <summary>
    /// Gets the current user id to store inside player current stats node
    /// </summary>
    /// <returns></returns>
    public string SetCurrentUserId()
    {
        currentUserId = authManager.currentUserId; // Checking if user logged out or not
        return currentUserId;
    }

    /// <summary>
    /// Creates new player into database
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    public void CreateNewUser(string currentUserId, string username,string email, bool playerOnline,string profilePhoto)
    {
        Debug.Log("Creating user");
        Debug.Log(currentUserId);
        UserDetails userDetails = new UserDetails(username,email,playerOnline, (int)unixTimestamp,profilePhoto);
        ThoughtDetails thoughtDetails = new ThoughtDetails("");

        // Generate a unique user path
        var userPath = userRef.Child(currentUserId);
        var auraPath = userPath.Child("aura");
        var thoughtLikesPath = userPath.Child("thoughtLikes");
        var userDetailsPath = userPath.Child("userDetails");
        var thoughtDetailsPath = userPath.Child("thoughtDetails");

        // Use async methods to ensure data is set correctly
        auraPath.SetValueAsync(0);
        thoughtLikesPath.SetValueAsync(0);
        userDetailsPath.SetRawJsonValueAsync(JsonUtility.ToJson(userDetails));
        thoughtDetailsPath.SetRawJsonValueAsync(JsonUtility.ToJson(thoughtDetails));
    }

    /// <summary>
    /// Update player online
    /// </summary>
    /// <param name="currentUserId"></param>
    /// <param name="playerOnline"></param>
    public void UpdateUserOnline(string currentUserId, bool userOnline)
    {
        Dictionary<string, object> updatedDetails = new Dictionary<string, object>
        {
            ["userOnline"] = userOnline,
        };

        dbRef.Child("users").Child(currentUserId).Child("userDetails").UpdateChildrenAsync(updatedDetails);
        Debug.Log("Updated playerDetails date");
    }

    /// <summary>
    /// Stores the images the user took into database
    /// </summary>
    /// <param name="imageUrl"></param>
    public void AddImage(string imageUrl)
    {
        DatabaseReference imagesRef = dbRef.Child("users").Child(SetCurrentUserId()).Child("imagesTaken");
        imagesRef.Push().SetValueAsync(imageUrl);
        Debug.Log("Image added to database");
    }

    /// <summary>
    /// Update user aura
    /// </summary>
    /// <param name="aura"></param>
    public void UpdateAura(int aura)
    {
        currentAura += aura;
        Dictionary<string, object> updatedDetails = new Dictionary<string, object>
        {
            ["aura"] = currentAura,
        };

        dbRef.Child("users").Child(SetCurrentUserId()).UpdateChildrenAsync(updatedDetails);
        Debug.Log("Updated playerDetails date");
        //DisplayAura(currentUserId);
    }

    /// <summary>
    /// Show the user aura physically
    /// </summary>
    public void DisplayAura()
    {
        Debug.Log("Aura showing");

        dbRef.Child("users").Child(SetCurrentUserId()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data from Firebase");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                databaseUiManager.auraText.text = snapshot.Child("aura").Value.ToString();
            }
        });
    }

    public void GetProfilePicture()
    {
        Debug.Log("Getting profile");
        dbRef.Child("users").Child(SetCurrentUserId()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log(task);
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve user data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // Read data
                    string imageUrl = snapshot.Child("userDetails").Child("profilePicture").Value.ToString();
                    applyShirtTexture.ApplyTexture(imageUrl);
                    Debug.Log($"Image url: {imageUrl}");
                }
                else
                {
                    Debug.Log("User does not exist.");
                }
            }
           
        });
    }


    /// <summary>
    /// Reads player data 
    /// </summary>
    private void GetAllPlayers()
    {
        userRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Issue with task...");
            }
            else if (task.IsCompleted)
            {
                //start retriving 
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (DataSnapshot ds in snapshot.Children)
                    {
                        UserDetails p = JsonUtility.FromJson<UserDetails>(ds.GetRawJsonValue());
                        Debug.LogFormat("PLayer data:{0}, level {1}", p.email);
                    }
                }
            }
        });
    }
}
