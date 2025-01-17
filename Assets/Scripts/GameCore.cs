using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Core class to manage the game logic
/// </summary><
public class GameCore : MonoBehaviour
{
    public static GameCore gameCore;
    public Player player;
    public Enemy enemy;
    public GameObject canvasObject;
    public GameObject winLosePanelObject;
    public TextMeshProUGUI winLoseText;
    public GameObject hpDecreaseObject;
    [HideInInspector] public bool attackForward;
    [HideInInspector] public bool attackBackward;
    

    public enum TurnDefinition {
        Player, Enemy
    };

    public TurnDefinition turnDefinition;

    void Awake ()
    {
        gameCore = this;
    }
    
    /// <summary>
    /// create next turn operation
    /// </summary>
    public void NextTurn (int maxNumberAction)
    {
        if(!winLosePanelObject.activeSelf)
        {
            if(turnDefinition.Equals(TurnDefinition.Player))
            {
                enemy.SetIdle();
                enemy.RandomAction(maxNumberAction); 
                enemy.randomActionFlag = false;
                turnDefinition = TurnDefinition.Enemy;
            }
            else 
            {
                player.SetIdle();
                turnDefinition = TurnDefinition.Player;
                if(!winLosePanelObject.activeSelf)ShowCanvas(); 
            }
        }
    }

    public void SpawnDamage (int amountDamage, Vector3 position)
    {
        GameObject hpDecreaseObjectClone = Instantiate(hpDecreaseObject);
        float x = position.x + 2.5f;
        float y = position.y;
        float z = position.z;
        hpDecreaseObjectClone.transform.position = new Vector3(x, y, z);
        TextMeshPro hpDamageText = hpDecreaseObjectClone.GetComponent<TextMeshPro>();
        hpDamageText.text = amountDamage.ToString(); 
    }

    #region  INTERACTION BUTTONS
    public void AttackButton ()
    {
        player.AttackAction();

        HideCanvas();
    }

    public void DefenseButton ()
    {
        player.DefenseAction();

        HideCanvas();
    }

    public void BuffButton ()
    {
        player.BuffAction();

        HideCanvas();
    }

    public void DebuffButton ()
    {
        player.DebuffAction();

        HideCanvas();
    }

    public void RetryButton ()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region ANIMATION LEAN TWEEN
    public void ShowCanvas ()
    {
        LeanTween.scale(canvasObject, Vector3.one, 0.01f)
            .setEase(LeanTweenType.easeInOutBounce)
            .setIgnoreTimeScale(true);
    }

    public void HideCanvas ()
    {
        LeanTween.scale(canvasObject, Vector3.zero, 0.01f)
            .setEase(LeanTweenType.easeInOutBounce)
            .setIgnoreTimeScale(true);
    }
    #endregion
}
