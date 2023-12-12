using System.IO.Compression;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ZipOpenEditor : EditorWindow
{
    private string url;
    private string zipName="zipData.zip";
    private string localPath;
    private string savePath;
    private bool zipPathDownload;

    private UnityWebRequest webRequest;

    [MenuItem("Window/ZipOpen")]
    static void Open()
    {
        // エディタウィンドウの生成、名前設定
        var window = GetWindow<ZipOpenEditor>();
        window.titleContent = new GUIContent("Zipダンロード展開");
    }
    private void OnGUI()
    {
        zipPathDownload = EditorGUILayout.Toggle("オンライン上のZipファイルですか", zipPathDownload);
        if (zipPathDownload)
        {
            url = EditorGUILayout.TextField("ダウンロードzipURL", url);
            zipName = EditorGUILayout.TextField("Zipファイル名", zipName);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            localPath = EditorGUILayout.TextField("ローカルZipパス", localPath);
            if (GUILayout.Button("参照", GUILayout.Width(50)))
            {
                localPath = EditorUtility.OpenFolderPanel("Select Folder", localPath, "");
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        savePath = EditorGUILayout.TextField("展開場所", savePath, "");
        if (GUILayout.Button("参照", GUILayout.Width(50)))
        {
            savePath = EditorUtility.OpenFolderPanel("Select Folder", savePath, "");
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("展開"))
        {
            ZipOpenProcess();
        }
    }

    private void ZipOpenProcess()
    {
        if (zipPathDownload) 
        {
            localPath = $"{Application.persistentDataPath}\\{zipName}";
            webRequest.downloadHandler = new DownloadHandlerFile(localPath);
            if(webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(webRequest.error);
            }
        }
        ZipFile.ExtractToDirectory(localPath, $"{savePath}\\");
    }
}