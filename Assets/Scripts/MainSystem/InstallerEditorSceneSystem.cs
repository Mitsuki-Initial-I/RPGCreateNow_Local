using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InstallerEditorSceneSystem : MonoBehaviour
{
    private string url = 
        "https://drive.google.com/uc?id=1nr4AiNAdPHHYwjtIaflK4XGoYea16Qs6";

    public Slider progressBar;
    public Text progressText;

    private UnityWebRequest webRequest;
    float progress;

    //?usp=sharing"; // ダウンロードしたいデータのURL
    //https://drive.google.com/file/d/1nr4AiNAdPHHYwjtIaflK4XGoYea16Qs6/view?usp=sharing
    
    void Start()
    {
        StartCoroutine(DownloadData());
    }

    IEnumerator DownloadData()
    {
        webRequest = UnityWebRequest.Get(url);
        string localPath = $"{Application.persistentDataPath}\\downloadedData.zip";
        webRequest.downloadHandler =// new DownloadHandlerBuffer(); 
new DownloadHandlerFile(localPath);

        webRequest.SendWebRequest();
        
        while (!webRequest.isDone)
        {
            progress = webRequest.downloadProgress;
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F2") + "%";
            Debug.Log("リクエスト中");
            yield return null;
        }

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            progress = 1;
            progressBar.value = progress;
            progressText.text = (progress * 100f).ToString("F2") + "%";

            ZipFile.ExtractToDirectory(localPath, $"{Application.persistentDataPath}\\");
        }
    }
}