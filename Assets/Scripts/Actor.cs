using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///unittask is used instead using coroutines for asynchronous operation
///might good to creating task and multiple threads 
///with no garbage collection 
using Cysharp.Threading.Tasks;
using Cysharp;
using System.Diagnostics;
using System;

using TMPro;

#region CLASS COMPONENT
/// <summary>
/// class parent (super class) play as componenet dependency types
/// to define every actors in the scene which is enemy and player
/// </summary>
public class Actor : MonoBehaviour
{ 
    public Transform targetTransform;

    public Animator actorAnimator;

    public StatsManager statsManager;
       
    public StateManager stateManager;

    public Transform startPos;

    public float speed;

    protected int maxHealth;

    [Header("Health Bar Content")]
    public UnityEngine.UI.Image healthBarContent;

    [Header("Buff Gameobject")]
    public GameObject buffObj;
    public GameObject debuffObj;

    protected Actor targetTransformActor;
    protected Actor thisActor;
    protected Material targetActorMaterial;

    [Header("Actor ID")]
    public int actorId;

    [Header("Buff and Debuff Max Turn to Apply")]
    int buffCounter;
    int debuffCounter;
    public int maxBuffCounter;
    public int maxDebuffCounter;
    protected int startAttackStat;
    protected int startDefenseStat;

    [Header("Hp Text")]
    public TextMeshProUGUI hpText;

    [Header("Actor Stats Text")]
    public TextMeshProUGUI actorStatsTextAttack;
    public TextMeshProUGUI actorStatsTextDefense;
    public GameObject arrowBuff;
    public GameObject arrowDebuff;

    protected virtual void AssignStats (int health , int attack, int defense, Actor actor)
    {
        statsManager = new StatsManager(health, attack, defense, actor);
    }

    protected virtual void AssignState(Transform targetTransform, Animator animator)
    {
        stateManager = new StateManager(animator, targetTransform, this.transform, speed, startPos, buffObj, debuffObj);
    }

    public virtual void SetIdle ()
    {
        stateManager.actionState = StateManager.ActionState.Idle;
        stateManager.State();
        //check if target its on debuff
        if(isActorDebuffed())
            if(debuffCounter < maxDebuffCounter)
            {
                debuffCounter++;
            }
            else
            {
                debuffCounter = 0;
                targetTransformActor.statsManager.defensePoint = targetTransformActor.startDefenseStat;
                targetTransformActor.debuffObj.SetActive(false);
                targetTransformActor.arrowDebuff.SetActive(false);
                targetTransformActor.UpdateStatsText();
            }
        //check if we are on buff
        if(isActorBuffed())
            if(buffCounter < maxBuffCounter)
            {
                buffCounter++;
            }
            else
            {
                buffCounter = 0;
                thisActor.statsManager.attackPoint = thisActor.startAttackStat;
                thisActor.buffObj.SetActive(false);
                thisActor.arrowBuff.SetActive(false);
                thisActor.UpdateStatsText();
            }
       
    }

    protected virtual void Update ()
    {
        UpdateAttackAnimation();
    }

    /// <summary>
    /// updating attack animation (smash the enemy with melee attack)
    /// </summary>
    async void UpdateAttackAnimation ()
    {
        if(stateManager.actionState.Equals(StateManager.ActionState.Attack)){
            float distance1 = Vector3.Distance(this.transform.position, targetTransform.position);
            float distance2 = Vector3.Distance(this.transform.position, startPos.position);

            //flag motion forward
            if(GameCore.gameCore.attackForward) 
            {
                stateManager.MoveToAttack();
                if(distance1 <= 0.5f)
                {
                    stateManager.AttackAction(); //disable the forward
                    await AttackAnimation();
                }
            }

            //flag motion backward
            if(GameCore.gameCore.attackBackward) 
            {
                stateManager.MoveFromAttack();
                if(distance2 <= 0.5f)
                {
                    stateManager.AttackAfterMath();
                    stateManager.AttackEndTurn(); //end attack phase
                } 
            }
        }
    }

    /// <summary>
    /// give handle while animation attack
    /// </summary>
    /// <returns></returns>
    async UniTask AttackAnimation ()
    {
        stateManager.AttackMotion();
        await UniTask.Delay(800);
        targetActorMaterial.color = Color.red;
        ComputeDamage();
        HandleDead();
        await UniTask.Delay(1100);
        targetActorMaterial.color = Color.white;
        await UniTask.Delay(1000);
        stateManager.AttackAfterMath();
    }

    void ComputeDamage ()
    {
        if(IsTargetDefense())statsManager.TakeDamageWithDefense(statsManager.attackPoint, targetTransformActor.statsManager.defensePoint);
        else statsManager.TakeDamageOnly(statsManager.attackPoint);
        UpdateHealthBar();
        UpdateText();
    }

    void HandleDead ()
    {
        if(statsManager.IsDead())
        {
            targetTransformActor.stateManager.actionState = StateManager.ActionState.Die;
            targetTransformActor.stateManager.State();
            switch(actorId)
            {
                case 1:
                    GameCore.gameCore.winLoseText.text = "Player Win";
                    break;
                case 2:
                    GameCore.gameCore.winLoseText.text = "Enemy Win";
                    break;
            }
            GameCore.gameCore.winLosePanelObject.SetActive(true);
        }
    }

    /// <summary>
    /// value of the health bar
    /// </summary>
    /// <returns></returns>
    void UpdateHealthBar ()
    {
        float amount = (float)statsManager.healthPoint/maxHealth;

        targetTransformActor.healthBarContent.fillAmount = amount;
    }

    protected virtual void Start ()
    {
        targetTransformActor = targetTransform.GetComponent<Actor>();

        thisActor = this.GetComponent<Actor>();

        targetActorMaterial = targetTransformActor.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().materials[1];
    }

    /// <summary>
    /// check if target defense ?
    /// </summary>
    /// <returns></returns>
    bool IsTargetDefense ()
    {
        return targetTransformActor.stateManager.actionState == StateManager.ActionState.Defense;
    }

    /// <summary>
    /// actor buff checker
    /// </summary>
    /// <returns></returns>
    bool isActorBuffed ()
    {
       return thisActor.buffObj.activeSelf;
    }

    /// <summary>
    /// debuff actor checker
    /// </summary>
    /// <returns></returns>
    bool isActorDebuffed ()
    {
       return targetTransformActor.debuffObj.activeSelf;
    }

    protected virtual void UpdateText ()
    {
        targetTransformActor.hpText.text = statsManager.healthPoint.ToString() + "/" + maxHealth.ToString();
    }

    /// <summary>
    /// update stats text 
    /// </summary>
    public void UpdateStatsText ()
    {
        actorStatsTextAttack.text = statsManager.attackPoint.ToString();
        actorStatsTextDefense.text = statsManager.defensePoint.ToString();
    }
}   
#endregion


#region  SRP CLASS -> Single Responsibility Component Class
/// <summary>
/// class managing the stats manager
/// </summary>
public class StatsManager 
{
    public int healthPoint;
    public int attackPoint;
    public int defensePoint;
    public Actor actor;

    //create constructor for stat manager
    public StatsManager(int hp, int atk, int def, Actor actorX)
    {
        healthPoint = hp;
        attackPoint = atk;
        defensePoint = def;
        actor = actorX;
    }

    public void TakeDamageWithDefense(int damage, int targetDefense)
    {
        damage = damage - targetDefense;
        if(damage < 0) damage = 0;//null
        GameCore.gameCore.SpawnDamage(damage, actor.transform.position);
        healthPoint -= damage;
        if (healthPoint < 0) healthPoint = 0;
    }

    public void TakeDamageOnly (int amount)
    {
        GameCore.gameCore.SpawnDamage(amount, actor.transform.position);
        healthPoint -= amount;
        if(healthPoint < 0)healthPoint = 0;
    }

    public bool IsDead()
    {
        return healthPoint <= 0;
    }
}

/// <summary>
/// class the managing the state action manager
/// </summary>
public class StateManager {
   
    Animator animatorComponent;
    Transform targetComponent;
    Transform currPlayer;
    float currSpeed;
    Transform startPos;
    bool attackAction;
    GameObject buffObj;
    GameObject debuffObj;

    public enum ActionState{
        Idle, Attack, Defense, Buff, Debuff, Die
    };

    public ActionState actionState;

    public StateManager(Animator animator, Transform target, Transform thisInstance, float speed, Transform start, GameObject buff, GameObject debuff)
    {
        animatorComponent = animator;
        targetComponent = target;
        currPlayer = thisInstance;
        currSpeed = speed;
        startPos = start;
        buffObj = buff;
        debuffObj = debuff;
    }

    public void State ()
    {
        switch(actionState)
        {
            case ActionState.Idle:
                IdleAction();
                break;
            case ActionState.Attack:
                AttackAction();
                break;
            case ActionState.Defense:
                DefenseAction();
                break;
            case ActionState.Buff:
                BuffAction();
                break;
            case ActionState.Debuff:
                DebuffAction();
                break;
            case ActionState.Die:
                DieAction();
                break;
        }
    }

    void IdleAction ()
    {
        animatorComponent.SetBool("walk", false);
        animatorComponent.SetBool("attack", false);
        animatorComponent.SetBool("defense", false);
        animatorComponent.SetBool("die", false);
    }

    #region  ATTACK ACTION

    public void AttackAction ()
    {
        float distance = Vector3.Distance(currPlayer.position, targetComponent.position);

        //check the distance
        if(distance <= 0.8f)
        {
            GameCore.gameCore.attackForward = false;
        }
        else
        {
            GameCore.gameCore.attackForward = true;
        }
    }

    public void AttackMotion ()
    {
        if(!attackAction)animatorComponent.SetBool("attack", true);
    }

    public void MoveToAttack ()
    {
        animatorComponent.SetBool("walk", true);
        currPlayer.LookAt(targetComponent);
        currPlayer.position = Vector3.MoveTowards(currPlayer.position, targetComponent.position, currSpeed * Time.deltaTime);
    }

    public void MoveFromAttack ()
    {
        //walk back
        animatorComponent.SetBool("attack", false);
        animatorComponent.SetBool("walk", true);
        currPlayer.LookAt(startPos);
        currPlayer.position = Vector3.MoveTowards(currPlayer.position, startPos.position, currSpeed * Time.deltaTime);
    }

    public void AttackAfterMath ()
    {        
        //walk back to the original pos
        float distance = Vector3.Distance(currPlayer.position, startPos.position);
        
        if(distance <= 0.8f)
        {
            //idle
            GameCore.gameCore.attackBackward = false;
        }
        else
        {
            GameCore.gameCore.attackBackward = true;
        }
    }

    public void AttackEndTurn ()
    {
        currPlayer.LookAt(targetComponent);
        actionState = ActionState.Idle;
        State();
        GameCore.gameCore.NextTurn(4);
    }
    
    #endregion

    #region  DEFENSE ACTION
    async void DefenseAction ()
    {
        animatorComponent.SetBool("defense", true);
        await DefenseAfterMath();
    }

    async UniTask DefenseAfterMath()
    {
        await UniTask.Delay(2000);
        GameCore.gameCore.NextTurn(4);
    }
    #endregion

    #region  BUFF
    async void BuffAction ()
    {
        await BuffAfterMath();
    }

    async UniTask BuffAfterMath()
    {
        AttackBuff attackBuff = new AttackBuff(buffObj);
        attackBuff.ApplyEffect(currPlayer.gameObject, 5);
        await UniTask.Delay(1000);
        GameCore.gameCore.NextTurn(4);
    }

    #endregion

    #region  DEBUFF
    async void DebuffAction ()
    {
        await DebuffAfterMath();
    }

    async UniTask DebuffAfterMath ()
    {
        DefenseDebuff defenseDebuff = new DefenseDebuff(debuffObj);
        defenseDebuff.ApplyEffect(targetComponent.gameObject, 5);
        await UniTask.Delay(1000);
        GameCore.gameCore.NextTurn(4);
    }
    #endregion

    #region  DIE STATE
    void DieAction ()
    {
        animatorComponent.SetBool("die", true);
    }
    #endregion

}

#endregion
