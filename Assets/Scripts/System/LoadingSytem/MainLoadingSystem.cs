using System.IO;
using UnityEngine;
using UnityEngine.UI;
using RPGCreateNow_Local.UseCase;

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

        PlayerStatus_Structure newData = new PlayerStatus_Structure();
        FileAccessSystem fileAccessSystem = new FileAccessSystem();
        ISetPlayerData setPlayerData;
        SceneChangeSystem sceneChangeSystem = new SceneChangeSystem();

        private void Start()
        {
            playerNameObj1.SetActive(false);
            playerNameObj2.SetActive(false);
            setPlayerData = GameObject.Find("StockPlayerData").GetComponent<ISetPlayerData>();
            FileAccessSystem fileAccessSystem = new FileAccessSystem();
            string pass = $"{Application.persistentDataPath}/Data";
            string fileNamePass = "/player.json";
            string fileName = pass + fileNamePass;
           
            if (!Directory.Exists(pass)|| !File.Exists(fileName))
            {
                newData.lv = 1;
                newData.hp = 10;
                newData.mp = 10;
                newData.ap = 10;
                newData.dp = 10;
                newData.map = 10;
                newData.mdp = 10;
                newData.sp = 10;
                newData.exp = 0;
                playerNameObj1.SetActive(true);
            }
            else
            {
                fileAccessSystem.LoadFileSystem(pass, fileNamePass, out newData);
                setPlayerData.SetPlayerStatusData(newData);
                Debug.Log(newData.playerName);
                sceneChangeSystem.SceneChange(SceneNameS.Map);
            }
        }

        public void NameCheck()
        {
            newData.playerName = playerNameText.text;
            playerNameObj1.SetActive(false);
            playerNameObj2.SetActive(true);
            playerNameCheckText.text = $"プレイヤー名\n{newData.playerName}\nでよろしいですか？";
        }
        public void NameOk()
        {
            string pass = $"{Application.persistentDataPath}/Data";
            string fileNamePass = "/player.json";
            fileAccessSystem.SaveFileSystem(pass, fileNamePass, newData);
            setPlayerData.SetPlayerStatusData(newData);
            sceneChangeSystem.SceneChange(SceneNameS.Map);
        }
        public void NameOut()
        { 
            newData.playerName = "";
            playerNameObj1.SetActive(true);
            playerNameObj2.SetActive(false);
        }
    }
}