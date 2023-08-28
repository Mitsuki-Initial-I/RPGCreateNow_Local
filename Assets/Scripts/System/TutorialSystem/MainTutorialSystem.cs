using RPGCreateNow_Local.Data;
using RPGCreateNow_Local.StockData;
using RPGCreateNow_Local.UseCase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCreateNow_Local.System
{
    public class MainTutorialSystem : MonoBehaviour
    {
        [SerializeField]
        EventSystem eventSystem;
        [SerializeField]
        GameObject[] settingObjects;
        [SerializeField]
        Text[] settingTextObjects;

        TutorialSettingStateEnums tutorialState;
        Access_DataBank access_DataBank = new Access_DataBank();
        string[] resourcesDataFaileNames;

        int pageNum = 0;
        int statusPoint = 10;

        string playerName;
        int specoesId;
        int jobId;
        int[] skillId = new int[3];
        int[] status = new int[6];

        const int DEFAULTBUFF = 5;
        const int MAXSELECTNUM = 9;

        void ResetObject()
        {
            for (int i = 0; i < settingObjects.Length; i++)
            {
                settingObjects[i].SetActive(false);
            }
            for (int i = 0; i < 6; i++)
            {
                settingObjects[(int)TutorialObjectNums.Step + i].SetActive(true);
            }
        }

        void Start()
        {
            ResetObject();
            // リソースデータを読み込む
            resourcesDataFaileNames = StackPlayData.Instance.resourcesDataFaileNames;
            access_DataBank.Load_Resources(resourcesDataFaileNames);
            tutorialState = TutorialSettingStateEnums.PlayerName;
            // 
            settingObjects[(int)TutorialObjectNums.PlayerName].SetActive(true);
            settingObjects[(int)TutorialObjectNums.NameButton].GetComponent<Button>().interactable = true;
        }

        public void NameCheck()
        {
            playerName = settingTextObjects[(int)TutorialTextObjectNums.PlayerName].text;
            playerName = playerName.Replace("\n", "").Replace(" ", "").Replace("　", "");
            if (playerName.Length <= 10 && playerName != "")
            {
                CheckTextUpdata();
            }
            else if (playerName.Length > 10)
            {
                settingTextObjects[(int)TutorialTextObjectNums.PlayerNameRead].text = "10文字以下で入力してください";
            }
            else if (playerName == "")
            {
                settingTextObjects[(int)TutorialTextObjectNums.PlayerNameRead].text = "文字を入力してください\n(空白は利用できません)";
            }
        }

        #region SJS
        void ButtonData()
        {
            // レアリティ0の配列番号を取得
            List<string> sjsDataLength = new List<string>();
            if (tutorialState == TutorialSettingStateEnums.Specoes)
            {
                for (int i = 0; i < access_DataBank.specoesData_.Length; i++)
                {
                    if (access_DataBank.specoesData_[i].specoesRarity == 0)
                    {
                        sjsDataLength.Add(access_DataBank.specoesData_[i].specoesName);
                    }
                }
            }
            else if (tutorialState == TutorialSettingStateEnums.Job)
            {
                for (int i = 0; i < access_DataBank.jobData_.Length; i++)
                {
                    if (access_DataBank.jobData_[i].jobRarity == 0)
                    {
                        sjsDataLength.Add(access_DataBank.jobData_[i].jobName);
                    }
                }
            }
            else if (tutorialState == TutorialSettingStateEnums.Skill)
            {
                for (int i = 0; i < access_DataBank.skillData_.Length; i++)
                {
                    if (access_DataBank.skillData_[i].skillRarity == 0)
                    {
                        sjsDataLength.Add(access_DataBank.skillData_[i].skillName);
                    }
                }
            }

            // ページによって次のページと前のページを選択出来ない様にする
            if (pageNum == 0)
            {
                settingObjects[(int)TutorialObjectNums.SJS].transform.Find("BackPageButton").gameObject.SetActive(false);
            }
            else
            {
                settingObjects[(int)TutorialObjectNums.SJS].transform.Find("BackPageButton").gameObject.SetActive(true);
            }
            int pageCheckNum = sjsDataLength.Count % MAXSELECTNUM == 0 ? sjsDataLength.Count / MAXSELECTNUM - 1 : sjsDataLength.Count / MAXSELECTNUM;

            if (pageNum < pageCheckNum)
            {
                settingObjects[(int)TutorialObjectNums.SJS].transform.Find("NextPageButton").gameObject.SetActive(true);
            }
            else
            {
                settingObjects[(int)TutorialObjectNums.SJS].transform.Find("NextPageButton").gameObject.SetActive(false);
            }

            // オブジェクト取得して、表示非表示を決めて、テキスト書き換えて、イベント追加する←このイベントは事前に登録でよくね？
            GameObject[] selectButtons = new GameObject[MAXSELECTNUM];
            for (int i = 0; i < selectButtons.Length; i++)
            {
                selectButtons[i] = settingObjects[(int)TutorialObjectNums.SJS].transform.Find($"SelectButton{i}").gameObject;
                if (pageNum * MAXSELECTNUM + i >= sjsDataLength.Count)
                {
                    selectButtons[i].SetActive(false);
                }
                else
                {
                    selectButtons[i].SetActive(true);
                    selectButtons[i].transform.GetChild(0).GetComponent<Text>().text = sjsDataLength[pageNum * MAXSELECTNUM + i];
                }
            }
        }
        public void NextPageButton()
        {
            pageNum++;
            ButtonData();
        }
        public void BackPageButton()
        {
            pageNum--;
            ButtonData();
        }
        public void SetSJSButtonEvent()
        {
            string word = eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;

            int sjsDataLength = 0;
            if (tutorialState == TutorialSettingStateEnums.Specoes)
            {
                sjsDataLength = access_DataBank.specoesData_.Length;
                for (int i = 0; i < sjsDataLength; i++)
                {
                    if (word == access_DataBank.specoesData_[i].specoesName)
                    {
                        specoesId = access_DataBank.specoesData_[i].specoesId;
                    }
                }
            }
            else if (tutorialState == TutorialSettingStateEnums.Job)
            {
                sjsDataLength = access_DataBank.jobData_.Length;
                for (int i = 0; i < sjsDataLength; i++)
                {
                    if (word == access_DataBank.jobData_[i].jobName)
                    {
                        jobId = access_DataBank.jobData_[i].jobId;
                    }
                }
            }
            else if (tutorialState == TutorialSettingStateEnums.Skill)
            {
                sjsDataLength = access_DataBank.skillData_.Length;
                for (int i = 0; i < sjsDataLength; i++)
                {
                    if (word == access_DataBank.skillData_[i].skillName)
                    {
                        skillId[0] = access_DataBank.skillData_[i].skillNumber;
                        skillId[1] = access_DataBank.skillData_[i].skillRarity;
                        skillId[2] = access_DataBank.skillData_[i].skillId;
                    }
                }
            }

            CheckTextUpdata(word);
        }
        #endregion

        #region SelectEnter
        void CheckTextUpdata(string setWord = "")
        {
            settingObjects[(int)TutorialObjectNums.SelectEnter].SetActive(true);
            string word = "";
            switch (tutorialState)
            {
                case TutorialSettingStateEnums.PlayerName:
                    word = $"プレイヤー名\n{playerName}\nでよろしいですか？";
                    break;
                case TutorialSettingStateEnums.Specoes:
                    word = $"種族:{setWord}\nでよろしいですか？";
                    break;
                case TutorialSettingStateEnums.Job:
                    word = $"職業:{setWord}\nでよろしいですか？";
                    break;
                case TutorialSettingStateEnums.Status:
                    break;
                case TutorialSettingStateEnums.Skill:
                    word = $"スキル:{setWord}\nでよろしいですか？";
                    break;
            }
            settingTextObjects[(int)TutorialTextObjectNums.SelectCheckEnter].text = word;
        }
        private void ObjectActiveDirector()
        {
            settingObjects[(int)TutorialObjectNums.SelectEnter].SetActive(false);
            switch (tutorialState)
            {
                case TutorialSettingStateEnums.PlayerName:
                    ResetObject();
                    settingObjects[(int)TutorialObjectNums.PlayerName].SetActive(true);
                    settingTextObjects[(int)TutorialTextObjectNums.PlayerNameRead].text = "プレイヤー名を入力してください\n(10文字以下)";
                    pageNum = 0;
                    statusPoint = 10;
                    specoesId = 0;
                    jobId = 0;
                    for (int i = 0; i < skillId.Length; i++)
                    {
                        skillId[i] = 0;
                    }
                    for (int i = 0; i < status.Length; i++)
                    {
                        status[i] = 0;
                    }
                    break;
                case TutorialSettingStateEnums.Specoes:
                    settingObjects[(int)TutorialObjectNums.SpecoesButton].GetComponent<Button>().interactable = true;
                    settingObjects[(int)TutorialObjectNums.PlayerName].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.SJS].SetActive(true);
                    pageNum = 0;
                    ButtonData();
                    break;
                case TutorialSettingStateEnums.Job:
                    settingObjects[(int)TutorialObjectNums.JobButton].GetComponent<Button>().interactable = true;
                    settingObjects[(int)TutorialObjectNums.SJS].SetActive(true);
                    pageNum = 0;
                    ButtonData();
                    break;
                case TutorialSettingStateEnums.Status:
                    settingObjects[(int)TutorialObjectNums.StatusButton].GetComponent<Button>().interactable = true;
                    settingObjects[(int)TutorialObjectNums.SJS].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.Status].SetActive(true);
                    for (int i = 0; i < 6; i++)
                    {
                        settingTextObjects[(int)TutorialTextObjectNums.STRText + i].text = status[i].ToString();
                    }
                    settingTextObjects[(int)TutorialTextObjectNums.PointText].text = statusPoint.ToString();
                    if (statusPoint != 0)
                    {
                        settingObjects[(int)TutorialObjectNums.Status].transform.Find("EndButton").gameObject.SetActive(false);
                    }
                    break;
                case TutorialSettingStateEnums.Skill:
                    settingObjects[(int)TutorialObjectNums.SkillButton].GetComponent<Button>().interactable = true;
                    settingObjects[(int)TutorialObjectNums.Status].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.SJS].SetActive(true);
                    pageNum = 0;
                    ButtonData();
                    break;
                case TutorialSettingStateEnums.lastCheck:
                    settingObjects[(int)TutorialObjectNums.SJS].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.lastCheck].SetActive(true);
                    string[] statusWord = new string[4];
                    for (int i = 0; i < access_DataBank.specoesData_.Length; i++)
                    {
                        if (specoesId == access_DataBank.specoesData_[i].specoesId)
                        {
                            statusWord[0] = access_DataBank.specoesData_[i].specoesName;
                            break;
                        }
                    }
                    for (int i = 0; i < access_DataBank.jobData_.Length; i++)
                    {
                        if (jobId == access_DataBank.jobData_[i].jobId)
                        {
                            statusWord[1] = access_DataBank.jobData_[i].jobName;
                            break;
                        }
                    }
                    for (int i = 0; i < access_DataBank.skillData_.Length; i++)
                    {
                        if (skillId[0] == access_DataBank.skillData_[i].skillNumber)
                        {
                            if (skillId[1] == access_DataBank.skillData_[i].skillRarity)
                            {
                                if (skillId[2] == access_DataBank.skillData_[i].skillId)
                                {
                                    statusWord[2] = access_DataBank.skillData_[i].skillName;
                                    break;
                                }
                            }
                        }
                    }

                    string lastStatusText = $"プレイヤー名：{playerName}　種族：{statusWord[0]}　職業：{statusWord[1]}";

                    for (int i = 0; i < lastStatusText.Length; i++)
                    {
                        statusWord[3] += "---";
                    }
                    lastStatusText += $"\n{statusWord[3]}\nSTR：{status[0]}　VIT：{status[1]}　INT：{status[2]}　DEX：{status[3]}　POW：{status[4]}　LUC：{status[5]}\nスキル\n{statusWord[2]}";

                    settingTextObjects[(int)TutorialTextObjectNums.LastStatusText].text = lastStatusText;
                    break;
                case TutorialSettingStateEnums.Loading:
                    settingObjects[(int)TutorialObjectNums.lastCheck].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.Step].SetActive(false);
                    settingObjects[(int)TutorialObjectNums.Loading].SetActive(true);
                    StartCoroutine(CreateMyData());
                    break;
                case TutorialSettingStateEnums.last:
                    settingObjects[(int)TutorialObjectNums.Loading].SetActive(false);
                    SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
                    sceneChangeSystem.SceneChange(SceneNames.Home);
                    Debug.Log("次のシーンへ行きます");
                    break;
            }
        }
        public void CheckClear()
        {
            tutorialState++;
            ObjectActiveDirector();
        }
        public void CheckOut()
        {
            settingObjects[(int)TutorialObjectNums.SelectEnter].SetActive(false);
        }
        #endregion

        #region Status
        public void StatusUpButton()
        {
            int statusNumber = eventSystem.currentSelectedGameObject.GetComponent<IStatusSetthingButtonMark>().MyNumber();
            if (statusPoint > 0)
            {
                statusPoint--;
                status[statusNumber]++;
                settingTextObjects[(int)TutorialTextObjectNums.STRText + statusNumber].text = status[statusNumber].ToString();
                settingTextObjects[(int)TutorialTextObjectNums.PointText].text = statusPoint.ToString();
                if (statusPoint == 0)
                {
                    settingObjects[(int)TutorialObjectNums.Status].transform.Find("EndButton").gameObject.SetActive(true);
                }
                else
                {
                    settingObjects[(int)TutorialObjectNums.Status].transform.Find("EndButton").gameObject.SetActive(false);
                }
            }
        }
        public void StatusDownButton()
        {
            int statusNumber = eventSystem.currentSelectedGameObject.GetComponent<IStatusSetthingButtonMark>().MyNumber();
            if (status[statusNumber] != 0)
            {
                statusPoint++;
                status[statusNumber]--;
                settingTextObjects[(int)TutorialTextObjectNums.STRText + statusNumber].text = status[statusNumber].ToString();
                settingTextObjects[(int)TutorialTextObjectNums.PointText].text = statusPoint.ToString();
                if (statusPoint == 0)
                {
                    settingObjects[(int)TutorialObjectNums.Status].transform.Find("EndButton").gameObject.SetActive(true);
                }
                else
                {
                    settingObjects[(int)TutorialObjectNums.Status].transform.Find("EndButton").gameObject.SetActive(false);
                }
            }
        }
        public void StatusEndButton()
        {
            CheckClear();
        }
        #endregion

        #region lastCheck
        public void LastCheckClear()
        {
            CheckClear();
        }
        public void LastCheckOut()
        {
            tutorialState = TutorialSettingStateEnums.PlayerName;
            ObjectActiveDirector();
        }
        #endregion

        IEnumerator CreateMyData()
        {
            // プレイデータを作成
            PlayData_Private_Structure playData_ = StackPlayData.Instance.GetPlayerData_PlayData();
            playData_.playerName = playerName;
            playData_.lastMapNumber = 0;
            playData_.myMoney = 0;

            // ステータスデータを作成
            StatusData_Private_Structure statusData_Private_ = StackPlayData.Instance.GetPlayerData_StatusData();
            statusData_Private_.lv = 1;
            statusData_Private_.exp = 0;
            statusData_Private_.gender = 0;
            statusData_Private_.p_str = status[0];
            statusData_Private_.p_vit = status[1];
            statusData_Private_.p_edu = status[2];
            statusData_Private_.p_dex = status[3];
            statusData_Private_.p_pow = status[4];
            statusData_Private_.p_luc = status[5];

            statusData_Private_.e_hp = statusData_Private_.p_str + statusData_Private_.p_vit * 2;
            statusData_Private_.e_mp = statusData_Private_.p_edu;
            statusData_Private_.e_yp = statusData_Private_.p_luc;
            statusData_Private_.e_lp = statusData_Private_.p_luc;

            statusData_Private_.s_atk = statusData_Private_.p_str * DEFAULTBUFF;
            statusData_Private_.s_def = statusData_Private_.p_vit * DEFAULTBUFF;
            statusData_Private_.s_map = statusData_Private_.p_edu * DEFAULTBUFF;
            statusData_Private_.s_mdp = statusData_Private_.p_dex * DEFAULTBUFF;
            statusData_Private_.s_agi = (int)((float)(statusData_Private_.p_str + statusData_Private_.p_dex) * (float)DEFAULTBUFF * 0.5);
            statusData_Private_.s_Luc = statusData_Private_.p_luc + (statusData_Private_.p_pow + statusData_Private_.p_dex);

            statusData_Private_.s_dex = statusData_Private_.p_dex * DEFAULTBUFF;
            statusData_Private_.s_res = statusData_Private_.p_pow;
            statusData_Private_.s_app = (int)((statusData_Private_.p_str + statusData_Private_.p_edu + statusData_Private_.p_dex) * 0.5);

            // 種族データの作成と種族データからステータス作成
            SpeciesData_Private_Structure speciesData_Private_ = StackPlayData.Instance.GetPlayerData_SpeciesData();
            speciesData_Private_.speciesOpenFlgs[specoesId] = true;
            speciesData_Private_.speciesLvDatas[specoesId] = 1;
            statusData_Private_.siz = access_DataBank.specoesData_[specoesId].siz;
            statusData_Private_.species[0] = specoesId;
            statusData_Private_.species[1] = 1;
            statusData_Private_.e_hp += access_DataBank.specoesData_[specoesId].hp;
            statusData_Private_.e_mp += access_DataBank.specoesData_[specoesId].mp;
            statusData_Private_.s_atk += access_DataBank.specoesData_[specoesId].ap;
            statusData_Private_.s_def += access_DataBank.specoesData_[specoesId].dp;
            statusData_Private_.s_map += access_DataBank.specoesData_[specoesId].map;
            statusData_Private_.s_mdp += access_DataBank.specoesData_[specoesId].mdp;
            statusData_Private_.s_agi += access_DataBank.specoesData_[specoesId].sp;
            statusData_Private_.s_Luc += access_DataBank.specoesData_[specoesId].luc;
            statusData_Private_.s_dex += access_DataBank.specoesData_[specoesId].des;
            statusData_Private_.s_res += access_DataBank.specoesData_[specoesId].res;
            statusData_Private_.s_app += access_DataBank.specoesData_[specoesId].app;

            // 職業データの作成と職業データからステータス作成
            JobData_Private_Structure jobData_Private_ = StackPlayData.Instance.GetPlayerData_JobData();
            jobData_Private_.jobOpenFlgs[jobId] = true;
            jobData_Private_.jobLvDatas[jobId] = 1;
            statusData_Private_.job[0] = jobId;
            statusData_Private_.job[1] = 1;
            statusData_Private_.e_hp += access_DataBank.jobData_[jobId].hp;
            statusData_Private_.e_mp += access_DataBank.jobData_[jobId].mp;
            statusData_Private_.s_atk += access_DataBank.jobData_[jobId].ap;
            statusData_Private_.s_def += access_DataBank.jobData_[jobId].dp;
            statusData_Private_.s_map += access_DataBank.jobData_[jobId].map;
            statusData_Private_.s_mdp += access_DataBank.jobData_[jobId].mdp;
            statusData_Private_.s_agi += access_DataBank.jobData_[jobId].sp;
            statusData_Private_.s_Luc += access_DataBank.jobData_[jobId].luc;
            statusData_Private_.s_dex += access_DataBank.jobData_[jobId].des;
            statusData_Private_.s_res += access_DataBank.jobData_[jobId].res;
            statusData_Private_.s_app += access_DataBank.jobData_[jobId].app;

            // スキルデータ作成
            SkillData_Private_Structure skillData_Private_ = StackPlayData.Instance.GetPlayerData_SkillData();
            for (int i = 0; i < access_DataBank.skillData_.Length; i++)
            {
                if (skillId[0] == access_DataBank.skillData_[i].skillNumber)
                {
                    if (skillId[1] == access_DataBank.skillData_[i].skillRarity)
                    {
                        if (skillId[2] == access_DataBank.skillData_[i].skillId)
                        {
                            skillData_Private_.skillOpenFlgs[i] = true;
                            skillData_Private_.skillLvDatas[i] = 1;
                        }
                    }
                }
            }

            // プレイマスターデータ作成
            PlayDataFile_Master_Structure playDataFile_Master_ = new PlayDataFile_Master_Structure();
            playDataFile_Master_.nowDataFlg = true;
            playDataFile_Master_.playerName = playerName;
            StackPlayData.Instance.SetPlayDataMaster(playDataFile_Master_);
            StackPlayData.Instance.SetPlayData(StackPlayData.Instance.GetPlayerData_PlayData_Master(), StackPlayData.Instance.GetPlayerData_CollectionData(), playData_, statusData_Private_, speciesData_Private_, jobData_Private_, skillData_Private_, StackPlayData.Instance.GetPlayerData_TechniqueData(), StackPlayData.Instance.GetPlayerData_ItemData(), StackPlayData.Instance.GetPlayerData_MonsterData(), StackPlayData.Instance.GetPlayerData_CharacterData());

            // 作成したデータを元にセーブデータ作成
            FileDataAccess fileDataAccess = new FileDataAccess();
            string[] fileNames = StackPlayData.Instance.fileNames;
            string[] folderNames = StackPlayData.Instance.folderNames;
            string pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}";
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayDatas_Master], StackPlayData.Instance.GetPlayerData_PlayData_Master());
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.CollectionDatas_Master], StackPlayData.Instance.GetPlayerData_CollectionData());
            pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}{folderNames[StackPlayData.Instance.saveNumber]}";
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayData_Private], playData_);
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.StatusData], statusData_Private_);
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.SpeclesData], speciesData_Private_);
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.JobData], jobData_Private_);
            fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.SkillData], skillData_Private_);

            yield return new WaitForSeconds(5f);

            settingObjects[(int)TutorialObjectNums.LoadingButton].SetActive(true);
        }
        public void LoadingButton()
        {
            CheckClear();
        }

        public void StepButton()
        {
            int getNumber = int.Parse(eventSystem.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text) - 1;
            tutorialState = (TutorialSettingStateEnums)getNumber;
            ResetObject();
            ObjectActiveDirector();
        }
    }
}