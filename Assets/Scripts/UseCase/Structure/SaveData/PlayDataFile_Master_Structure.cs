namespace RPGCreateNow_Local.UseCase
{
    [System.Serializable]
    public struct PlayDataFile_Master_Structure 
    {
        public string playerName;
        public int playTime_hours;
        public int playTime_minutes;
        public int lastMapNumber;
        public int lastStageNumber;
        public bool nowDataFlg;
    }
    [System.Serializable]
    public struct PlayData_Master_Structure
    {
        public PlayDataFile_Master_Structure[] data;
    }
}