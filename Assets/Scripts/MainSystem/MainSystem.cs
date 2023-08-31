using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    #region セーブファイル関連
    const byte FOURBITE = 4;
    const byte ONEBYTE = 8;
    const byte TWOBYTE = 16;
    const byte THREEBYTE = 24;

    // 受け取ったデータをシフト演算子で右側に0を一つ追加し、値を返す
    int CheckNumberCaculate(byte[] data)
    {
        int csum = data[data.Length - 1];
        csum <<= 1;
        return csum;
    }
    // dataの値を0と1を反転させる
    static void EncryptionSystem(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            data[i] ^= 0xff;
        }
    }
    // セーブシステム(structのモノのみ)
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
    // ロードシステム(structのモノのみ)
    public bool LoadFileSystem<T>(string folderPath, string fileName, out T loadData) where T : struct
    {
        fileName = folderPath + fileName;
        if (!Directory.Exists(folderPath)|| !File.Exists(fileName))
        {
            loadData = default;
            return false;
        }
        byte[] fileData;
        using (FileStream fs=new FileStream(fileName,FileMode.Open,FileAccess.Read))
        {
            try
            {
                fileData = new byte[fs.Length];
                fs.Read(fileData, 0, fileData.Length);
            }
            catch(Exception)
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
        using (MemoryStream ms=new MemoryStream(binaryData)) 
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

    public void StratGameButton()
    {
        MainObjects[(int)MainGameObjectNames.Title].SetActive(false);
        MainObjects[(int)MainGameObjectNames.SaveData].SetActive(true);
    }
    public void SaveDataButton()
    {
        MainObjects[(int)MainGameObjectNames.SaveData].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(true);
        MainObjects[(int)MainGameObjectNames.PlayerName].SetActive(true);
    }
    public void PlayerNameButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(true);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
        MainObjects[(int)MainGameObjectNames.PlayerName].SetActive(false);
        gamestate = GameSceneNames.PlayerSetting_Name;
    }
    public void SelectButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(true);
        MainObjects[(int)MainGameObjectNames.SelectButtons].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
    }
    public void ClauseStatusUpButton()
    {
        MainObjects[(int)MainGameObjectNames.StatusUp].SetActive(false);
        MainObjects[(int)MainGameObjectNames.CommuTexxt].SetActive(false);
        gamestate++;
        GameStateProcess();
    }

    public void Check_DeterminationButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(false);
        gamestate++;
        GameStateProcess();
    }
    public void Check_CancelButton()
    {
        MainObjects[(int)MainGameObjectNames.Check].SetActive(false); 
        GameStateProcess();
    }

    void GameStateProcess()
    {
        switch (gamestate)
        {
            case GameSceneNames.LoadData_First:
                // データの読み込み
                //   LoadSaveDatas();
                gamestate = GameSceneNames.Title;
                break;
            case GameSceneNames.Title:
                // タイトルとボタン表示
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
                break;
            case GameSceneNames.Home:
                break;
            case GameSceneNames.LoadData_Battle:
                break;
            case GameSceneNames.Battle:
                break;
        }
    }

    void Start()
    {
        GameStateProcess();
        StartCoroutine(Title());
    }
    IEnumerator Title()
    {
        yield return new WaitForSeconds(1f);
        GameStateProcess();
    }

    void Update()
    {
    }
}