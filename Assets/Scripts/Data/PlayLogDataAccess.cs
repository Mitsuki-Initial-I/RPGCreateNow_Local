using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;

namespace RPGCreateNow_Local.Data
{
    public class PlayLogDataAccess
    {
        string fileName = "/playLogData.json";

        public PlayLog_Structure FirstData()
        {
            PlayLog_Structure newData = new PlayLog_Structure();
            newData.playerId = 0;
            newData.mainScenarioProcessLv = 0;
            newData.playTime = 0;
            return newData;
        }

        public bool PlayLogDataSeva(PlayLog_Structure playLog)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.SaveFileSystem(pass, fileName, playLog);
        }
        public bool PlayLogDataLoad(out PlayLog_Structure playLog)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.LoadFileSystem(pass, fileName, out playLog);
        }
    }
}
