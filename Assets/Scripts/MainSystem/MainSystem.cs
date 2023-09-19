using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MainSystem : MonoBehaviour
{
    [SerializeField]
    string[] saveFolderNames;
    [SerializeField]
    string[] saveFileNames;
    [SerializeField]
    string[] resourceFileNames;

    [SerializeField]
    GameObject[] MainObjects;

    GameSceneNames gamestate = GameSceneNames.LoadData_First;

    #region �Z�[�u�t�@�C���֘A
    const byte FOURBITE = 4;
    const byte ONEBYTE = 8;
    const byte TWOBYTE = 16;
    const byte THREEBYTE = 24;

    // �󂯎�����f�[�^���V�t�g���Z�q�ŉE����0����ǉ����A�l��Ԃ�
    int CheckNumberCaculate(byte[] data)
    {
        int csum = data[data.Length - 1];
        csum <<= 1;
        return csum;
    }
    // data�̒l��0��1�𔽓]������
    static void EncryptionSystem(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= 0xff;
        }
    }
    // �Z�[�u�V�X�e��(struct�̃��m�̂�)
    public bool SaveFileSystem<T>(string folderPath, string fileName, T saveData) where T : struct
    {
        fileName = folderPath + fileName;
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        byte[] binaryData;
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter writer = new BinaryFormatter();
            writer.Serialize(ms, saveData);
            binaryData = new byte[ms.Length + FOURBITE];
            ms.ToArray().CopyTo(binaryData, FOURBITE);

            int csum = CheckNumberCaculate(binaryData);

            binaryData[0] = (byte)((csum >> THREEBYTE) & 0xff);
            binaryData[1] = (byte)((csum >> TWOBYTE) & 0xff);
            binaryData[2] = (byte)((csum >> ONEBYTE) & 0xff);
            binaryData[3] = (byte)(csum & 0xff);
        }

        EncryptionSystem(binaryData);

        using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
            try
            {
                fs.Write(binaryData, 0, binaryData.Length);
            }
            catch (Exception)
            {
                return false;
            }
        }
        return true;
    }
    // ���[�h�V�X�e��(struct�̃��m�̂�)
    public bool LoadFileSystem<T>(string folderPath, string fileName, out T loadData) where T : struct
    {
        fileName = folderPath + fileName;
        if (!Directory.Exists(folderPath) || !File.Exists(fileName))
        {
            loadData = default;
            return false;
        }
        byte[] fileData;
        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            try
            {
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
            }
            catch (Exception)
            {
                loadData = default;
                return false;
            }
        }

        EncryptionSystem(fileData);

        int csum = CheckNumberCaculate(fileData);
        int sum = (fileData[0] << THREEBYTE) | (fileData[1] << TWOBYTE) | (fileData[2] << ONEBYTE) | (fileData[3]);
        if (csum != sum)
        {
            loadData = default;
            return false;
        }
        byte[] binaryData = new byte[fileData.Length - FOURBITE];
        Array.Copy(fileData, FOURBITE, binaryData, 0, binaryData.Length);
        using (MemoryStream ms = new MemoryStream(binaryData))
        {
            BinaryFormatter reader = new BinaryFormatter();
            loadData = (T)reader.Deserialize(ms);
        }

        return true;
    }
    #endregion

    void LoadSaveDatas()
    {
        string pass = $"{Application.persistentDataPath}/{saveFolderNames[(int)FolderNames.MasterFolder]}";

        MasterPlayData_Struct masterPlayData_ = new MasterPlayData_Struct();
        PrivatePlayData_Struct[] privatePlayData_s = new PrivatePlayData_Struct[3];
        if (!Directory.Exists(pass))
        {
            masterPlayData_.gameVersion = Application.version;
            SaveFileSystem(pass, saveFileNames[0], masterPlayData_);

            for (int i = 0; i < privatePlayData_s.Length; i++)
            {
                privatePlayData_s[i] = default;
                pass = $"{Application.persistentDataPath}/{saveFolderNames[(int)FolderNames.MasterFolder]}/{saveFolderNames[(int)FolderNames.PrivateFolder_00 + i]}";
                SaveFileSystem(pass, saveFileNames[1], privatePlayData_s[i]);
            }
        }
        else
        {
            LoadFileSystem(pass, saveFileNames[0], out masterPlayData_);

            for (int i = 0; i < privatePlayData_s.Length; i++)
            {
                privatePlayData_s[i] = default;
                pass = $"{Application.persistentDataPath}/{saveFolderNames[(int)FolderNames.MasterFolder]}/{saveFolderNames[(int)FolderNames.PrivateFolder_00 + i]}";
                LoadFileSystem(pass, saveFileNames[1], out privatePlayData_s[i]);
            }
        }
    }

    void GameStateProcess()
    {
        switch (gamestate)
        {
            case GameSceneNames.LoadData_First:
                // �f�[�^�̓ǂݍ���
                LoadSaveDatas();
                gamestate = GameSceneNames.Title;
                break;
            case GameSceneNames.Title:
                // �^�C�g���ƃ{�^���\��
                MainObjects[(int)MainGameObjectNames.Title].SetActive(true);
                break;
            case GameSceneNames.PlayerSetting_Name:
                SaveDataButton();
                break;
            case GameSceneNames.PlayerSetting_Specoes:
                MainObjects[(int)MainGameObjectNames.SelectButtons].SetActive(true);
                MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
                break;
            case GameSceneNames.PlayerSetting_Job:
                MainObjects[(int)MainGameObjectNames.SelectButtons].SetActive(true);
                MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
                break;
            case GameSceneNames.PlayerSetting_Status:
                MainObjects[(int)MainGameObjectNames.StatusUp].SetActive(true);
                MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
                break;
            case GameSceneNames.PlayerSetting_Skill:
                MainObjects[(int)MainGameObjectNames.SelectButtons].SetActive(true);
                MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
                break;
            case GameSceneNames.PlayerSetting_Check:
                MainObjects[(int)MainGameObjectNames.Status].SetActive(true);
                break;
            case GameSceneNames.Home:
                MainObjects[(int)MainGameObjectNames.Home].SetActive(true);
                break;
            case GameSceneNames.Shop:
                MainObjects[(int)MainGameObjectNames.Shop].SetActive(true);
                break;
            case GameSceneNames.Battle:
                MainObjects[(int)MainGameObjectNames.BattleSelectButton].SetActive(true);
                MainObjects[(int)MainGameObjectNames.BattleStatus].SetActive(true);
                MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
                break;
        }
    }

    #region Button
    // �^�C�g���ŕ\�������{�^���p
    // �^�C�g������Z�[�u�f�[�^�I���ɐ؂�ւ���
    public void StratGameButton()
    {
        MainObjects[(int)MainGameObjectNames.Title].SetActive(false);
        MainObjects[(int)MainGameObjectNames.SaveData].SetActive(true);
    }

    // �Z�[�u�f�[�^�I���ŕ\�������{�^���p
    // �Z�[�u�f�[�^�I������v���C���\���ݒ�ɐ؂�ւ���
    public void SaveDataButton()
    {
        MainObjects[(int)MainGameObjectNames.SaveData].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
        MainObjects[(int)MainGameObjectNames.PlayerName].SetActive(true);
    }

    // �v���C���\���ݒ�ŕ\�������{�^���p
    // �v���C���\���ݒ肩��m�F��ʂɐ؂�ւ���
    public void PlayerNameButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(true);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
        MainObjects[(int)MainGameObjectNames.PlayerName].SetActive(false);
        gamestate = GameSceneNames.PlayerSetting_Name;
    }

    // ���������̒�����I�������ʂŕ\�������{�^���p
    // �I����ʂ���m�F��ʂɐ؂�ւ���
    public void SelectButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(true);
        MainObjects[(int)MainGameObjectNames.SelectButtons].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
    }

    // �X�e�[�^�X�A�b�v��ʂŕ\�������{�^���p
    // �X�e�[�^�X�A�b�v��ʂ��玟�̃X�e�[�g�ɐ؂�ւ���
    public void ClauseStatusUpButton()
    {
        MainObjects[(int)MainGameObjectNames.StatusUp].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
        gamestate++;
        GameStateProcess();
    }

    // �X�e�[�^�X�m�F�ŕ\�������{�^���p
    public void StatusCheckButton()
    {
        MainObjects[(int)MainGameObjectNames.Status].SetActive(false);
        gamestate++;
        GameStateProcess();
    }

    // �m�F��ʂŕ\�������m��{�^���p
    // �m�F��ʂ��玟�̃X�e�[�g�֐؂�ւ���
    public void Check_DeterminationButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(false);
        gamestate++;
        GameStateProcess();
    }

    // �m�F��ʂŕ\�������ے�{�^���p
    // �m�F��ʂ��猳�̉�ʂ֐؂�ւ���
    public void Check_CancelButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(false);
        GameStateProcess();
    }
    #endregion
    IEnumerator Title()
    {
        yield return new WaitForSeconds(1f);
        GameStateProcess();
    }
    void Start()
    {
        GameStateProcess();
        StartCoroutine(Title());
    }
    void Update()
    {

    }
}