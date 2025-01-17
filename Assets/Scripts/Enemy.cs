using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region LSP IMPLEMENTATION WITH INHERITANCE FOR ENEMY
//LSP (Liskov Subtitution Principal) and OCP (Open Closed Principle)
//try make child object change logic without change its parent class
//every actor must know what they need to do without look if its enemy or player
//this enemy need AI to randomize its action , create interface from Ai Interface
public class Enemy : Actor, AiInterface
{
    [System.Serializable]
    public struct Stats {
        public int health;
        public int attack;
        public int defense;
    }

    [HideInInspector] public bool randomActionFlag;

    public Stats stats;

    protected override void Start ()
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
    /// create method from interface OCP (Open Close Principle)
    /// </summary>
    /// <param name="maxNumber"></param>
    public void RandomAction(int maxNumber)
    {
        int random = UnityEngine.Random.Range(0,maxNumber);

        switch(random)
        {
            case 0:
                AttackAction();
                break;
            case 1:
                DefenseAction();
                break;
            case 2:
                if(!buffObj.activeSelf) BuffAction();
                else RandomAction(4);
                break;
            case 3:
                DebuffAction();
                break;
        }
    }

    public void AttackAction ()
    {
        stateManager.actionState = StateManager.ActionState.Attack;

        stateManager.State();
    }

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