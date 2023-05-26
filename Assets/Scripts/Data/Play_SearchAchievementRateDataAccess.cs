using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCreateNow_Local.UseCase;
using System;

namespace RPGCreateNow_Local.Data
{
    public class Play_SearchAchievementRateDataAccess
    {
        public string fileName; //= "/play_SearchAchievementRateData.json";
        public Play_SearchAchievementRate_Structure FirstData()
        {
            Play_SearchAchievementRate_Structure newData = new Play_SearchAchievementRate_Structure();

            int[] mapNums = new int[Enum.GetValues(typeof(SearchAreaNames)).Length];
            mapNums[(int)SearchAreaNames.meadow] = 5;
            mapNums[(int)SearchAreaNames.forest] = 5;
            mapNums[(int)SearchAreaNames.volcano] = 5;
            mapNums[(int)SearchAreaNames.Ocean] = 5;
            mapNums[(int)SearchAreaNames.cave] = 5;
            int count = 0;
            for (int i = 0; i < mapNums.Length; i++)
            {
                count += mapNums[i];
            }
            newData.play_SearchStages = new Play_SearchStage_Structure[count];
            count = 0;
            for (int i = 0; i < mapNums.Length; i++)
            {
                for (int j = 0; j < mapNums[i]; j++)
                {
                    Play_SearchStage_Structure play_Search = new Play_SearchStage_Structure();
                    play_Search.mapNumber = i;
                    play_Search.stageNumber = j;
                    newData.play_SearchStages[count] = play_Search;
                    count++;
                }
            }
            return newData;
        }
        public bool Play_SearchAchievementRateSave(Play_SearchAchievementRate_Structure playerSearchAchievementRateData)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.SaveFileSystem(pass, fileName, playerSearchAchievementRateData);
        }
        public bool Play_SearchAchievementRateLoad(out Play_SearchAchievementRate_Structure playerSearchAchievementRateData)
        {
            string pass = $"{Application.persistentDataPath}/Data";
            FileDataAccess fileDataAccess = new FileDataAccess();
            return fileDataAccess.LoadFileSystem(pass, fileName, out playerSearchAchievementRateData);
        }
    }
}