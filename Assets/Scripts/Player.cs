using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

#region LSP IMPLEMENTATION WITH INHERITANCE FOR PLAYER
//LSP (Liskov Subtitution Principal)
//try make child object change logic without change its parent class
//every actor must know what they need to do without look if its enemy or player
public class Player : Actor
{
    /// <summary>
    /// abstraction 
    /// </summary>
    [System.Serializable]
    public struct Stats {
        public int health;
        public int attack;
        public int defense;
    }

    public Stats stats;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AssignStats(stats.health, stats.attack, stats.defense, targetTransformActor); //LSP Principal assign it with different stats

        AssignState(targetTransform, actorAnimator); //LSP Principal assign it with different components

        maxHealth = stats.health;

        startAttackStat = stats.attack;

        startDefenseStat = stats.defense;

        UpdateText();

        UpdateStatsText();
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// only to be call in button
    /// update attack action
    /// </summary>
    public void AttackAction ()
    {
        stateManager.actionState = StateManager.ActionState.Attack;

        stateManager.State();
    }

    /// <summary>
    /// only to be call in button
    /// update defense action
    /// </summary>
    public void DefenseAction ()
    {
        stateManager.actionState = StateManager.ActionState.Defense;

        stateManager.State();
    }

    public void BuffAction ()
    {
        stateManager.actionState = StateManager.ActionState.Buff;

        stateManager.State();
    }

    public void DebuffAction ()
    {
        stateManager.actionState = StateManager.ActionState.Debuff;

        stateManager.State();
    }
}

#endregion
