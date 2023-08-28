using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.System
{
    public class FirstDataSettingSystem
    {
        public void FirstPlayData(out PlayData_Private_Structure playData)
        {
            playData = new PlayData_Private_Structure();
            playData.playerName = default;
        }
        public void FirstStatusData(out StatusData_Private_Structure statusData)
        {
            statusData = new StatusData_Private_Structure();
            statusData.lv = 1;
            statusData.exp = 0;
            statusData.siz = 10;
            statusData.gender = 10;
            statusData.e_hp = 10;
            statusData.e_mp = 10;
            statusData.e_yp = 10;
            statusData.e_lp = 10;
            statusData.s_atk = 10;
            statusData.s_def = 10;
            statusData.s_map = 10;
            statusData.s_mdp = 10;
            statusData.s_agi = 10;
            statusData.s_Luc = 10;
            statusData.s_dex = 10;
            statusData.s_res = 10;
            statusData.s_app = 10;
            statusData.p_str = 10;
            statusData.p_vit = 10;
            statusData.p_edu = 10;
            statusData.p_dex = 10;
            statusData.p_pow = 10;
        }
    }
}