using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.Data
{
    public class PlayerStatusDataAccess
    {
        public string fileName;// = "/playerStatusData.json";

        public PlayerStatus_Structure FirstData()
        {
            PlayerStatus_Structure newData = new PlayerStatus_Structure();
            newData.lv = 1;
            newData.hp = 10;
            newData.mp = 10;
            newData.ap = 10;
            newData.dp = 10;
            newData.map = 10;
            newData.mdp = 10;
            newData.sp = 10;
            newData.exp = 0;

            return newData;
        }

        public bool PlayerStatusDataSeva(PlayerStatus_Structure playerStatusData)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.SaveFileSystem(pass, fileName, playerStatusData);
        }
        public bool PlayerStatusDataLoad(out PlayerStatus_Structure playerStatusData)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.LoadFileSystem(pass, fileName, out playerStatusData);
        }
    }
}