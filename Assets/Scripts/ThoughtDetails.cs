/*
* Author: Tan Ting Yu Gwyneth
* Date: 3/2/2025
* Description: This file defines the PlayerDetails class, which is used to store and manage player information when users interact with the UI.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtDetails 
{
    /// <summary>
    /// Variable to store the thought of the player
    /// </summary>
    public string thought;

    /// <summary>
    /// Initializes an empty ThoughtDetails object
    /// </summary>
    public ThoughtDetails()
    {
        // This constructor initializes an empty ThoughtDetails object
    }

    /// <summary>
    /// Constructor with parameters to initialize all fields
    /// </summary>
    /// <param name="username">The username of the player</param>
    /// <param name="aura">The date when the player's information was last updated</param>
    public ThoughtDetails(string thought)
    {
        this.thought = thought; // Set thought
    }
}
