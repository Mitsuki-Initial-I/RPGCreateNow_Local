namespace RPGCreateNow_Local.UseCase
{
    public interface IStockData
    {
        public void SetPlayerStatusData(PlayerStatus_Structure setData);
        public PlayerStatus_Structure GetPlayerStatusData();
        public void SetEnemyStatusData(EnemyStatus_Structure setData);
        public EnemyStatus_Structure GetEnemyStatusData();
    }
}