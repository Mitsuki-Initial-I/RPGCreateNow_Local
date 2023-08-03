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
        GameObject playerName_InputObj;
        [SerializeField]
        GameObject playerName_CheckObj;
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
            // 一度、全ての画面を非表示にする
            // スタックデータをヒエラルキー上から取得する
            // フォルダーのパスとファイル名を宣言取得、一番始めのパスも専用に変数を用意
            // それぞれのアクセス先のファイル名を設定する
            playerName_InputObj.SetActive(false);
            playerName_CheckObj.SetActive(false);
            setPlayerData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            string pass = $"{Application.persistentDataPath}/Data";
            string[] fileNames = setPlayerData.GetFileName();
            string playerStatusPass = $"{Application.persistentDataPath}/Data{fileNames[0]}";
            playerStatusDataAccess.fileName = fileNames[0];
            playLogDataAccess.fileName = fileNames[1];
            play_SearchAchievementRateDataAccess.fileName = fileNames[2];

            // フォルダの存在を確認
            // なければ、全てのデータを用意し、プレイヤ名入力画面へ移動する
            if (!Directory.Exists(pass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playLogData = playLogDataAccess.FirstData();
                play_SearchAchievementRateData = play_SearchAchievementRateDataAccess.FirstData();
                playerName_InputObj.SetActive(true);
            }
            // ファイルを確認
            // なければ、初期データを用意し、プレイヤ名入力画面へ移動する
            else if (!File.Exists(playerStatusPass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
                playerName_InputObj.SetActive(true);
            }
            else
            {
                // パスを更新しながらファイルを確認
                // 無ければ作成する
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
                //全てのデータを読み込み、スタックデータを書き換え、シーンを切り替える
                playerStatusDataAccess.PlayerStatusDataLoad(out playerStatusData);
                playLogDataAccess.PlayLogDataLoad(out playLogData);
                play_SearchAchievementRateDataAccess.Play_SearchAchievementRateLoad(out play_SearchAchievementRateData);
                setPlayerData.SetPlayerStatusData(playerStatusData);
                setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
                sceneChangeSystem.SceneChange(SceneNameS.Home);
            }
        }

        // 名前入力後、確認の画面へ移る
        public void NameCheck()
        {
            playerStatusData.playerName = playerNameText.text;
            playerName_InputObj.SetActive(false);
            playerName_CheckObj.SetActive(true);
            playerNameCheckText.text = $"プレイヤー名\n{playerStatusData.playerName}\nでよろしいですか？";
        }
        // 名前が確定したら、ファイルを作成し、プレイヤ情報をスタックする
        // その後シーンを切り替える
        public void NameOk()
        {
            playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
            playLogDataAccess.PlayLogDataSeva(playLogData);
            play_SearchAchievementRateDataAccess.Play_SearchAchievementRateSave(play_SearchAchievementRateData);
            
            setPlayerData.SetPlayerStatusData(playerStatusData);
            setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
            
            sceneChangeSystem.SceneChange(SceneNameS.Home);
        }
        // 名前を変える場合、一時保存した名前をリセットし、画面を変える
        public void NameOut()
        {
            playerStatusData.playerName = "";
            playerName_InputObj.SetActive(true);
            playerName_CheckObj.SetActive(false);
        }
    }
}