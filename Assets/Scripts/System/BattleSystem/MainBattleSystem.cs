using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPGCreateNow_Local.UseCase;
using RPGCreateNow_Local.Data;

namespace RPGCreateNow_Local.System
{
    public class MainBattleSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject textBox;                     // 行動表示するオブジェクト
        [SerializeField]
        GameObject selectButtontBox;            // 選択ボタン格納オブジェクト
        [SerializeField]
        EventSystem eventSystem;                // クリックしたオブジェクト取得用

        private IStockData stockData;                               // 一時保管されたデータの取得用
        private PlayerStatus_Structure playerBattleStatusData;      // プレイヤのバトルステータスデータ
        private EnemyStatus_Structure enemyBattleStatusData;        // 敵のバトルステータスデータ
       
        private BattlePhase phase = BattlePhase.Start;
        private BattleMainSelectAction battleAction;                // 選択したコマンド
        private string setStr;                                      // 表示するテキスト
        string[] selectButtonWord;
        private bool AttackerFlg;                                   // 先制攻撃できるか
        private bool inputCheck;
        private int playerMaxHp;                                    // プレイヤの最大HP
        private int clickCount;
        void Start()
        {
            stockData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            playerBattleStatusData = stockData.GetPlayerStatusData();
            enemyBattleStatusData = stockData.GetEnemyStatusData();
            MainBattleProcess();
            playerMaxHp = playerBattleStatusData.hp;
        }
        private void Update()
        {
            inputCheck = Input.GetMouseButtonDown(0);
            MainBattleProcess();
        }

        // バトル処理
        public void EnemyBattleProcess(int clickCount)
        {
            // 面倒だから攻撃しかしない
            switch (clickCount)
            {
                case 0:
                    //if ()
                    //{
                    //  setText = $"{enemyStatusData.enemyName}の攻撃";
                    //}
                    //else
                    //{
                    setStr = "敵の攻撃";
                    //}
                    break;
                case 1:
                    int damage = enemyBattleStatusData.ap - playerBattleStatusData.dp;
                    damage = damage <= 0 ? 1 : damage;
                    playerBattleStatusData.hp -= damage;
                    setStr = $"{playerBattleStatusData.playerName}は{damage}ダメージ受けた";
                    break;
            }
        }
        private void PlayerBattleProcess()
        {
            switch (clickCount)
            {
                case 0:
                    switch (battleAction)
                    {
                        case BattleMainSelectAction.Attack:
                            setStr = $"{playerBattleStatusData.playerName}は攻撃した";
                            break;
                        case BattleMainSelectAction.Guard:
                            setStr = $"{playerBattleStatusData.playerName}は防御した";
                            break;
                        case BattleMainSelectAction.Skill:
                            setStr = $"{playerBattleStatusData.playerName}はスキルを発動した";
                            break;
                        case BattleMainSelectAction.UseItem:
                            setStr = $"{playerBattleStatusData.playerName}は薬草を使用した";
                            break;
                        case BattleMainSelectAction.Escape:
                            setStr = $"{playerBattleStatusData.playerName}は逃走した";
                            clickCount = 1;
                            break;
                    }
                    break;
                case 1:
                    switch (battleAction)
                    {
                        case BattleMainSelectAction.Attack:
                            int damage = playerBattleStatusData.ap - enemyBattleStatusData.dp;
                            damage = damage <= 0 ? 1 : damage;
                            enemyBattleStatusData.hp -= damage;
                            //if () { }
                            // else{}
                            setStr = $"敵は{damage}ダメージ受けた";
                            break;
                        case BattleMainSelectAction.Guard:
                            if (AttackerFlg)
                            {
                                setStr = $"{playerBattleStatusData.playerName}は攻撃を防いだ";
                                phase = BattlePhase.Start_Wait;
                            }
                            else
                                setStr = $"{playerBattleStatusData.playerName}は攻撃を防ぐことが出来なかった";
                            break;
                        case BattleMainSelectAction.Skill:
                            damage = playerBattleStatusData.map - enemyBattleStatusData.mdp;
                            damage = damage <= 0 ? 1 : damage;
                            enemyBattleStatusData.hp -= damage;
                            setStr = $"敵は{damage}ダメージ受けた";
                            break;
                        case BattleMainSelectAction.UseItem:
                            playerBattleStatusData.hp += 10;
                            playerBattleStatusData.hp = playerBattleStatusData.hp >= playerMaxHp ? playerMaxHp : playerBattleStatusData.hp;
                            setStr = $"{playerBattleStatusData.playerName}は体力を回復した";
                            break;
                        case BattleMainSelectAction.Escape:
                            break;
                    }
                    break;
            }
        }
        
        // リザルト処理
        private void GetExp()
        {
            playerBattleStatusData.exp += enemyBattleStatusData.dropExp;
            clickCount++;
            setStr = $"{enemyBattleStatusData.dropExp}経験値獲得しました";
        }
        private bool LvUpCheck()
        {
            int baseUpExp = 10;
            baseUpExp = baseUpExp << (playerBattleStatusData.lv - 1);
            return baseUpExp <= playerBattleStatusData.exp;
        }
        private void LvUpProcess()
        {
            int baseUpExp = 10;
            baseUpExp = baseUpExp << (playerBattleStatusData.lv - 1);
            playerBattleStatusData.lv++;
            if (playerBattleStatusData.lv % 10 == 0)
            {
                playerBattleStatusData.hp += 10;
                playerBattleStatusData.mp += 10;
                playerBattleStatusData.ap += 5;
                playerBattleStatusData.dp += 5;
                playerBattleStatusData.map += 5;
                playerBattleStatusData.mdp += 5;
                playerBattleStatusData.sp += 5;
            }
            playerBattleStatusData.exp -= baseUpExp;
            setStr = $"レベルが上がりました";
        }

        // 速度チェック
        private void SpeedCheck()
        {
            AttackerFlg = playerBattleStatusData.sp < enemyBattleStatusData.sp ? false : true;
        }

        // ボタン
        private void RemoveButton_Event()
        {
            for (int i = 0; i < selectButtontBox.transform.childCount; i++)
            {
                Transform childTransform = selectButtontBox.transform.GetChild(i);
                childTransform.gameObject.SetActive(true);
                childTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
        private void SetButton_BattleEvent()
        {
            var actionStr = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            switch (actionStr)
            {
                case "攻撃":
                    battleAction = BattleMainSelectAction.Attack;
                    break;
                case "防御":
                    battleAction = BattleMainSelectAction.Guard;
                    break;
                case "特技":
                    battleAction = BattleMainSelectAction.Skill;
                    break;
                case "道具":
                    battleAction = BattleMainSelectAction.UseItem;
                    break;
                case "逃走":
                    battleAction = BattleMainSelectAction.Escape;
                    break;
                default:
                    break;
            }
            selectButtontBox.SetActive(false);
            phase = BattlePhase.Battle_PhaseONE_Start;
        }
        private void SetButton_StatusUpEvent()
        {
            var selectStatusStr = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            switch (selectStatusStr)
            {
                case "体力":
                    playerBattleStatusData.hp += 10;
                    break;
                case "魔力":
                    playerBattleStatusData.mp += 10;
                    break;
                case "攻撃力":
                    playerBattleStatusData.ap += 1;
                    break;
                case "防御力":
                    playerBattleStatusData.dp += 1;
                    break;
                case "魔法攻撃力":
                    playerBattleStatusData.map += 1;
                    break;
                case "魔法防御力":
                    playerBattleStatusData.mdp += 1;
                    break;
                case "俊敏力":
                    playerBattleStatusData.sp += 1;
                    break;
            }
            setStr = "ステータスが上昇した";
            selectButtontBox.SetActive(false);
            phase = BattlePhase.End;
        }
        private void SetBuutton_BattleText(int num)
        {
            selectButtonWord = new string[num];
            for (int i = 0; i < num; i++)
            {
                switch ((BattleMainSelectAction)i)
                {
                    case BattleMainSelectAction.Attack:
                        selectButtonWord[i] = "攻撃";
                        break;
                    case BattleMainSelectAction.Guard:
                        selectButtonWord[i] = "防御";
                        break;
                    case BattleMainSelectAction.Skill:
                        selectButtonWord[i] = "特技";
                        break;
                    case BattleMainSelectAction.UseItem:
                        selectButtonWord[i] = "道具";
                        break;
                    case BattleMainSelectAction.Escape:
                        selectButtonWord[i] = "逃走";
                        break;
                    default:
                        break;
                }
            }
        }
        private void SetButton_StatusUpText(int num)
        {
            selectButtonWord = new string[num];
            for (int i = 0; i < num; i++)
            {
                switch ((StatustNames)i)
                {
                    case StatustNames.hp:
                        selectButtonWord[i] = "体力";
                        break;
                    case StatustNames.mp:
                        selectButtonWord[i] = "魔力";
                        break;
                    case StatustNames.ap:
                        selectButtonWord[i] = "攻撃力";
                        break;
                    case StatustNames.dp:
                        selectButtonWord[i] = "防御力";
                        break;
                    case StatustNames.map:
                        selectButtonWord[i] = "魔法攻撃力";
                        break;
                    case StatustNames.mdp:
                        selectButtonWord[i] = "魔法防御力";
                        break;
                    case StatustNames.sp:
                        selectButtonWord[i] = "俊敏力";
                        break;
                }
            }
        }
        // 表示する選択ボタンに文字を挿入、イベント設定
        void SetButton_BattleProcess()
        {
            RemoveButton_Event();
            int selectMainActionNum = Enum.GetValues(typeof(BattleMainSelectAction)).Length;
            SetBuutton_BattleText(selectMainActionNum);
            for (int i = 0; i < selectButtontBox.transform.childCount; i++)
            {
                Transform childTransform = selectButtontBox.transform.GetChild(i);
                if (i >= selectMainActionNum)
                {
                    childTransform.gameObject.SetActive(false);
                }
                else
                {
                    childTransform.GetChild(0).GetComponent<Text>().text = selectButtonWord[i];
                    childTransform.GetComponent<Button>().onClick.AddListener(SetButton_BattleEvent);
                }
            }
        }
        void SetButton_StatusUpProccess()
        {
            setStr = "どのステータスを上げますか？";
            RemoveButton_Event();
            selectButtontBox.SetActive(true);
            int selectMainActionNum = Enum.GetValues(typeof(StatustNames)).Length;
            SetButton_StatusUpText(selectMainActionNum);
            for (int i = 0; i < selectButtontBox.transform.childCount; i++)
            {
                Transform childTransform = selectButtontBox.transform.GetChild(i);
                if (i >= selectMainActionNum)
                {
                    childTransform.gameObject.SetActive(false);
                }
                else
                {
                    childTransform.GetChild(0).GetComponent<Text>().text = selectButtonWord[i];
                    childTransform.GetComponent<Button>().onClick.AddListener(SetButton_StatusUpEvent);
                }
            }
        }

        // テキスト
        private void Text_Start()
        {
            //if ()
            //{
            //  setText = $"{enemyStatusData.enemyName}が現れた";
            //}
            //else
            //{
            setStr = "敵が現れた";
            //}

        }
        private void Text_Wait()
        {
            setStr = "どうする？";
        }
        private void Text_Battle(int openBattleText_SelectName)
        {
            if (openBattleText_SelectName==0)
            {
                PlayerBattleProcess();
            }
            if (openBattleText_SelectName == 1)
            {
                EnemyBattleProcess(clickCount);
            }
        }
        private void Text_Result()
        {   
            if (playerBattleStatusData.hp <= 0)
            {
                setStr = $"{playerBattleStatusData.playerName}は戦闘で敗北した";
                phase = BattlePhase.End;
            }
            else if (enemyBattleStatusData.hp <= 0)
            {
                setStr = $"{playerBattleStatusData.playerName}は戦闘で勝利した";
                int[] mapData = stockData.GetStageData();
                var data = stockData.GetPlay_SearchAchievementRateData();
                for (int i = 0; i < data.play_SearchStages.Length; i++)
                {
                    if (data.play_SearchStages[i].mapNumber == mapData[0] && data.play_SearchStages[i].stageNumber == mapData[1])
                    {
                        data.play_SearchStages[i].clearFlag = true;
                        break;
                    }
                }
                stockData.SetPlay_SearchAchievementRateData(data);
                Play_SearchAchievementRateDataAccess play_SearchAchievementRateDataAccess = new Play_SearchAchievementRateDataAccess();
                play_SearchAchievementRateDataAccess.fileName = stockData.GetFileName()[2];
                play_SearchAchievementRateDataAccess.Play_SearchAchievementRateSave(data);
                phase = BattlePhase.Result;
            }
            else if (battleAction == BattleMainSelectAction.Escape)
            {
                setStr = $"{playerBattleStatusData.playerName}は戦闘から離脱した";
                phase = BattlePhase.End;
            }
            else
            {
                phase = phase == BattlePhase.Battle_PhaseONE_EndCheck ?BattlePhase.Battle_PhaseTWO_Start: BattlePhase.Select;
            }
        }

        // メイン処理
        private void MainBattleProcess()
        {
            switch (phase)
            {
                case BattlePhase.Start:
                    selectButtontBox.SetActive(false);
                    Text_Start();
                    phase = BattlePhase.Start_Wait;
                    break;
                case BattlePhase.Start_Wait:
                    if (inputCheck) phase = BattlePhase.Select;
                    return;
                case BattlePhase.Select:
                    clickCount = 0;
                    selectButtontBox.SetActive(true);
                    SetButton_BattleProcess();
                    Text_Wait();
                    phase = BattlePhase.Select_Wait;
                    break;
                case BattlePhase.Select_Wait:
                    return;
                case BattlePhase.Battle_PhaseONE_Start:
                    SpeedCheck();
                    int attacker_Num = 0;
                    if (AttackerFlg)
                    {
                        attacker_Num = 0;
                    }
                    else
                    {
                        attacker_Num = 1;
                    }
                    Text_Battle(attacker_Num);
                    clickCount++;
                    phase = phase == BattlePhase.Battle_PhaseONE_Start ? BattlePhase.Battle_PhaseONE_Wait : phase;
                    break;
                case BattlePhase.Battle_PhaseONE_Wait:
                    if (inputCheck && clickCount != 2)
                    {
                        phase = BattlePhase.Battle_PhaseONE_Start;
                        break;
                    }
                    else if (inputCheck && clickCount == 2)
                    {
                        clickCount = 0;
                        phase = BattlePhase.Battle_PhaseONE_EndCheck;
                        break;
                    }
                    return;
                case BattlePhase.Battle_PhaseONE_EndCheck:
                    Text_Result();
                    break;
                case BattlePhase.Battle_PhaseTWO_Start:
                    if (AttackerFlg)
                    {
                        attacker_Num = 1;
                    }
                    else
                    {
                        attacker_Num = 0;
                    }
                    Text_Battle(attacker_Num);
                    clickCount++;
                    phase = BattlePhase.Battle_PhaseTWO_Wait;
                    break;
                case BattlePhase.Battle_PhaseTWO_Wait:
                    if (inputCheck && clickCount != 2)
                    {
                        phase = BattlePhase.Battle_PhaseTWO_Start;
                        break;
                    }
                    else if (inputCheck && clickCount == 2)
                    {
                        clickCount = 0;
                        phase = BattlePhase.Battle_PhaseTWO_EndCheck;
                        break;
                    }
                    break;
                case BattlePhase.Battle_PhaseTWO_EndCheck:
                    Text_Result();
                    break;
                case BattlePhase.Result:
                    if (inputCheck)
                    {
                        switch (clickCount)
                        {
                            case 0:
                                GetExp();
                                if (!LvUpCheck())
                                {
                                    phase = BattlePhase.End;
                                }
                                break;
                            case 1:
                                LvUpProcess();
                                clickCount++;
                                break;
                            default:
                                SetButton_StatusUpProccess();
                                break;
                        }
                    }
                    break;
                case BattlePhase.End:
                    if (inputCheck)
                    {
                        playerBattleStatusData.hp = playerMaxHp;
                        PlayerStatusDataAccess playerStatusDataAccess = new PlayerStatusDataAccess();
                        playerStatusDataAccess.fileName = stockData.GetFileName()[0];
                        playerStatusDataAccess.PlayerStatusDataSeva(playerBattleStatusData);
                        stockData.SetPlayerStatusData(playerBattleStatusData);
                        phase = BattlePhase.SceneChange;
                    }
                    break;
                case BattlePhase.SceneChange:
                    SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                    sceneChangeSystem.SceneChange(SceneNames.Home);
                    break;
            }
            textBox.transform.GetChild(0).GetComponent<Text>().text = setStr;
        }
    }
}