using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCommandButtonController : MonoBehaviour
{
    public BattleCommandNames myCommand;
    public void ButtonEvent()
    {
        GameObject.Find("MainSystem").GetComponent<NewMainSystem>().SelectPlayerCommand(myCommand);
        
        //switch (myCommand)
        //{
        //    case BattleCommandNames.NormalAttack:
        //        GameObject.Find("MainSystem").GetComponent<NewMainSystem>().PlayerSelectCommand(myCommand);
        //        break;
        //    case BattleCommandNames.NormalDefense:
        //        GameObject.Find("MainSystem").GetComponent<NewMainSystem>().PlayerSelectCommand(myCommand);
        //        break;
        //    case BattleCommandNames.EndBattle_Escape:
        //        GameObject.Find("MainSystem").GetComponent<NewMainSystem>().PlayerSelectCommand(myCommand);
        //        break;
        //    default:
        //        break;
        //}
    }
}