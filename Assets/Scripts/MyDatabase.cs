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
    public void CreateNewUser(string currentUserId, string username,string email, bool playerOnline, int aura,string profilePhoto)
    {
        Debug.Log("Creating user");
        Debug.Log(currentUserId);
        UserDetails userDetails = new UserDetails(username,email,playerOnline, (int)unixTimestamp,aura,profilePhoto);
        ThoughtDetails thoughtDetails = new ThoughtDetails("",0);
        ImagesTaken imageTaken = new ImagesTaken();

        // Generate a unique user path
        var userPath = userRef.Child(currentUserId);
        var userDetailsPath = userPath.Child("userDetails");
        var thoughtDetailsPath = userPath.Child("thoughtDetails");
        var imagesTakenPath = userPath.Child("imagesTaken");

        // Use async methods to ensure data is set correctly
        userDetailsPath.SetRawJsonValueAsync(JsonUtility.ToJson(userDetailsPath));
        thoughtDetailsPath.SetRawJsonValueAsync(JsonUtility.ToJson(thoughtDetailsPath));
        imagesTakenPath.SetRawJsonValueAsync(JsonUtility.ToJson(imagesTakenPath));
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

        dbRef.Child("users").Child(SetCurrentUserId()).Child("userDetails").UpdateChildrenAsync(updatedDetails);
        Debug.Log("Updated playerDetails date");
        //DisplayAura(currentUserId);
    }

    /// <summary>
    /// Show the user aura physically
    /// </summary>
    public void DisplayAura()
    {
        Debug.Log("Aura showing");

        dbRef.Child("users").Child(SetCurrentUserId()).Child("userDetails").GetValueAsync().ContinueWithOnMainThread(task =>
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
