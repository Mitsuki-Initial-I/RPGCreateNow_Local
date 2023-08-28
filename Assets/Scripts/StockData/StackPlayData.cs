using UnityEngine;
using RPGCreateNow_Local.UseCase;
using System;

namespace RPGCreateNow_Local.StockData
{
    public class StackPlayData : MonoBehaviour //, IStockData
    {
        #region シングルトン処理
        private static StackPlayData instance = null;
        public static StackPlayData Instance => instance
            ?? (instance = GameObject.Find("StackDataObject").GetComponent<StackPlayData>());
        private void Awake()
        {
            if (this != Instance)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        [SerializeField]
        public string[] folderNames;
        [SerializeField]
        public string[] fileNames;
        [SerializeField]
        public string[] resourcesDataFaileNames;

        public int saveNumber;
        PlayData_Master_Structure masterPlayData = new PlayData_Master_Structure();
        CollectionData_Master_Structure masterCollectionData = new CollectionData_Master_Structure();

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

        public void SetMapData(int getMapNumber)
        {
            playData.lastMapNumber = getMapNumber;
        }

        public void SetPlayDataMaster(PlayDataFile_Master_Structure getPlayDataFile_Master_)
        {
            masterPlayData.data[saveNumber-1] = getPlayDataFile_Master_;
        }
        public void SetPlayData(
            PlayData_Master_Structure getMasterPlayData,
            CollectionData_Master_Structure getMasterCollectionData,
            PlayData_Private_Structure getPlayData,
            StatusData_Private_Structure getStatusData,
            SpeciesData_Private_Structure getSpeciesData,
            JobData_Private_Structure getJobData,
            SkillData_Private_Structure getSkillData,
             TechniqueData_Private_Structure getTechniqueData,
             //MagicData_Private_Structure getMagicData,
             ItemData_Private_Structure getItemData,
             MonsterData_Private_Structure getKillMonsterData,
             CharacterData_Private_Structure getCharacterData
             //QuestData_Private_Structure getQuestData
            )
        {
            masterPlayData = getMasterPlayData;
            masterCollectionData = getMasterCollectionData;
            playData = getPlayData;
            statusData = getStatusData;
            speciesData = getSpeciesData;
            jobData = getJobData;
            skillData = getSkillData;
            techniqueData = getTechniqueData;
            //magicData = getMagicData;
            itemData = getItemData;
            killMonsterData = getKillMonsterData;
            characterData = getCharacterData;
            //questData = getQuestData;
        }

        public PlayData_Master_Structure GetPlayerData_PlayData_Master()
        {
            return masterPlayData;
        }
        public CollectionData_Master_Structure GetPlayerData_CollectionData()
        {
            return masterCollectionData;
        }        
        public PlayData_Private_Structure GetPlayerData_PlayData()
        {
            return playData;
        }
        public StatusData_Private_Structure GetPlayerData_StatusData()
        {
            return statusData;
        }
        public SpeciesData_Private_Structure GetPlayerData_SpeciesData()
        {
            return speciesData;
        }
        public JobData_Private_Structure GetPlayerData_JobData()
        {
            return jobData;
        }
        public SkillData_Private_Structure GetPlayerData_SkillData()
        {
            return skillData;
        }
        public TechniqueData_Private_Structure GetPlayerData_TechniqueData()
        {
            return techniqueData;
        }
        public ItemData_Private_Structure GetPlayerData_ItemData()
        {
            return itemData;
        }
        public MonsterData_Private_Structure GetPlayerData_MonsterData()
        {
            return killMonsterData;
        }
        public CharacterData_Private_Structure GetPlayerData_CharacterData()
        {
            return characterData;
        }

        /// <summary>
        /// 過去のスクリプト
        /// </summary>
        /*
















        PlayerStatus_Structure playerStatusData_Default;    // 素のステータス
        PlayerStatus_Structure playerStatusData;            // 装備やバフによる変化しているステータス
        EnemyStatus_Structure battleEnemyStatus;            // これから戦う敵のステータス

        Play_SearchAchievementRate_Structure play_SearchAchievementRateData;
        int mapNumber;      // 最後に行ったマップ
        int stageNumber;    // 最後に行ったステージ

        [SerializeField]
        //string[] fileNames;

        void IStockData.SetPlayerStatusData(PlayerStatus_Structure setData)
        {
            playerStatusData = setData;
        }
        PlayerStatus_Structure IStockData.GetPlayerStatusData()
        {
            return playerStatusData;
        }
        void IStockData.SetEnemyStatusData(EnemyStatus_Structure setData)
        {
            battleEnemyStatus = setData;
        }
        EnemyStatus_Structure IStockData.GetEnemyStatusData()
        {
            return battleEnemyStatus;
        }
        void IStockData.SetPlay_SearchAchievementRateData(Play_SearchAchievementRate_Structure setData)
        {
            play_SearchAchievementRateData = setData;
        }
        Play_SearchAchievementRate_Structure IStockData.GetPlay_SearchAchievementRateData()
        {
            return play_SearchAchievementRateData;
        }
        void IStockData.SetStageData(int setMapNumber, int setStageNumber)
        {
            mapNumber = setMapNumber;
            stageNumber = setStageNumber;
        }
        int[] IStockData.GetStageData()
        {
            int[] mapData = new int[2];
            mapData[0] = mapNumber;
            mapData[1] = stageNumber;
            return mapData;
        }
        string[] IStockData.GetFileName()
        {
            return fileNames;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log($"{playerStatusData.playerName},{playerStatusData.lv},{playerStatusData.hp},{playerStatusData.mp},{playerStatusData.ap},{playerStatusData.dp},{playerStatusData.map},{playerStatusData.mdp},{playerStatusData.sp},{playerStatusData.exp}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log($"{battleEnemyStatus.enemyName},{battleEnemyStatus.lv},{battleEnemyStatus.hp},{battleEnemyStatus.mp},{battleEnemyStatus.ap},{battleEnemyStatus.dp},{battleEnemyStatus.map},{battleEnemyStatus.mdp},{battleEnemyStatus.sp},{battleEnemyStatus.dropExp}");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                for (int i = 0; i < play_SearchAchievementRateData.play_SearchStages.Length; i++)
                {
                    if (play_SearchAchievementRateData.play_SearchStages[i].mapNumber==mapNumber&& play_SearchAchievementRateData.play_SearchStages[i].stageNumber == stageNumber)
                    {
                        Debug.Log((play_SearchAchievementRateData.play_SearchStages[i].clearFlag));
                    }
                }
            }
            if (Input.GetKey(KeyCode.Tab) && Input.GetKeyDown(KeyCode.Return))
            {
                playerStatusData.hp = 100;
                playerStatusData.mp = 100;
                playerStatusData.ap = 100;
                playerStatusData.dp = 100;
                playerStatusData.map = 100;
                playerStatusData.mdp = 100;
                playerStatusData.sp = 100;
            }
        }
        */
    }
}