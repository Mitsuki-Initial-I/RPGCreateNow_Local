namespace RPGCreateNow_Local.UseCase
{
    [System.Serializable]
    public struct StatusData_Private_Structure
    {
        public int lv;
        public int exp;
        public int siz;
        public int gender;
        public int[] species;
        public string speciesName;
        public int[] job;
        public string jobName;

        public int e_hp;
        public int e_mp;
        public int e_yp;
        public int e_lp;

        public int s_atk;
        public int s_def;
        public int s_map;
        public int s_mdp;
        public int s_agi;
        public int s_Luc;

        public int s_dex;
        public int s_res;
        public int s_app;

        public int p_str;
        public int p_vit;
        public int p_edu;
        public int p_dex;
        public int p_pow;
        public int p_luc;
    }
}