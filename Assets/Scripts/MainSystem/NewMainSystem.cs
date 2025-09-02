using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMainSystem : MonoBehaviour
{
    [SerializeField]
    PlayerStatus playersStatus;
    [SerializeField]
    EnemyStatus enemyStatus;

    [SerializeField]
    GameObject[] scenes;
    [SerializeField]
    GameObject[] BattleCommandButtons;
    [SerializeField]
    Text[] CommunicationTexts = new Text[2];
    [SerializeField]
    string[] uniqueTexts;
    [SerializeField]
    Text[] statusTexts;
    [SerializeField]
    string[] commandName;           // 戦闘コマンド用の文字を保存

    BattleState_Enum battleState;
    BattleSystem battleSystem = new BattleSystem();
    List<CommunicationText_Struct> setText = new List<CommunicationText_Struct>();
    List<BetaStatus> battleMoveCharaList = new List<BetaStatus>();

    int setTextNumber = 0;
    int battleMoveCharaNumber=0;
    bool firstProcessFlg = false;
    bool endProcessFlg = false;

    const int COMMANDBUTTONMAX = 5;
    const string SYSTEMNAME = "システム";

    #region コマンドボタン
    public void SelectPlayerCommand(BattleCommandNames getBattleCommandNames)
    {
        playersStatus.battleCommand = getBattleCommandNames;
        battleState = BattleState_Enum.SpeedCheck;
        scenes[(int)SceneObjectNames.BattleSelectButtons].SetActive(false);
        firstProcessFlg = false;
    }
    #endregion

    #region テキストの更新

    void StatusDataUpdata()
    {
        statusTexts[0].text = $"{playersStatus.name}　Lv{playersStatus.lv}\nHP{playersStatus.currentHp}/{playersStatus.maxhp}";
        statusTexts[1].text = $"{enemyStatus.name}　Lv{enemyStatus.lv}\nHP{enemyStatus.currentHp}/{enemyStatus.maxhp}";
    }

    void CommunicationTextUpdate()
    {
        CommunicationTexts[0].text = setText[setTextNumber].commnicationCharaName; 
        CommunicationTexts[1].text = setText[setTextNumber].commnicationWord;
        if (setText[setTextNumber].commniEvent)
        {
            StatusDataUpdata();
        }
    }
    void CommunicationTextUpdate(string word, string name = SYSTEMNAME, bool eventFlg = false)
    {
        CommunicationTexts[0].text = name;
        CommunicationTexts[1].text = word; ;
        if (eventFlg)
        {
            StatusDataUpdata();
        }
    }
    #endregion

    #region バトル処理

    // StartPhaseの処理
    void StartPhaseProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            battleState = BattleState_Enum.StandPhase;
        }
    }

    // StandPhaseの処理
    void SelectCommandProcess()
    {
        if (!firstProcessFlg)
        {
            firstProcessFlg = true;
            CommunicationTextUpdate(uniqueTexts[1]);
            scenes[(int)SceneObjectNames.BattleSelectButtons].SetActive(true);
            playersStatus.moveSuccess = false;
            enemyStatus.moveSuccess = false;
        }
        if (endProcessFlg)
        {
            battleState = BattleState_Enum.SpeedCheck;
            firstProcessFlg = false;
            endProcessFlg = false;
        }
    }

    // SpeedCheckの処理
    void SpeedCheclProcess()
    {
        battleMoveCharaList.Clear();
        if(playersStatus.currentHp >= 0)
        {
            battleMoveCharaList.Add(playersStatus);
        }
        if (enemyStatus.currentHp >= 0)
        {
            // 一旦ね？とりあえずランダムで・・・いいよね？
            enemyStatus.battleCommand = (BattleCommandNames)UnityEngine.Random.Range(0, Enum.GetValues(typeof(BattleCommandNames)).Length);
            battleMoveCharaList.Add(enemyStatus);
        }
        battleMoveCharaList.Sort((a, b) => a.NormalSpeed().CompareTo(b.NormalSpeed()));
        battleState = BattleState_Enum.BattlePhase;
    }

    // BattlePhaseの処理
    void BattlePhaseProcess()
    {
        //if (battleMoveCharaList.Count != battleMoveCharaNumber && !firstProcessFlg)
        {
            // 選択されたキャラのHPが0なら値を加算して処理を即終わらせる
            if (battleMoveCharaList[battleMoveCharaNumber].currentHp <= 0)
            {
                battleMoveCharaNumber++;
                firstProcessFlg = true;
                return;
            }

            // 選択されたキャラを取得
            var moveCharaData = battleMoveCharaList[battleMoveCharaNumber];
            // テキスト設定変数準備
            CommunicationText_Struct communicationText = new CommunicationText_Struct();
            communicationText.commnicationCharaName = SYSTEMNAME;
            communicationText.commniEvent = false;
            // 選択されたキャラの選んだ行動に合わせて処理を行う
            switch (moveCharaData.battleCommand)
            {
                case BattleCommandNames.NormalAttack:
                    var attackData = moveCharaData.NormalAttack();
                    int getId = battleSystem.HitTargetIDs(attackData, battleMoveCharaList.ToArray(), battleMoveCharaNumber);
                    int damage = battleMoveCharaList[getId].DamageProcess(attackData);
                    communicationText.commnicationWord = $"{moveCharaData.name}の攻撃";
                    communicationText.commniEvent = false;
                    setText.Add(communicationText);
                    communicationText.commnicationWord = $"{battleMoveCharaList[getId].name}は{damage}ダメージ受けた";
                    communicationText.commniEvent = true;
                    setText.Add(communicationText);
                    if (battleMoveCharaList[getId].currentHp <= 0)
                    {
                        communicationText.commnicationWord = $"{battleMoveCharaList[getId].name}は倒れた";
                        communicationText.commniEvent = false;
                        setText.Add(communicationText);
                        if (moveCharaData.ImOnYourSide)
                        {
                            moveCharaData.exp = battleMoveCharaList[getId].exp;
                            communicationText.commnicationWord = $"{moveCharaData.name}は{battleMoveCharaList[getId].exp}経験値獲得した";
                            communicationText.commniEvent = false;
                            // レベルアップの処理欲しい
                            setText.Add(communicationText);
                        }
                    }
                    break;
                case BattleCommandNames.NormalDefense:
                    moveCharaData.moveSuccess = true;
                    communicationText.commnicationWord = $"{moveCharaData.name}は守りの姿勢を取った";
                    setText.Add(communicationText);
                    break;
                case BattleCommandNames.EndBattle_Escape:
                    communicationText.commnicationWord = $"{moveCharaData.name}は逃げようとした";
                    setText.Add(communicationText);
                    bool escape_result = battleSystem.EscapeChallenge(battleMoveCharaList.ToArray(), battleMoveCharaNumber);
                    moveCharaData.moveSuccess = escape_result;
                    communicationText.commnicationWord = escape_result ? $"{moveCharaData.name}は逃げる事ができた" : $"{moveCharaData.name}は逃げる事ができなかった";
                    setText.Add(communicationText);
                    break;
            }
            // 全てが終わったら値を加算する
            // battleMoveCharaNumber++;
            //}
            //else
            //{
            // firstProcessFlg = true;
            // battleMoveCharaNumber = 0;
            CommunicationTextUpdate();
            setTextNumber++;
            battleState = BattleState_Enum.ResultPhase;
        }
    }

    // ResultPhaseの処理
    void ResultPhaseProcess()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (setTextNumber == setText.Count )
            {
                firstProcessFlg = false;
                setTextNumber = 0;
                if (playersStatus.currentHp <= 0 || enemyStatus.currentHp <= 0)
                {
                    battleState = playersStatus.currentHp <= 0 || enemyStatus.currentHp <= 0 ? BattleState_Enum.EndPhase : BattleState_Enum.StandPhase;
                }
                else if (battleMoveCharaList.Count != battleMoveCharaNumber)
                {
                    battleState = BattleState_Enum.BattlePhase;
                    battleMoveCharaNumber++;
                    setText.Clear();
                }
                
            }
            else
            {
                CommunicationTextUpdate();
                setTextNumber++;
            }
        }
    }

    // EndPhaseの処理
    void EndPhaseProcess()
    {
        CommunicationText_Struct communicationText = new CommunicationText_Struct();
        communicationText.commnicationCharaName = SYSTEMNAME;
        if (enemyStatus.currentHp <= 0)
        {
            CommunicationTextUpdate($"{playersStatus.name}は戦闘に勝利した");
        }
        else
        {
            CommunicationTextUpdate($"{playersStatus.name}は戦闘に敗北した");
        }
        //UnityEditor.EditorApplication.isPaused = true;
    }
    #endregion

    private void Start()
    {
        // それぞれのステータス取得(今はしないけど(だってめんどいし))

        playersStatus.ImOnYourSide = true;
        enemyStatus.ImOnYourSide = false;

        // テキストの更新
        battleState = BattleState_Enum.StartPhase;
        CommunicationTextUpdate(uniqueTexts[0]);
        StatusDataUpdata();

        // バトルコマンドの設定
        var commandNum = Enum.GetValues(typeof(BattleCommandNames)).Length;
        if (commandNum < BattleCommandButtons.Length)
        {
            for (int i = 0; i < BattleCommandButtons.Length; i++)
            {
                if (commandNum - 1 < i)
                {
                    BattleCommandButtons[i].SetActive(false);
                }
                else
                {
                    BattleCommandButtons[i].transform.GetChild(0).GetComponent<Text>().text = commandName[i];
                    var buttonController = BattleCommandButtons[i].GetComponent<BattleCommandButtonController>();
                    buttonController.myCommand = (BattleCommandNames)i;
                    var button = buttonController.GetComponent<Button>();
                    button.onClick.AddListener(buttonController.ButtonEvent);
                }
            }
        }
    }
    private void Update()
    {
        switch (battleState)
        {
            case BattleState_Enum.StartPhase:
                StartPhaseProcess();
                break;
            case BattleState_Enum.StandPhase:
                SelectCommandProcess();
                break;
            case BattleState_Enum.SpeedCheck:
                SpeedCheclProcess();
                break;
            case BattleState_Enum.BattlePhase:
                BattlePhaseProcess();
                break;
            case BattleState_Enum.ResultPhase:
                ResultPhaseProcess();
                break;
            case BattleState_Enum.EndPhase:
                EndPhaseProcess();
                break;
            default:
                break;
        }
    }
}