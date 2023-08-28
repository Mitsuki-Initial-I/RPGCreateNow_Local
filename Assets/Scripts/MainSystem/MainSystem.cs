using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    string[] savefolderNames;
    [SerializeField]
    string[] saveFileNames;
    [SerializeField]
    string[] resourceFileNames;

    GameSceneNames gamestate = GameSceneNames.LoadData_First;

    void LoadSaveDatas()
    {

    }

    void GameStateProcess(GameSceneNames getState)
    {
        switch (getState)
        {
            case GameSceneNames.LoadData_First:
                break;
            case GameSceneNames.Title:
                break;
            case GameSceneNames.PlayerSetting:
                break;
            case GameSceneNames.Home:
                break;
            case GameSceneNames.LoadData_Battle:
                break;
            case GameSceneNames.Battle:
                break;
            default:
                break;
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}