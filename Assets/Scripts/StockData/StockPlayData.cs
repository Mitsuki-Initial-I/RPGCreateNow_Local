using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.StockData
{
    public class StockPlayData : MonoBehaviour, IStockData
    {
        private static StockPlayData instance = null;

        public static StockPlayData Instance => instance
            ?? (instance = GameObject.Find("StockPlayerData").GetComponent<StockPlayData>());

        PlayerStatus_Structure playerStatusData_Default;    // 素のステータス
        PlayerStatus_Structure playerStatusData;            // 装備やバフによる変化しているステータス
        EnemyStatus_Structure battleEnemyStatus;            // これから戦う敵のステータス

        Play_SearchAchievementRate_Structure play_SearchAchievementRateData;
        int mapNumber;      // 最後に行ったマップ
        int stageNumber;    // 最後に行ったステージ

        [SerializeField]
        string[] fileNames;

        private void Awake()
        {
            if (this != Instance)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
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
    }
}