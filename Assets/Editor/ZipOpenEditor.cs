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
        // �G�f�B�^�E�B���h�E�̐����A���O�ݒ�
        var window = GetWindow<ZipOpenEditor>();
        window.titleContent = new GUIContent("Zip�_�����[�h�W�J");
    }
    private void OnGUI()
    {
        zipPathDownload = EditorGUILayout.Toggle("�I�����C�����Zip�t�@�C���ł���", zipPathDownload);
        if (zipPathDownload)
        {
            url = EditorGUILayout.TextField("�_�E�����[�hzipURL", url);
            zipName = EditorGUILayout.TextField("Zip�t�@�C����", zipName);
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            localPath = EditorGUILayout.TextField("���[�J��Zip�p�X", localPath);
            if (GUILayout.Button("�Q��", GUILayout.Width(50)))
            {
                localPath = EditorUtility.OpenFolderPanel("Select Folder", localPath, "");
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.BeginHorizontal();
        savePath = EditorGUILayout.TextField("�W�J�ꏊ", savePath, "");
        if (GUILayout.Button("�Q��", GUILayout.Width(50)))
        {
            savePath = EditorUtility.OpenFolderPanel("Select Folder", savePath, "");
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("�W�J"))
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