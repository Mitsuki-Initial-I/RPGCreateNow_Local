using System.IO;
using UnityEngine;
using UnityEngine.UI;
using RPGCreateNow_Local.UseCase;
using RPGCreateNow_Local.Data;

namespace RPGCreateNow_Local.System
{
    public class MainLoadingSystem : MonoBehaviour
    {
        [SerializeField]
        GameObject playerNameObj1;
        [SerializeField]
        GameObject playerNameObj2;
        [SerializeField]
        Text playerNameText;
        [SerializeField]
        Text playerNameCheckText;

        PlayerStatus_Structure playerStatusData = new PlayerStatus_Structure();
        PlayLog_Structure playLogData = new PlayLog_Structure();
        Play_SearchAchievementRate_Structure play_SearchAchievementRateData = new Play_SearchAchievementRate_Structure();

        IStockData setPlayerData;

        SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
        PlayerStatusDataAccess playerStatusDataAccess = new PlayerStatusDataAccess();
        PlayLogDataAccess playLogDataAccess = new PlayLogDataAccess();
        Play_SearchAchievementRateDataAccess play_SearchAchievementRateDataAccess = new Play_SearchAchievementRateDataAccess();
        private void Start()
        {
            playerNameObj1.SetActive(false);
            playerNameObj2.SetActive(false);
            setPlayerData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            string pass = $"{Application.persistentDataPath}/Data";
            string[] fileNames = setPlayerData.GetFileName();
            string playerStatusPass = $"{Application.persistentDataPath}/Data{fileNames[0]}";
            playerStatusDataAccess.fileName = fileNames[0];
            playLogDataAccess.fileName = fileNames[1];
            play_SearchAchievementRateDataAccess.fileName = fileNames[2];

            if (!Directory.Exists(pass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playLogData = playLogDataAccess.FirstData();
                play_SearchAchievementRateData = play_SearchAchievementRateDataAccess.FirstData();
                playerNameObj1.SetActive(true);
            }
            else if (!File.Exists(playerStatusPass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
                playerNameObj1.SetActive(true);
            }
            else
            {
                pass = $"{Application.persistentDataPath}/Data{fileNames[1]}";
                if (!File.Exists(pass))
                {
                    playLogData = playLogDataAccess.FirstData();
                    playLogDataAccess.PlayLogDataSeva(playLogData);
                }
                pass = $"{Application.persistentDataPath}/Data{fileNames[2]}";
                if (!File.Exists(pass))
                {
                    play_SearchAchievementRateData = play_SearchAchievementRateDataAccess.FirstData();
                    play_SearchAchievementRateDataAccess.Play_SearchAchievementRateSave(play_SearchAchievementRateData);
                }
                playerStatusDataAccess.PlayerStatusDataLoad(out playerStatusData);
                playLogDataAccess.PlayLogDataLoad(out playLogData);
                play_SearchAchievementRateDataAccess.Play_SearchAchievementRateLoad(out play_SearchAchievementRateData);
                setPlayerData.SetPlayerStatusData(playerStatusData);
                setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
                sceneChangeSystem.SceneChange(SceneNameS.Home);
            }
        }

        public void NameCheck()
        {
            playerStatusData.playerName = playerNameText.text;
            playerNameObj1.SetActive(false);
            playerNameObj2.SetActive(true);
            playerNameCheckText.text = $"プレイヤー名\n{playerStatusData.playerName}\nでよろしいですか？";
        }
        public void NameOk()
        {
            playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
            playLogDataAccess.PlayLogDataSeva(playLogData);
            play_SearchAchievementRateDataAccess.Play_SearchAchievementRateSave(play_SearchAchievementRateData);
            
            setPlayerData.SetPlayerStatusData(playerStatusData);
            setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
            
            sceneChangeSystem.SceneChange(SceneNameS.Home);
        }
        public void NameOut()
        {
            playerStatusData.playerName = "";
            playerNameObj1.SetActive(true);
            playerNameObj2.SetActive(false);
        }
    }
}