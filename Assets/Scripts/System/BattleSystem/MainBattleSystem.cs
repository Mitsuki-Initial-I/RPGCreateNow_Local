using UnityEngine;
using RPGCreateNow_Local.UseCase;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using RPGCreateNow_Local.Data;

namespace RPGCreateNow_Local.System
{
    public class MainBattleSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject textBox;
        [SerializeField]
        GameObject buttontBox;
        [SerializeField]
        EventSystem eventSystem;

        PlayerStatus_Structure playerStatusData;
        EnemyStatus_Structure enemyStatusData=new EnemyStatus_Structure();
        IStockData stockData;

        int battleTurn = 0;
        int playerMaxHp;
        BattlePhase phase = BattlePhase.Start;

        string[] setWord;
        bool AttackerFlg;
        string actionWord;

        private void SelectButtonReset()
        {
            for (int i = 0; i < buttontBox.transform.childCount; i++)
            {
                Transform childTransform = buttontBox.transform.GetChild(i);
                childTransform.gameObject.SetActive(true);
                childTransform.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
        void SetWord_ButtonMainSelectAction(int num)
        {
            setWord = new string[num];
            for (int i = 0; i < num; i++)
            {
                switch ((BattleMainSelectAction)i)
                {
                    case BattleMainSelectAction.Attack:
                        setWord[i] = "çUåÇ";
                        break;
                    case BattleMainSelectAction.Guard:
                        setWord[i] = "ñhå‰";
                        break;
                    case BattleMainSelectAction.Skill:
                        setWord[i] = "ì¡ãZ";
                        break;
                    case BattleMainSelectAction.UseItem:
                        setWord[i] = "ìπãÔ";
                        break;
                    case BattleMainSelectAction.Escape:
                        setWord[i] = "ì¶ëñ";
                        break;
                    default:
                        break;
                }
            }
        }
        void SetButtonEvent_ButtonMainSelectAction()
        {
            actionWord = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            buttontBox.SetActive(false);
            battleTurn++;
            BattleStart_SpeedCheck();
            textBox.transform.GetChild(0).GetComponent<Text>().text = Action1TextOpen_String(AttackerFlg);
            phase = BattlePhase.Action1ResultText;
        }
        void Set_ButtonMainSelect()
        {
            SelectButtonReset();
            int selectMainActionNum = Enum.GetValues(typeof(BattleMainSelectAction)).Length;
            SetWord_ButtonMainSelectAction(selectMainActionNum);
            for (int i = 0; i < buttontBox.transform.childCount; i++)
            {
                Transform childTransform = buttontBox.transform.GetChild(i);
                if (i >= selectMainActionNum)
                {
                    childTransform.gameObject.SetActive(false);
                }
                else
                {
                    childTransform.GetChild(0).GetComponent<Text>().text = setWord[i];
                    childTransform.GetComponent<Button>().onClick.AddListener(SetButtonEvent_ButtonMainSelectAction);
                }
            }
        }

        void BattleStart_SpeedCheck() 
        {
            AttackerFlg = playerStatusData.sp < enemyStatusData.sp ? false : true;
        }

        string PlayerAction_String()
        {
            switch (actionWord)
            {
                case "çUåÇ":
                    return $"{playerStatusData.playerName}ÇÃçUåÇ";
                case "ñhå‰":
                    return $"{playerStatusData.playerName}ÇÕñhå‰ÇµÇΩ";
                case "ì¡ãZ":
                    return $"{playerStatusData.playerName}ÇÕì¡ãZÇégÇ¡ÇΩ";
                case "ìπãÔ":
                    return $"{playerStatusData.playerName}ÇÕìπãÔÇégÇ¡ÇΩ";
                case "ì¶ëñ":
                    return $"{playerStatusData.playerName}ÇÕì¶ëñÇ∑ÇÈÇ±Ç∆Ç…ÇµÇΩ";
                default:
                    return $"{playerStatusData.playerName}ÇÕÇ»Ç…Ç‡ÇµÇ»Ç©Ç¡ÇΩ";
            }
        }
        string PlayerActionResult_String()
        {
            int damage;
            switch (actionWord)
            {
                case "çUåÇ":
                    damage = playerStatusData.ap - enemyStatusData.dp;
                    damage = damage <= 0 ? 0 : damage;
                    enemyStatusData.hp -= damage; 
                    if (enemyStatusData.hp <= 0)
                    {
                        phase = BattlePhase.End;
                    }
                    return $"{damage}É_ÉÅÅ[ÉWó^Ç¶ÇΩ";
                case "ñhå‰":
                    if (AttackerFlg)
                    {
                        phase = BattlePhase.Start;
                        return $"{playerStatusData.playerName}ÇÕçUåÇÇñhÇ¢Çæ";
                    }
                    else
                        return $"{playerStatusData.playerName}ÇÕçUåÇÇñhÇÆÇ±Ç∆Ç™Ç≈Ç´Ç»Ç©Ç¡ÇΩ";
                case "ì¡ãZ":
                    return $"Ç»Ç…Ç‡ãNÇ´Ç»Ç©Ç¡ÇΩ";
                case "ìπãÔ":
                    playerStatusData.hp += 10;
                    playerStatusData.hp = playerStatusData.hp >= playerMaxHp ? playerMaxHp : playerStatusData.hp;
                    return $"10âÒïúÇµÇΩ";
                case "ì¶ëñ":
                    if (AttackerFlg)
                    {
                        phase = BattlePhase.End;
                        return $"{playerStatusData.playerName}ÇÕì¶ëñÇµÇΩ";
                    }
                    else
                        return $"{playerStatusData.playerName}ÇÕì¶ëñÇ∑ÇÈÇ±Ç∆Ç™Ç≈Ç´Ç»Ç©Ç¡ÇΩ";
                default:
                    return $"{playerStatusData.playerName}ÇÕÇ»Ç…Ç‡ÇµÇ»Ç©Ç¡ÇΩ";
            }
        }

        string Action1TextOpen_String(bool flg)
        {
            if (flg)
            {
                return PlayerAction_String();
            }
            else
            {
                return "ìGÇÃçUåÇ";
            }
        }
        string Action2TextOpen_String(bool flg)
        {
            if (!flg)
            {
                return PlayerAction_String();
            }
            else
            {
                return "ìGÇÃçUåÇ";
            }
        }
        string Action1ResultText_String(bool flg)
        {
            if (flg)
            {
                return PlayerActionResult_String();
            }
            else
            {
                int damage = enemyStatusData.ap - playerStatusData.dp;
                damage = damage <= 0 ? 0 : damage;
                if (playerStatusData.hp <= 0)
                {
                    phase = BattlePhase.End;
                }
                return $"{playerStatusData.playerName}ÇÕ{damage}É_ÉÅÅ[ÉWéÛÇØÇΩ";
            }
        }
        string Action2ResultText_String(bool flg)
        {
            if (!flg)
            {
                return PlayerActionResult_String();
            }
            else
            {
                int damage = enemyStatusData.ap - playerStatusData.dp;
                damage = damage <= 0 ? 0 : damage;
                if (playerStatusData.hp <= 0)
                {
                    phase = BattlePhase.End;
                }
                return $"{playerStatusData.playerName}ÇÕ{damage}É_ÉÅÅ[ÉWéÛÇØÇΩ";
            }
        }

        string GameResult()
        {
            phase = BattlePhase.SceneChange;
            if (playerStatusData.hp <= 0)
                return "ÉQÅ[ÉÄÉIÅ[ÉoÅ[";
            else if (enemyStatusData.hp <= 0)
            {
                playerStatusData.exp += enemyStatusData.dropExp;
                return "ÉQÅ[ÉÄÉNÉäÉA";
            }
            else
                return "êÌì¨Ç©ÇÁó£íEÇµÇΩ";
        }
        void ResultSceneChange()
        {
            SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
            if (playerStatusData.hp <= 0)
            {
                sceneChangeSystem.SceneChange(SceneNameS.Title);
            }
            else
            {
                playerStatusData.hp = playerMaxHp;
                if (playerStatusData.exp == 100 * playerStatusData.lv)
                {
                    playerStatusData.exp -= 100 * playerStatusData.lv;
                    playerStatusData.lv++;
                }
                IStockData setPlayerData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
                PlayerStatusDataAccess playerStatusDataAccess = new PlayerStatusDataAccess();
                playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
                setPlayerData.SetPlayerStatusData(playerStatusData);
                sceneChangeSystem.SceneChange(SceneNameS.Home);
            }
        }

        void Start()
        {
            stockData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            playerStatusData = stockData.GetPlayerStatusData();
            {
                enemyStatusData.enemyName = "ÉGÉXÉg";
                enemyStatusData.hp = 5;
                enemyStatusData.mp = 5;
                enemyStatusData.ap = 5;
                enemyStatusData.dp = 5;
                enemyStatusData.map = 5;
                enemyStatusData.mdp = 5;
                enemyStatusData.sp = 5;
                enemyStatusData.lv = 1;
                enemyStatusData.dropExp = 100;
            }
            buttontBox.SetActive(false);
            playerMaxHp = playerStatusData.hp;
            textBox.transform.GetChild(0).GetComponent<Text>().text = $"ìGÇ™åªÇÍÇΩ";
        }
        private void Update()
        {
            if (phase == BattlePhase.Wait)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                switch (phase)
                {
                    case BattlePhase.Start:
                        textBox.transform.GetChild(0).GetComponent<Text>().text = $"Ç«Ç§Ç∑ÇÈ?";
                        phase = BattlePhase.Wait;
                        Set_ButtonMainSelect();
                        buttontBox.SetActive(true);
                        break;
                    case BattlePhase.Action1ResultText:
                        textBox.transform.GetChild(0).GetComponent<Text>().text = Action1ResultText_String(AttackerFlg);
                        phase = phase == BattlePhase.Action1ResultText ? BattlePhase.EndCheck1: phase;
                        break;
                    case BattlePhase.EndCheck1:
                        textBox.transform.GetChild(0).GetComponent<Text>().text = Action2TextOpen_String(AttackerFlg);
                        phase = BattlePhase.Action2ResultText;
                        break;
                    case BattlePhase.Action2ResultText:
                        textBox.transform.GetChild(0).GetComponent<Text>().text = Action2ResultText_String(AttackerFlg);
                        phase = phase == BattlePhase.Action2ResultText ? BattlePhase.Start : phase;
                        break;
                    case BattlePhase.End:
                        textBox.transform.GetChild(0).GetComponent<Text>().text = GameResult();
                        break;
                    case BattlePhase.SceneChange:
                        ResultSceneChange();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}