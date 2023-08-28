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
                        // MapIdごとに文字として保管
                        getStrData = mapData.mapStringData.Split('_');
                        // Idからマップ名に交換
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
                        // 今のマップの深度が0ならマップ名として表示
                        if (mapData.mapDepth == 0)
                        {
                            mapNameTextObject.transform.GetChild(0).GetComponent<Text>().text = mapData.mapName;
                        }
                        // ボタンの設定
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
                        // 選択ボタンとマップ名は非表示
                        SelectHomeButtonBox.SetActive(false);
                        mapNameTextObject.SetActive(false);
                        // データから必要情報を分割し取得
                        getStrData = mapData.mapStringData.Split('_');
                        // パネルボックスは表示
                        PanelBox.SetActive(true);
                        // 基礎パネルを表示
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
                                    string lastStatusText = $"プレイヤー名：{StackPlayData.Instance.GetPlayerData_PlayData().playerName}　種族：{ statusWord[0] }　職業：{ statusWord[1] }";

                                    for (int i = 0; i < lastStatusText.Length; i++)
                                    {
                                        statusWord[2] += "---";
                                    }

                                    for (int i = 0; i < access_DataBank.skillData_.Length; i++)
                                    {
                                        if (StackPlayData.Instance.GetPlayerData_SkillData().skillOpenFlgs[i])
                                        {
                                            statusWord[3] += $"{access_DataBank.skillData_[i].skillName}:Lv{StackPlayData.Instance.GetPlayerData_SkillData().skillLvDatas[i]}　";
                                        }
                                    }
                                    lastStatusText += $"\n{statusWord[2]}\nSTR：{playerStatus.p_str}　VIT：{playerStatus.p_vit}　INT：{playerStatus.p_edu}　DEX：{playerStatus.p_dex}　POW：{playerStatus.p_pow}　LUC：{playerStatus.p_luc}\nスキル\n{statusWord[3]}";
                                    panelChildTransform.gameObject.SetActive(true);
                                    panelChildTransform.GetComponent<Text>().text = lastStatusText;
                                }
                                break;
                            case HomePanelIdEnums.Enlargement:
                                panelChildTransform = PanelBox.transform.Find(getStrData[0]).Find("EnlargementText");
                                panelChildTransform.gameObject.SetActive(true);
                                //                            if ()
                                {
                                    panelChildTransform.GetComponent<Text>().text = "準備中";
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
                                        myItemName.Add($"{access_DataBank.itemData_[i].itemName}　x　{itemData_Private_.itemDatas[i]}");
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
            // マップIdからデータ取得
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
            // マップIdからデータ取得
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
            // テキスト取得
            string getMapName = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;
            // 次への場合はページ追加してボタン情報の更新
            if (buttonAssistText[0] == getMapName)
            {
                selectPageNumber++;
                SelectAction();
            }
            // 戻るの場合
            else if (buttonAssistText[1] == getMapName)
            {
                if (selectPageNumber == 0)
                {
                    mapMoveLog.Remove(mapData.mapNumber);
                    SearchMapNumber(mapMoveLog[mapMoveLog.Count - 1]);
                }
                else
                {
                    // ページ減らしてボタン情報の更新
                    selectPageNumber--;
                    SelectAction();
                }
            }
            // 戻る以外の場合
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
            buttonAssistText[0] = "次へ";
            buttonAssistText[1] = "戻る";
            // データバンクのデータを読み込む
            resourcesDataFaileNames = StackPlayData.Instance.resourcesDataFaileNames;
            access_DataBank.Load_Resources(resourcesDataFaileNames);
            // 現在のマップIdをStackPlayDataから貰う
            int getMapNumber = StackPlayData.Instance.GetPlayerData_PlayData().lastMapNumber;
            // マップ情報に合わせた処理を行う
            SearchMapNumber(getMapNumber);
            mapMoveLog.Add(mapData.mapNumber);
            // 現在のマップ名を表示
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