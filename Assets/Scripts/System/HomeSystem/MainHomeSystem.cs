using RPGCreateNow_Local.StockData;
using RPGCreateNow_Local.UseCase;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCreateNow_Local.System
{
    public class MainHomeSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject SelectHomeButtonBox;
        [SerializeField]
        GameObject PanelBox;
        [SerializeField]
        GameObject mapNameTextObject;
        [SerializeField]
        EventSystem eventSystem;

        string[] resourcesDataFaileNames;
        Access_DataBank access_DataBank = new Access_DataBank();
        MapData_BankData_Structure mapData = new MapData_BankData_Structure();

        int selectPageNumber = 0;
        string[] buttonAssistText = new string[2];
        const int MAINSELECTBUTTONNUM = 5;
        List<int> mapMoveLog = new List<int>();
        const int MAXSELECTNUM = 9;

        void SelectButton()
        {
            for (int i = 0; i < MAXSELECTNUM; i++)
            {

            }
        }


        #region Map
        private void SelectAction()
        {
            string[] getStrData;
            switch ((MapTypeEnums)mapData.mapType)
            {
                case MapTypeEnums.SetButtons:
                    {
                        // MapId���Ƃɕ����Ƃ��ĕۊ�
                        getStrData = mapData.mapStringData.Split('_');
                        // Id����}�b�v���Ɍ���
                        for (int i = 0; i < getStrData.Length; i++)
                        {
                            for (int j = 0; j < access_DataBank.mapData_.Length; j++)
                            {
                                if (access_DataBank.mapData_[j].mapNumber == int.Parse(getStrData[i]))
                                {
                                    getStrData[i] = access_DataBank.mapData_[j].mapName;
                                    break;
                                }
                            }
                        }
                        bool endFlg = false;
                        // ���̃}�b�v�̐[�x��0�Ȃ�}�b�v���Ƃ��ĕ\��
                        if (mapData.mapDepth == 0)
                        {
                            mapNameTextObject.transform.GetChild(0).GetComponent<Text>().text = mapData.mapName;
                        }
                        // �{�^���̐ݒ�
                        for (int i = 0; i < SelectHomeButtonBox.transform.childCount; i++)
                        {
                            Transform selectHomeButtonChildTransform = SelectHomeButtonBox.transform.GetChild(i);
                            selectHomeButtonChildTransform.gameObject.SetActive(false);
                            selectHomeButtonChildTransform.GetComponent<Button>().onClick.RemoveAllListeners();
                            int checkNumber = selectPageNumber * MAINSELECTBUTTONNUM + i;
                            if (!endFlg)
                            {
                                if (checkNumber < getStrData.Length && i < MAINSELECTBUTTONNUM)
                                {
                                    selectHomeButtonChildTransform.gameObject.SetActive(true);
                                    selectHomeButtonChildTransform.GetChild(0).GetComponent<Text>().text = getStrData[checkNumber];
                                    selectHomeButtonChildTransform.GetComponent<Button>().onClick.AddListener(HomeMainButton);
                                }
                                else if (checkNumber < getStrData.Length && i == MAINSELECTBUTTONNUM)
                                {
                                    selectHomeButtonChildTransform.gameObject.SetActive(true);
                                    selectHomeButtonChildTransform.GetChild(0).GetComponent<Text>().text = buttonAssistText[0];
                                    selectHomeButtonChildTransform.GetComponent<Button>().onClick.AddListener(HomeMainButton);
                                }
                                else if ((selectPageNumber != 0) || (mapData.mapDepth != 0))
                                {
                                    selectHomeButtonChildTransform.gameObject.SetActive(true);
                                    selectHomeButtonChildTransform.GetChild(0).GetComponent<Text>().text = buttonAssistText[1];
                                    selectHomeButtonChildTransform.GetComponent<Button>().onClick.AddListener(HomeMainButton);
                                    endFlg = true;
                                }
                            }
                        }
                        endFlg = false;
                    }
                    break;
                case MapTypeEnums.OpenPanel:
                    {
                        // �I���{�^���ƃ}�b�v���͔�\��
                        SelectHomeButtonBox.SetActive(false);
                        mapNameTextObject.SetActive(false);
                        // �f�[�^����K�v���𕪊����擾
                        getStrData = mapData.mapStringData.Split('_');
                        // �p�l���{�b�N�X�͕\��
                        PanelBox.SetActive(true);
                        // ��b�p�l����\��
                        Transform panelChildTransform = PanelBox.transform.Find(getStrData[0]);
                        panelChildTransform.gameObject.SetActive(true);

                        switch ((HomePanelIdEnums)mapData.mapNumber)
                        {
                            case HomePanelIdEnums.Status:
                                {
                                    panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find("LastStatusText");
                                    string[] statusWord = new string[4];
                                    StatusData_Private_Structure playerStatus = StackPlayData.Instance.GetPlayerData_StatusData();
                                    for (int i = 0; i < access_DataBank.specoesData_.Length; i++)
                                    {
                                        if (playerStatus.species[0] == access_DataBank.specoesData_[i].specoesId)
                                        {
                                            statusWord[0] = access_DataBank.specoesData_[i].specoesName;
                                            break;
                                        }
                                    }
                                    for (int i = 0; i < access_DataBank.jobData_.Length; i++)
                                    {
                                        if (playerStatus.job[0] == access_DataBank.jobData_[i].jobId)
                                        {
                                            statusWord[1] = access_DataBank.jobData_[i].jobName;
                                            break;
                                        }
                                    }
                                    string lastStatusText = $"�v���C���[���F{StackPlayData.Instance.GetPlayerData_PlayData().playerName}�@�푰�F{ statusWord[0] }�@�E�ƁF{ statusWord[1] }";

                                    for (int i = 0; i < lastStatusText.Length; i++)
                                    {
                                        statusWord[2] += "---";
                                    }

                                    for (int i = 0; i < access_DataBank.skillData_.Length; i++)
                                    {
                                        if (StackPlayData.Instance.GetPlayerData_SkillData().skillOpenFlgs[i])
                                        {
                                            statusWord[3] += $"{access_DataBank.skillData_[i].skillName}:Lv{StackPlayData.Instance.GetPlayerData_SkillData().skillLvDatas[i]}�@";
                                        }
                                    }
                                    lastStatusText += $"\n{statusWord[2]}\nSTR�F{playerStatus.p_str}�@VIT�F{playerStatus.p_vit}�@INT�F{playerStatus.p_edu}�@DEX�F{playerStatus.p_dex}�@POW�F{playerStatus.p_pow}�@LUC�F{playerStatus.p_luc}\n�X�L��\n{statusWord[3]}";
                                    panelChildTransform.gameObject.SetActive(true);
                                    panelChildTransform.GetComponent<Text>().text = lastStatusText;
                                }
                                break;
                            case HomePanelIdEnums.Enlargement:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find("EnlargementText");
                                panelChildTransform.gameObject.SetActive(true);
                                //                            if ()
                                {
                                    panelChildTransform.GetComponent<Text>().text = "������";
                                    panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find("MyMoneyTextBox");
                                    panelChildTransform.Find("MyMoneyText (1)").GetComponent<Text>().text = StackPlayData.Instance.GetPlayerData_PlayData().myMoney.ToString();
                                    panelChildTransform.gameObject.SetActive(true);
                                    panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find("EnlargementButton");
                                    panelChildTransform.gameObject.SetActive(false);
                                }
                                break;
                            case HomePanelIdEnums.Storage:
                                List<string> myItemName = new List<string>();
                                ItemData_Private_Structure itemData_Private_ = StackPlayData.Instance.GetPlayerData_ItemData();
                                for (int i = 0; i < itemData_Private_.itemDatas.Length; i++)
                                {
                                    if (itemData_Private_.itemDatas[i] != 0)
                                    {
                                        myItemName.Add($"{access_DataBank.itemData_[i].itemName}�@x�@{itemData_Private_.itemDatas[i]}");
                                    }
                                }
                                for (int i = 0; i < MAXSELECTNUM; i++)
                                {
                                    Transform selectStorageButtonChildTransform = PanelBox.transform.Find(getStrData[0]).Find($"SelectButton{i}");
                                    selectStorageButtonChildTransform.gameObject.SetActive(false);
                                    selectStorageButtonChildTransform.GetComponent<Button>().onClick.RemoveAllListeners();
                                    int checkNumber = selectPageNumber * MAINSELECTBUTTONNUM + i;
                                }

                                break;
                            case HomePanelIdEnums.WeaponShop:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find(getStrData[1]);
                                panelChildTransform.gameObject.SetActive(true);
                                break;
                            case HomePanelIdEnums.ArmorShop:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find(getStrData[1]);
                                panelChildTransform.gameObject.SetActive(true);
                                break;
                            case HomePanelIdEnums.ToolShop:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find(getStrData[1]);
                                panelChildTransform.gameObject.SetActive(true);
                                break;
                            case HomePanelIdEnums.DrugShop:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find(getStrData[1]);
                                panelChildTransform.gameObject.SetActive(true);
                                break;
                            case HomePanelIdEnums.ChangeJob:
                                break;
                            case HomePanelIdEnums.Books:
                                break;
                            case HomePanelIdEnums.Collection:
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case MapTypeEnums.OpenText:
                    break;
                case MapTypeEnums.ChangeScene:
                    break;
                default:
                    break;
            }
        }

        private void SearchMapNumber(int mapNumber)
        {
            // �}�b�vId����f�[�^�擾
            for (int i = 0; i < access_DataBank.mapData_.Length; i++)
            {
                if (access_DataBank.mapData_[i].mapNumber == mapNumber)
                {
                    mapData = access_DataBank.mapData_[i];
                    break;
                }
            }
            SelectAction();
        }
        public void SearchMapName(string getMapName)
        { 
            // �}�b�vId����f�[�^�擾
            for (int i = 0; i < access_DataBank.mapData_.Length; i++)
            {
                if (access_DataBank.mapData_[i].mapName == getMapName)
                {
                    mapData = access_DataBank.mapData_[i];
                    break;
                }
            }
            SelectAction();
        }
        public void HomeMainButton()
        {
            // �e�L�X�g�擾
            string getMapName = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            // ���ւ̏ꍇ�̓y�[�W�ǉ����ă{�^�����̍X�V
            if (buttonAssistText[0] == getMapName)
            {
                selectPageNumber++;
                SelectAction();
            }
            // �߂�̏ꍇ
            else if (buttonAssistText[1] == getMapName)
            {
                if (selectPageNumber == 0)
                {
                    mapMoveLog.Remove(mapData.mapNumber);
                    SearchMapNumber(mapMoveLog[mapMoveLog.Count - 1]);
                }
                else
                {
                    // �y�[�W���炵�ă{�^�����̍X�V
                    selectPageNumber--;
                    SelectAction();
                }
            }
            // �߂�ȊO�̏ꍇ
            else
            {
                selectPageNumber = 0;
                SearchMapName(getMapName);
                mapMoveLog.Add(mapData.mapNumber);
            }
        }
        #endregion

        #region Panel 
        public void PanelEndButton()
        {
            for (int i = 0; i < PanelBox.transform.childCount; i++)
            {
                Transform childTransform = PanelBox.transform.GetChild(i);
                childTransform.gameObject.SetActive(false);
                for (int j = 0; j < childTransform.transform.childCount; j++)
                {
                    Transform nextChildTransform = childTransform.GetChild(i);
                    nextChildTransform.gameObject.SetActive(false);
                }
            }
            PanelBox.SetActive(false);
            SelectHomeButtonBox.SetActive(true);
            mapNameTextObject.SetActive(true);
            mapMoveLog.Remove(mapData.mapNumber);
            SearchMapNumber(mapMoveLog[mapMoveLog.Count - 1]);
        }

        #endregion

        #region Text
        #endregion
        #region Scene
        #endregion

        private void Start()
        {
            buttonAssistText[0] = "����";
            buttonAssistText[1] = "�߂�";
            // �f�[�^�o���N�̃f�[�^��ǂݍ���
            resourcesDataFaileNames = StackPlayData.Instance.resourcesDataFaileNames;
            access_DataBank.Load_Resources(resourcesDataFaileNames);
            // ���݂̃}�b�vId��StackPlayData����Ⴄ
            int getMapNumber = StackPlayData.Instance.GetPlayerData_PlayData().lastMapNumber;
            // �}�b�v���ɍ��킹���������s��
            SearchMapNumber(getMapNumber);
            mapMoveLog.Add(mapData.mapNumber);
            // ���݂̃}�b�v����\��
            mapNameTextObject.transform.GetChild(0).GetComponent<Text>().text = mapData.mapName;

            /*
            homeButtonSystem.SelectBox = SelectBox;
            homeButtonSystem.eventSystem = eventSystem;
            homeButtonSystem.SetSelectHomeButton();
            */
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                for (int i = 0; i < mapMoveLog.Count; i++)
                {
                    Debug.Log(mapMoveLog[i]);
                }
            }
        }
    }
}