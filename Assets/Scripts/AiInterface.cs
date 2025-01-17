using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region IMPLEMENTATION OCP PRINCIPLE CREATE INTERFACE FOR AI 
/// <summary>
/// interface to implement OCP (Open Closed Principle)
/// create interface for any kind of AI that randomize the action
/// in this case make interface only use if that actor was an AI
/// </summary>
public interface AiInterface
{
    /// <summary>
    /// randomize what action will be the next for this enemy
    /// </summary>
    public void RandomAction (int maxNumber);
}
#endregion