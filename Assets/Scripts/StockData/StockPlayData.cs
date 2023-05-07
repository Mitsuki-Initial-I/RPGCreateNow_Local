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
        }
    }
}