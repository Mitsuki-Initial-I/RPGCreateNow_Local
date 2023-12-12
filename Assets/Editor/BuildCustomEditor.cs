using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

public class BuildCustomEditor : EditorWindow
{
    private static bool versionFileFlg;
    private static bool projectFolderFlg;
    private static bool zipFileFlg;
    private static bool versionFlg;

    private static string folderPath = "Build";
    private static string folderName = "MyFolder";
    private static string projectName = "MyGame";
    private static string zipName = "zipData";

    [MenuItem("Window/MyEditor/CustomBuildEditor")]
    static void Open()
    {
        // �G�f�B�^�E�B���h�E�̐����A���O�ݒ�
        var window = GetWindow<BuildCustomEditor>();
        window.titleContent = new GUIContent("�r���h�ҏW");
    }
    private void OnGUI()
    {
        versionFileFlg = EditorGUILayout.Toggle("�o�[�W�����t�@�C������", versionFileFlg);
        projectFolderFlg = EditorGUILayout.Toggle("�Ǝ��̃p�X�ɐ���", projectFolderFlg);
        if (projectFolderFlg)
        {
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField("�t�H���_�[�p�X", folderPath);
            if (GUILayout.Button("�Q��", GUILayout.Width(50)))
            {
                folderPath = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            folderName = EditorGUILayout.TextField("�t�H���_�[��", folderName);
            versionFlg = EditorGUILayout.Toggle(versionFlg);
            if (versionFlg)
            {
                folderName = PlayerSettings.bundleVersion;
                folderName = folderName.Replace('.', '_');
            }
            EditorGUILayout.EndHorizontal();
            zipFileFlg = EditorGUILayout.Toggle("Zip�t�@�C���ɂ��邩", zipFileFlg);
            if (zipFileFlg)
            {
                zipName = EditorGUILayout.TextField("���k�t�@�C����", zipName);
            }
        }
        else
        {
            folderPath = "Build";
        }
        projectName = EditorGUILayout.TextField("�v���W�F�N�g��", projectName);
        if (GUILayout.Button("�r���h"))
        {
            BuildGame();
        }
    }

    private static string[] GetScenePaths()
    {
        // �r���h����V�[���̃p�X���擾
        string[] scenePaths = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenePaths.Length; i++)
        {
            scenePaths[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenePaths;

    }
    async void BuildGame()
    {
        if (versionFileFlg)
        {
            // �o�[�W���������擾
            string version = PlayerSettings.bundleVersion;
            if (!Directory.Exists($"{folderPath}/{folderName}"))
            {
                Directory.CreateDirectory($"{folderPath}/{folderName}");
            }
            // �o�[�W���������e�L�X�g�t�@�C���ɏ�������
            string filePath = $"{folderPath}\\BuildVersion.txt";
            File.WriteAllText(filePath, version);
        }

        var buildPath = $"{folderPath}/{projectName}.exe";
        if (projectFolderFlg)
        {
            buildPath = $"{folderPath}/{folderName}/{projectName}.exe";
            if (!Directory.Exists($"{folderPath}/{folderName}"))
            {
                Directory.CreateDirectory($"{folderPath}/{folderName}");
            }
        }

        // �r���h�ݒ���쐬
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = GetScenePaths(); // �r���h����V�[���̃p�X
        buildOptions.locationPathName = buildPath; // �r���h�o�͐�
        buildOptions.target = BuildTarget.StandaloneWindows; // Windows�����Ƀr���h

        // �r���h�����s
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        await Task.Delay(5000);

        // �r���h���������������m�F
        if (report.summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("�r���h����");
            if (zipFileFlg && projectFolderFlg)
            {
                CompressFolder();

            }
            else
            {
                UnityEngine.Debug.LogError("Build failed.");
            }
        }
        static void CompressFolder()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new Process { StartInfo = startInfo };
            process.Start();

            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine($"cd {folderPath}");
                    sw.WriteLine($"powershell compress-archive {folderName} {zipName}");
                }
            }

            process.WaitForExit();
            process.Close();
        }
    }
}