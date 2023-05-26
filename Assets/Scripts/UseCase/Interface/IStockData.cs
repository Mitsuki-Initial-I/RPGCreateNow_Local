namespace RPGCreateNow_Local.UseCase
{
    public interface IStockData
    {
        public void SetPlayerStatusData(PlayerStatus_Structure setData);
        public PlayerStatus_Structure GetPlayerStatusData();
        public void SetEnemyStatusData(EnemyStatus_Structure setData);
        public EnemyStatus_Structure GetEnemyStatusData();
        public void SetPlay_SearchAchievementRateData(Play_SearchAchievementRate_Structure setData);
        public Play_SearchAchievementRate_Structure GetPlay_SearchAchievementRateData();
        public void SetStageData(int setMapNumber, int setStageNumber);
        public int[] GetStageData();
        public string[] GetFileName();
    }
}