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
        // エディタウィンドウの生成、名前設定
        var window = GetWindow<BuildCustomEditor>();
        window.titleContent = new GUIContent("ビルド編集");
    }
    private void OnGUI()
    {
        versionFileFlg = EditorGUILayout.Toggle("バージョンファイル生成", versionFileFlg);
        projectFolderFlg = EditorGUILayout.Toggle("独自のパスに生成", projectFolderFlg);
        if (projectFolderFlg)
        {
            EditorGUILayout.BeginHorizontal();
            folderPath = EditorGUILayout.TextField("フォルダーパス", folderPath);
            if (GUILayout.Button("参照", GUILayout.Width(50)))
            {
                folderPath = EditorUtility.OpenFolderPanel("Select Folder", folderPath, "");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            folderName = EditorGUILayout.TextField("フォルダー名", folderName);
            versionFlg = EditorGUILayout.Toggle(versionFlg);
            if (versionFlg)
            {
                folderName = PlayerSettings.bundleVersion;
                folderName = folderName.Replace('.', '_');
            }
            EditorGUILayout.EndHorizontal();
            zipFileFlg = EditorGUILayout.Toggle("Zipファイルにするか", zipFileFlg);
            if (zipFileFlg)
            {
                zipName = EditorGUILayout.TextField("圧縮ファイル名", zipName);
            }
        }
        else
        {
            folderPath = "Build";
        }
        projectName = EditorGUILayout.TextField("プロジェクト名", projectName);
        if (GUILayout.Button("ビルド"))
        {
            BuildGame();
        }
    }

    private static string[] GetScenePaths()
    {
        // ビルドするシーンのパスを取得
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
            // バージョン情報を取得
            string version = PlayerSettings.bundleVersion;
            if (!Directory.Exists($"{folderPath}/{folderName}"))
            {
                Directory.CreateDirectory($"{folderPath}/{folderName}");
            }
            // バージョン情報をテキストファイルに書き込む
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

        // ビルド設定を作成
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = GetScenePaths(); // ビルドするシーンのパス
        buildOptions.locationPathName = buildPath; // ビルド出力先
        buildOptions.target = BuildTarget.StandaloneWindows; // Windows向けにビルド

        // ビルドを実行
        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

        await Task.Delay(5000);

        // ビルドが成功したかを確認
        if (report.summary.result == BuildResult.Succeeded)
        {
            UnityEngine.Debug.Log("ビルド完了");
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