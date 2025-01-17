using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region  ISP IMPLEMENTATION
/// <summary>
/// buff to implementation ISP (Interface segregation principle)
/// sepearate interface between is function
/// </summary>
public class AttackBuff : IBuff
{
    public GameObject attackBuffEffect;

    public AttackBuff (GameObject buffObj)
    {
        attackBuffEffect = buffObj;
    }

    public void ApplyEffect(GameObject target, int effectAmount)
    {
        attackBuffEffect?.SetActive(true);

        Actor actor = target.GetComponent<Actor>();

        actor.statsManager.attackPoint += effectAmount;

        actor.UpdateStatsText();

        actor.arrowBuff.SetActive(true);
    }
}

/// <summary>
/// create debuff that can debuff defense stats
/// </summary>
public class DefenseDebuff : IDebuff
{
    public GameObject defenseDebuffEffect;

    public DefenseDebuff(GameObject debuffObj)
    {
        defenseDebuffEffect = debuffObj;
    }

    public void ApplyEffect(GameObject target, int effectAmount)
    {
        Actor actor = target.GetComponent<Actor>();

        actor.debuffObj.SetActive(true);

        actor.statsManager.defensePoint -= effectAmount;

        actor.UpdateStatsText();

        actor.arrowDebuff.SetActive(true);

        if(actor.statsManager.defensePoint < 0)actor.statsManager.defensePoint = 0;
    }
}

#region  Interface Segregation Principle

public interface IBuff
{
    public void ApplyEffect (GameObject target, int effectAmount);

    //could be another method needs in case of buff
}

public interface IDebuff
{
    public void ApplyEffect (GameObject target, int effectAmount);

    //could be another method needs in case of debuff
}

#endregion

#endregion


