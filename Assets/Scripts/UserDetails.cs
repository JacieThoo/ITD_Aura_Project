/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file defines the PlayerDetails class, which is used to store and manage player information when users interact with the UI.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDetails
{
    /// <summary>
    /// Variable to store the username of the player
    /// </summary>
    public string username;

    /// <summary>
    /// Variable to store the email of the player
    /// </summary>
    public string email;

    /// <summary>
    /// Variable to indicate if the user is currently online
    /// </summary>
    public bool userOnline;

    /// <summary>
    /// Variable to store the date when the player's account was created
    /// </summary>
    public int dateCreated;

    /// <summary>
    /// Variable to store the aura of the player
    /// </summary>
    public int aura;

    /// <summary>
    /// Variable to store the profilePhoto of the player
    /// </summary>
    public string profilePhoto;


    /// <summary>
    /// Initializes an empty PlayerDetails object
    /// </summary>
    public UserDetails()
    {
        // This constructor initializes an empty PlayerDetails object
    }

    /// <summary>
    /// Constructor with parameters to initialize all fields
    /// </summary>
    /// <param name="username">The username of the player</param>
    /// <param name="email">The email of the player</param>
    /// <param name="userOnline">Indicates if the user is currently online</param>
    /// <param name="dateCreated">The date when the player's account was created</param>
    /// <param name="aura">The date when the player's information was last updated</param>
    /// <param name="aura">The date when the player's information was last updated</param>
    public UserDetails(string username, string email, bool userOnline, int dateCreated, int aura, string profilePhoto)
    {
        this.username = username; // Set username
        this.email = email; // Set email
        this.userOnline = userOnline; // Set online status
        this.dateCreated = dateCreated; // Set account creation date
        this.aura = aura; // Set aura
        this.profilePhoto = profilePhoto; // Set profilePhoto
    }
}
