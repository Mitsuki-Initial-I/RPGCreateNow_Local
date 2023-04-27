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

        ISetPlayerData setPlayerData;

        SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();
        PlayerStatusDataAccess playerStatusDataAccess = new PlayerStatusDataAccess();
        PlayLogDataAccess playLogDataAccess = new PlayLogDataAccess();
        private void Start()
        {
            playerNameObj1.SetActive(false);
            playerNameObj2.SetActive(false);
            setPlayerData = GameObject.Find("StockPlayerData").GetComponent<ISetPlayerData>();
            string pass = $"{Application.persistentDataPath}/Data";
           
            if (!Directory.Exists(pass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playLogData = playLogDataAccess.FirstData();
                playerNameObj1.SetActive(true);
            }
            else
            {
                playerStatusDataAccess.PlayerStatusDataLoad(out playerStatusData);
                playLogDataAccess.PlayLogDataLoad(out playLogData);
                setPlayerData.SetPlayerStatusData(playerStatusData);
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
            setPlayerData.SetPlayerStatusData(playerStatusData);
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