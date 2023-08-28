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
        GameObject textBox;                     // �s���\������I�u�W�F�N�g
        [SerializeField]
        GameObject selectButtontBox;            // �I���{�^���i�[�I�u�W�F�N�g
        [SerializeField]
        EventSystem eventSystem;                // �N���b�N�����I�u�W�F�N�g�擾�p

        private IStockData stockData;                               // �ꎞ�ۊǂ��ꂽ�f�[�^�̎擾�p
        private PlayerStatus_Structure playerBattleStatusData;      // �v���C���̃o�g���X�e�[�^�X�f�[�^
        private EnemyStatus_Structure enemyBattleStatusData;        // �G�̃o�g���X�e�[�^�X�f�[�^
       
        private BattlePhase phase = BattlePhase.Start;
        private BattleMainSelectAction battleAction;                // �I�������R�}���h
        private string setStr;                                      // �\������e�L�X�g
        string[] selectButtonWord;
        private bool AttackerFlg;                                   // �搧�U���ł��邩
        private bool inputCheck;
        private int playerMaxHp;                                    // �v���C���̍ő�HP
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

        // �o�g������
        public void EnemyBattleProcess(int clickCount)
        {
            // �ʓ|������U���������Ȃ�
            switch (clickCount)
            {
                case 0:
                    //if ()
                    //{
                    //  setText = $"{enemyStatusData.enemyName}�̍U��";
                    //}
                    //else
                    //{
                    setStr = "�G�̍U��";
                    //}
                    break;
                case 1:
                    int damage = enemyBattleStatusData.ap - playerBattleStatusData.dp;
                    damage = damage <= 0 ? 1 : damage;
                    playerBattleStatusData.hp -= damage;
                    setStr = $"{playerBattleStatusData.playerName}��{damage}�_���[�W�󂯂�";
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
                            setStr = $"{playerBattleStatusData.playerName}�͍U������";
                            break;
                        case BattleMainSelectAction.Guard:
                            setStr = $"{playerBattleStatusData.playerName}�͖h�䂵��";
                            break;
                        case BattleMainSelectAction.Skill:
                            setStr = $"{playerBattleStatusData.playerName}�̓X�L���𔭓�����";
                            break;
                        case BattleMainSelectAction.UseItem:
                            setStr = $"{playerBattleStatusData.playerName}�͖򑐂��g�p����";
                            break;
                        case BattleMainSelectAction.Escape:
                            setStr = $"{playerBattleStatusData.playerName}�͓�������";
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
                            setStr = $"�G��{damage}�_���[�W�󂯂�";
                            break;
                        case BattleMainSelectAction.Guard:
                            if (AttackerFlg)
                            {
                                setStr = $"{playerBattleStatusData.playerName}�͍U����h����";
                                phase = BattlePhase.Start_Wait;
                            }
                            else
                                setStr = $"{playerBattleStatusData.playerName}�͍U����h�����Ƃ��o���Ȃ�����";
                            break;
                        case BattleMainSelectAction.Skill:
                            damage = playerBattleStatusData.map - enemyBattleStatusData.mdp;
                            damage = damage <= 0 ? 1 : damage;
                            enemyBattleStatusData.hp -= damage;
                            setStr = $"�G��{damage}�_���[�W�󂯂�";
                            break;
                        case BattleMainSelectAction.UseItem:
                            playerBattleStatusData.hp += 10;
                            playerBattleStatusData.hp = playerBattleStatusData.hp >= playerMaxHp ? playerMaxHp : playerBattleStatusData.hp;
                            setStr = $"{playerBattleStatusData.playerName}�̗͑͂��񕜂���";
                            break;
                        case BattleMainSelectAction.Escape:
                            break;
                    }
                    break;
            }
        }
        
        // ���U���g����
        private void GetExp()
        {
            playerBattleStatusData.exp += enemyBattleStatusData.dropExp;
            clickCount++;
            setStr = $"{enemyBattleStatusData.dropExp}�o���l�l�����܂���";
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
            setStr = $"���x�����オ��܂���";
        }

        // ���x�`�F�b�N
        private void SpeedCheck()
        {
            AttackerFlg = playerBattleStatusData.sp < enemyBattleStatusData.sp ? false : true;
        }

        // �{�^��
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
                case "�U��":
                    battleAction = BattleMainSelectAction.Attack;
                    break;
                case "�h��":
                    battleAction = BattleMainSelectAction.Guard;
                    break;
                case "���Z":
                    battleAction = BattleMainSelectAction.Skill;
                    break;
                case "����":
                    battleAction = BattleMainSelectAction.UseItem;
                    break;
                case "����":
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
                case "�̗�":
                    playerBattleStatusData.hp += 10;
                    break;
                case "����":
                    playerBattleStatusData.mp += 10;
                    break;
                case "�U����":
                    playerBattleStatusData.ap += 1;
                    break;
                case "�h���":
                    playerBattleStatusData.dp += 1;
                    break;
                case "���@�U����":
                    playerBattleStatusData.map += 1;
                    break;
                case "���@�h���":
                    playerBattleStatusData.mdp += 1;
                    break;
                case "�r�q��":
                    playerBattleStatusData.sp += 1;
                    break;
            }
            setStr = "�X�e�[�^�X���㏸����";
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
                        selectButtonWord[i] = "�U��";
                        break;
                    case BattleMainSelectAction.Guard:
                        selectButtonWord[i] = "�h��";
                        break;
                    case BattleMainSelectAction.Skill:
                        selectButtonWord[i] = "���Z";
                        break;
                    case BattleMainSelectAction.UseItem:
                        selectButtonWord[i] = "����";
                        break;
                    case BattleMainSelectAction.Escape:
                        selectButtonWord[i] = "����";
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
                        selectButtonWord[i] = "�̗�";
                        break;
                    case StatustNames.mp:
                        selectButtonWord[i] = "����";
                        break;
                    case StatustNames.ap:
                        selectButtonWord[i] = "�U����";
                        break;
                    case StatustNames.dp:
                        selectButtonWord[i] = "�h���";
                        break;
                    case StatustNames.map:
                        selectButtonWord[i] = "���@�U����";
                        break;
                    case StatustNames.mdp:
                        selectButtonWord[i] = "���@�h���";
                        break;
                    case StatustNames.sp:
                        selectButtonWord[i] = "�r�q��";
                        break;
                }
            }
        }
        // �\������I���{�^���ɕ�����}���A�C�x���g�ݒ�
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
            setStr = "�ǂ̃X�e�[�^�X���グ�܂����H";
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

        // �e�L�X�g
        private void Text_Start()
        {
            //if ()
            //{
            //  setText = $"{enemyStatusData.enemyName}�����ꂽ";
            //}
            //else
            //{
            setStr = "�G�����ꂽ";
            //}

        }
        private void Text_Wait()
        {
            setStr = "�ǂ�����H";
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
                setStr = $"{playerBattleStatusData.playerName}�͐퓬�Ŕs�k����";
                phase = BattlePhase.End;
            }
            else if (enemyBattleStatusData.hp <= 0)
            {
                setStr = $"{playerBattleStatusData.playerName}�͐퓬�ŏ�������";
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
                setStr = $"{playerBattleStatusData.playerName}�͐퓬���痣�E����";
                phase = BattlePhase.End;
            }
            else
            {
                phase = phase == BattlePhase.Battle_PhaseONE_EndCheck ?BattlePhase.Battle_PhaseTWO_Start: BattlePhase.Select;
            }
        }

        // ���C������
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