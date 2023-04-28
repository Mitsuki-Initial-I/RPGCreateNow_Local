namespace RPGCreateNow_Local.UseCase
{
    public interface IStockData
    {
        public void SetPlayerStatusData(PlayerStatus_Structure setData);
        public PlayerStatus_Structure GetPlayerStatusData();
    }
}