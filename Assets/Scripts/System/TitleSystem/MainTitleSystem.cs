using System.IO;
using System.Collections;
using UnityEngine;
using RPGCreateNow_Local.UseCase;
using RPGCreateNow_Local.Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RPGCreateNow_Local.StockData;

/// TODO: �v���C�f�[�^�Ȃǂ̃f�[�^�̍\�������܂��Ă��Ȃ��̂ŕύX�����\��
/// HACK: �s���������A���Â炢�̂ł��̕ӂ�C��

namespace RPGCreateNow_Local.System
{
    public class MainTitleSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject TextBox;             // �^�C�g�����Ȃǂ̃e�L�X�g���������I�u�W�F�N�g
        [SerializeField]
        GameObject ButtonBox;           // �I�����̃{�^�����������I�u�W�F�N�g
        [SerializeField]
        GameObject SettingObject;       // �ݒ��ʂ̃I�u�W�F�N�g
        [SerializeField]
        GameObject SaveDataBoxObject;   // �Z�[�u�f�[�^�I����ʗp

        string[] folderNames;
        string[] fileNames;
        string[] resourcesDataFaileNames;

        bool checkFlg = false;
        bool newGameFlg = false;

        // �V�[���ύX�V�X�e��
        SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();

        // �Z�[�u�f�[�^�֘A�\����
        // �}�X�^�[
        PlayData_Master_Structure masterPlayData = new PlayData_Master_Structure();
        CollectionData_Master_Structure masterCollectionData = new CollectionData_Master_Structure();
        // ��
        PlayData_Private_Structure playData = new PlayData_Private_Structure();
        StatusData_Private_Structure statusData = new StatusData_Private_Structure();
        SpeciesData_Private_Structure speciesData = new SpeciesData_Private_Structure();
        JobData_Private_Structure jobData = new JobData_Private_Structure();
        SkillData_Private_Structure skillData = new SkillData_Private_Structure();
        TechniqueData_Private_Structure techniqueData = new TechniqueData_Private_Structure();
        //MagicData_Private_Structure magicData = new MagicData_Private_Structure();
        ItemData_Private_Structure itemData = new ItemData_Private_Structure();
        MonsterData_Private_Structure killMonsterData = new MonsterData_Private_Structure();
        CharacterData_Private_Structure characterData = new CharacterData_Private_Structure();
        //QuestData_Private_Structure questData = new QuestData_Private_Structure();

        // �Z�[�u�f�[�^�֘A�V�X�e��
        Access_DataBank access_DataBank = new Access_DataBank();
        FileDataAccess fileDataAccess = new FileDataAccess();

        IEnumerator CheckMasterData()
        {
            string pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}";
            if (!Directory.Exists(pass))
            {
                // �}�X�^�[�f�[�^����
                masterPlayData.data = new PlayDataFile_Master_Structure[3];
                for (int i = 0; i < masterPlayData.data.Length; i++)
                {
                    masterPlayData.data[i].playerName = "�f�[�^������܂���";
                }
                // ���\�[�X�f�[�^��ǂݍ���
                access_DataBank.Load_Resources(resourcesDataFaileNames);

                // �f�[�^�����ɔz����쐬
                //�}�X�^
                masterCollectionData.speciesOpuntLvs = new int[access_DataBank.specoesData_.Length];
                masterCollectionData.jobOpuntLvs = new int[access_DataBank.jobData_.Length];
                masterCollectionData.skillOpuntLvs = new int[access_DataBank.skillData_.Length];
                masterCollectionData.TechniqueOpuntLvs = new int[access_DataBank.techniqueData_.Length];
                masterCollectionData.ItemOpuntLvs = new int[access_DataBank.itemData_.Length];
                masterCollectionData.MonsterOpuntLvs = new int[access_DataBank.monsterData_.Length];
                masterCollectionData.CharacterOpuntLvs = new int[access_DataBank.character_.Length];

                // ��
                speciesData.speciesLvDatas = new int[access_DataBank.specoesData_.Length];
                speciesData.speciesOpenFlgs = new bool[access_DataBank.specoesData_.Length];
                jobData.jobLvDatas = new int[access_DataBank.jobData_.Length];
                jobData.jobOpenFlgs = new bool[access_DataBank.jobData_.Length];
                skillData.skillLvDatas = new int[access_DataBank.skillData_.Length];
                skillData.skillOpenFlgs = new bool[access_DataBank.skillData_.Length];
                techniqueData.techniqueLvDatas = new int[access_DataBank.techniqueData_.Length];
                techniqueData.techniqueOpenFlgs = new bool[access_DataBank.techniqueData_.Length];
                itemData.itemDatas = new int[access_DataBank.itemData_.Length];
                killMonsterData.monsterKillDatas = new int[access_DataBank.monsterData_.Length];
                killMonsterData.monsterOpenFlgs = new bool[access_DataBank.monsterData_.Length];
                characterData.characterFavorabilityRatingDatas = new int[access_DataBank.character_.Length];
                characterData.characterOpenFlgs = new bool[access_DataBank.character_.Length];

                FirstDataSettingSystem firstDataSettingSystem = new FirstDataSettingSystem();
                firstDataSettingSystem.FirstPlayData(out playData);
                firstDataSettingSystem.FirstStatusData(out statusData);
                statusData.species = new int[2] { 0, 0 };
                statusData.job = new int[2] { 0, 0 };

                yield return new WaitForSeconds(1f);

                // �Z�[�u
                fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayDatas_Master], masterPlayData);
                fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.CollectionDatas_Master], masterCollectionData);

                for (int i = 0; i < 3; i++)
                {
                    if (i == (int)SaveData_FolderNames.PrivateSaveData_01 - 1)
                    {
                        pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}{folderNames[(int)SaveData_FolderNames.PrivateSaveData_01]}";
                    }
                    else if (i == (int)SaveData_FolderNames.PrivateSaveData_02 - 1)
                    {
                        pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}{folderNames[(int)SaveData_FolderNames.PrivateSaveData_02]}";
                    }
                    else if (i == (int)SaveData_FolderNames.PrivateSaveData_03 - 1)
                    {
                        pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}{folderNames[(int)SaveData_FolderNames.PrivateSaveData_03]}";
                    }

                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayData_Private], playData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.StatusData], statusData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.SpeclesData], speciesData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.JobData], jobData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.SkillData], skillData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.TechniqueData], techniqueData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.ItemData], itemData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.MonsterData], killMonsterData);
                    fileDataAccess.SaveFileSystem(pass, fileNames[(int)SaveData_FileNames.CharacterData], characterData);
                }
            }
            else
            {
                // ���[�h�̓}�X�^�[�̂�
                fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayDatas_Master], out masterPlayData);
                fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.CollectionDatas_Master], out masterCollectionData);
                yield break;
            }
        }
        IEnumerator Start()
        {
            // ���߂͔�\��
            TextBox.SetActive(false);
            ButtonBox.SetActive(false);
            SettingObject.SetActive(false);
            SaveDataBoxObject.SetActive(false);

            folderNames = StackPlayData.Instance.folderNames;
            fileNames = StackPlayData.Instance.fileNames;
            resourcesDataFaileNames = StackPlayData.Instance.resourcesDataFaileNames;

            // �����ł̓}�X�^�[�f�[�^�̊m�F�̂ݍs��
            var checkData = CheckMasterData();

            yield return checkData;

            // �f�[�^�̗p�ӂ��I��莟��e�L�X�g�\��
            TextBox.SetActive(true);
        }
        private void Update()
        {
            // �N���b�N���ꂽ��{�^���\��
            if (Input.GetMouseButtonDown(0) && !checkFlg)
            {
                checkFlg = true; 
                BackButton();
            }
        }
       �@private void LoadPrivateData()
        {
            // �Z�[�u�f�[�^�\��
            for (int i = 0; i < (int)SaveData_FolderNames.PrivateSaveData_03; i++)
            {
                SaveDataBoxObject.transform.Find($"SaveData{i + 1}").gameObject.transform.Find("FileNumberText").gameObject.GetComponent<Text>().text = $"�t�@�C��{i + 1}";
                SaveDataBoxObject.transform.Find($"SaveData{i + 1}").gameObject.transform.Find("PlayerNameText").gameObject.GetComponent<Text>().text = masterPlayData.data[i].playerName;
                SaveDataBoxObject.transform.Find($"SaveData{i + 1}").gameObject.transform.Find("PlayTimeText").gameObject.GetComponent<Text>().text = $"{masterPlayData.data[i].playTime_hours.ToString().PadLeft(2,'0')}:{masterPlayData.data[i].playTime_minutes.ToString().PadLeft(2, '0')}";
                SaveDataBoxObject.transform.Find($"SaveData{i + 1}").gameObject.transform.Find("ProgressionText").gameObject.GetComponent<Text>().text = $"{masterPlayData.data[i].lastMapNumber}-{masterPlayData.data[i].lastStageNumber}";
            }
        }

        // �{�^���N���b�N���ɌĂ΂�郁�\�b�h
        public void NewGameButton()
        {
            // NewGame�̃t���O�𗧂ĂāA�Z�[�u�f�[�^��\��
            newGameFlg = true;
            SaveDataBoxObject.SetActive(true);
            SaveDataBoxObject.transform.Find("ReadText").GetComponent<Text>().text = "�ǂ̃Z�[�u�f�[�^�Ŏn�߂܂����H\n(�����̃f�[�^�̏ꍇ�㏑������܂�)";
            LoadPrivateData();
        }
        public void LoadGameButton()
        {
            // NewGame�̃t���O�𗧂������A�Z�[�u�f�[�^��\��
            newGameFlg = false;
            SaveDataBoxObject.SetActive(true);
            SaveDataBoxObject.transform.Find("ReadText").GetComponent<Text>().text = "�ǂ̃Z�[�u�f�[�^�����[�h���܂����H";
            LoadPrivateData();
        }
        public void SettingButton()
        {
            // �ݒ��ʕ\��
            SettingObject.SetActive(true);
        }
        public void CollectionButton()
        {
            // �R���N�V������ʕ\��
            sceneChangeSystem.SceneChange(UseCase.SceneNames.Collection);
        }
        public void BackButton()
        {
            // �{�^���I����\���A����ȊO��\��
            TextBox.SetActive(false);
            ButtonBox.SetActive(true);
            SettingObject.SetActive(false);
            SaveDataBoxObject.SetActive(false);
        }
        public void StartGame()
        {
            // Click�����ԍ���F��
            EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

            string saveFileName = eventSystem.currentSelectedGameObject.name;
            int saveFileNumber = int.Parse(saveFileName.Substring(saveFileName.Length - 1));
            string pass = $"{Application.persistentDataPath}/{folderNames[(int)SaveData_FolderNames.SaveData]}{folderNames[saveFileNumber]}";
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.PlayData_Private],out playData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.StatusData], out statusData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.SpeclesData], out speciesData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.JobData], out jobData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.SkillData], out skillData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.TechniqueData], out techniqueData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.ItemData], out itemData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.MonsterData], out killMonsterData);
            fileDataAccess.LoadFileSystem(pass, fileNames[(int)SaveData_FileNames.CharacterData], out characterData);
            StackPlayData.Instance.SetPlayData(masterPlayData, masterCollectionData, playData, statusData, speciesData, jobData, skillData, techniqueData, itemData, killMonsterData, characterData);
            StackPlayData.Instance.saveNumber = saveFileNumber;

            if (newGameFlg)
            {
                sceneChangeSystem.SceneChange(SceneNames.Tutorial);
            }
            else�@if(!newGameFlg && masterPlayData.data[saveFileNumber-1].nowDataFlg)
            {
                sceneChangeSystem.SceneChange(SceneNames.Home);
            }
            else if (!newGameFlg && !masterPlayData.data[saveFileNumber - 1].nowDataFlg)
            {
                SaveDataBoxObject.transform.Find("ReadText").GetComponent<Text>().text = "�f�[�^������܂���";
            }
        }
    }
}