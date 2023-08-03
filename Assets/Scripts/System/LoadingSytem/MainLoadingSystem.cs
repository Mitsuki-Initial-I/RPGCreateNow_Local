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
            // ��x�A�S�Ẳ�ʂ��\���ɂ���
            // �X�^�b�N�f�[�^���q�G�����L�[�ォ��擾����
            // �t�H���_�[�̃p�X�ƃt�@�C������錾�擾�A��Ԏn�߂̃p�X����p�ɕϐ���p��
            // ���ꂼ��̃A�N�Z�X��̃t�@�C������ݒ肷��
            playerName_InputObj.SetActive(false);
            playerName_CheckObj.SetActive(false);
            setPlayerData = GameObject.Find("StockPlayerData").GetComponent<IStockData>();
            string pass = $"{Application.persistentDataPath}/Data";
            string[] fileNames = setPlayerData.GetFileName();
            string playerStatusPass = $"{Application.persistentDataPath}/Data{fileNames[0]}";
            playerStatusDataAccess.fileName = fileNames[0];
            playLogDataAccess.fileName = fileNames[1];
            play_SearchAchievementRateDataAccess.fileName = fileNames[2];

            // �t�H���_�̑��݂��m�F
            // �Ȃ���΁A�S�Ẵf�[�^��p�ӂ��A�v���C�������͉�ʂֈړ�����
            if (!Directory.Exists(pass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playLogData = playLogDataAccess.FirstData();
                play_SearchAchievementRateData = play_SearchAchievementRateDataAccess.FirstData();
                playerName_InputObj.SetActive(true);
            }
            // �t�@�C�����m�F
            // �Ȃ���΁A�����f�[�^��p�ӂ��A�v���C�������͉�ʂֈړ�����
            else if (!File.Exists(playerStatusPass))
            {
                playerStatusData = playerStatusDataAccess.FirstData();
                playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
                playerName_InputObj.SetActive(true);
            }
            else
            {
                // �p�X���X�V���Ȃ���t�@�C�����m�F
                // ������΍쐬����
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
                //�S�Ẵf�[�^��ǂݍ��݁A�X�^�b�N�f�[�^�����������A�V�[����؂�ւ���
                playerStatusDataAccess.PlayerStatusDataLoad(out playerStatusData);
                playLogDataAccess.PlayLogDataLoad(out playLogData);
                play_SearchAchievementRateDataAccess.Play_SearchAchievementRateLoad(out play_SearchAchievementRateData);
                setPlayerData.SetPlayerStatusData(playerStatusData);
                setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
                sceneChangeSystem.SceneChange(SceneNameS.Home);
            }
        }

        // ���O���͌�A�m�F�̉�ʂֈڂ�
        public void NameCheck()
        {
            playerStatusData.playerName = playerNameText.text;
            playerName_InputObj.SetActive(false);
            playerName_CheckObj.SetActive(true);
            playerNameCheckText.text = $"�v���C���[��\n{playerStatusData.playerName}\n�ł�낵���ł����H";
        }
        // ���O���m�肵����A�t�@�C�����쐬���A�v���C�������X�^�b�N����
        // ���̌�V�[����؂�ւ���
        public void NameOk()
        {
            playerStatusDataAccess.PlayerStatusDataSeva(playerStatusData);
            playLogDataAccess.PlayLogDataSeva(playLogData);
            play_SearchAchievementRateDataAccess.Play_SearchAchievementRateSave(play_SearchAchievementRateData);
            
            setPlayerData.SetPlayerStatusData(playerStatusData);
            setPlayerData.SetPlay_SearchAchievementRateData(play_SearchAchievementRateData);
            
            sceneChangeSystem.SceneChange(SceneNameS.Home);
        }
        // ���O��ς���ꍇ�A�ꎞ�ۑ��������O�����Z�b�g���A��ʂ�ς���
        public void NameOut()
        {
            playerStatusData.playerName = "";
            playerName_InputObj.SetActive(true);
            playerName_CheckObj.SetActive(false);
        }
    }
}