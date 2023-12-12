using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;

public class DataDirectEditor : EditorWindow
{
    // 行える大まかな作業のEnum(選択タブと密接な関係)
    enum TaskModo
    {
        File_Control,
        File_Add,
        Table_Control,
    }

    // 選択タブ
    private readonly string[] _taskTabToggles = { "ファイル操作", "ファイル作成", "テーブル操作" };
    private readonly string[] _tableTabToggles = { "要素一覧" };

    private int _taskTabIndex;          // 選択中のタブのid
    private int stockTaskTabIndex;      // 選択していたタブのid
    private int file_PopupIndex;        // ファイルポップアップのid
    private int set_file_PopupIndex;    // 
    private int columnNum;              // カラムの数
    private string folderPath;          // フォルダーのパス
    private string fileName = default;  // ファイル入力時のファイル名
    private bool firstFlg = true;       // タブが切り替わった時か
    private bool columnItemOpen = true; // カラムの折り畳み
    private bool opunFileNameFlg;
    private bool addColumNameFlg;
    private bool resetFlg;

    private string[] fileNames;                         // 保存されているデータファイルの名前
    private string[] columnNames = new string[0];       // 変数名
    private string[] fileAddData = new string[0];
    private string[,] fileDatas = new string[0,0];
    private bool[] fileTapItemOpens = new bool[0];
    GUIContent[] file_PopupDisplayOptions;              // ファイル一覧

    [MenuItem("Window/DataDirectEdiotr")]
    static void Open()
    {
        // エディタウィンドウの生成、名前設定
        var window = GetWindow<DataDirectEditor>();
        window.titleContent = new GUIContent("データの管理・編集");
    }

    #region 各項目の処理
    // 操作時の処理
    private void File_ControlProcess() 
    {
        // 選択された時に行う処理
        if (firstFlg)
        {
            opunFileNameFlg = false;
            addColumNameFlg = false;
            // フォルダーの確認
            folderPath = $"{Application.persistentDataPath}/BaseData";
            if (!Directory.Exists(folderPath))
            {
                // ない場合生成
                Directory.CreateDirectory(folderPath);
            }

            // フォルダー内のファイルを読み込み
            fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);
            // ファイルが存在しないなら処理終了
            if (fileNames.Length == 0)
            {
                return;
            }
            // ファイルを加工
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace(folderPath, "").Replace("\\", "").Replace(".csv", "");
            }

            // ポップアップ設定
            file_PopupDisplayOptions = new GUIContent[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                file_PopupDisplayOptions[i] = new GUIContent(fileNames[i]);
            }
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;
            firstFlg = false;
        }
        // 選択中のファイル番号を保存
        file_PopupIndex = EditorGUILayout.Popup(label: new GUIContent("ファイル"), selectedIndex: file_PopupIndex, displayedOptions: file_PopupDisplayOptions);

        if (file_PopupIndex != set_file_PopupIndex)
        {
            set_file_PopupIndex = file_PopupIndex;
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;
            opunFileNameFlg = false;
            addColumNameFlg = false;
        }

        // ファイルが存在するなら
        if (fileNames.Length != 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("ファイル名");
                fileName = EditorGUILayout.TextField(fileName);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button("表示"))
                    {
                        var fileNamePath = $"{folderPath}/{file_PopupDisplayOptions[file_PopupIndex].text}.csv";
                        StreamReader sr = new StreamReader(fileNamePath);
                        {
                            string line = sr.ReadLine();
                            columnNames = line.Split(',');
                        }
                        opunFileNameFlg = !opunFileNameFlg;
                    }
                    if (GUILayout.Button("編集"))
                    {
                        var fileNamePath = $"{folderPath}/{file_PopupDisplayOptions[file_PopupIndex].text}.csv";
                        StreamReader sr = new StreamReader(fileNamePath);
                        {
                            string line = sr.ReadLine();
                            columnNames = line.Split(',');
                            columnNum = columnNames.Length;
                        }
                        addColumNameFlg = !addColumNameFlg;
                    }
                    if (GUILayout.Button("削除"))
                    {
                        Debug.LogError($"{file_PopupDisplayOptions[file_PopupIndex].text}を削除しました");
                        string filePath = $"{folderPath}/{file_PopupDisplayOptions[file_PopupIndex].text}.csv";
                        File.Delete(filePath);
                        file_PopupIndex = 0;
                        firstFlg = true;
                    }
                }
                EditorGUILayout.EndHorizontal();
                if (opunFileNameFlg)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.LabelField("カラム一覧");
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            EditorGUILayout.LabelField(columnNames[i]);
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                if (addColumNameFlg)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    {
                        EditorGUILayout.LabelField("カラム編集");
                        columnNum = EditorGUILayout.IntField("カラム数", columnNum);
                        if (columnNum != columnNames.Length)
                        {
                            var setName = columnNames;
                            for (int i = 0; i < setName.Length; i++)
                            {
                                setName[i] = columnNames[i];
                            }
                            columnNames = new string[columnNum];
                            for (int i = 0; i < setName.Length; i++)
                            {
                                if (columnNames.Length == i)
                                {
                                    return;
                                }
                                columnNames[i] = setName[i];
                            }
                        }
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            columnNames[i] = EditorGUILayout.TextField(columnNames[i], columnNames[i]);
                        }
                        if (GUILayout.Button("保存"))
                        {
                            string word = string.Join(",", columnNames);
                            string filePath = $"{folderPath}/{file_PopupDisplayOptions[file_PopupIndex].text}.csv";

                            File.Delete(filePath);

                            filePath = $"{folderPath}/{fileName}.csv";//Path.Combine(folderPath, fileName);

                            if (!File.Exists(filePath))
                            {
                                using (FileStream fs = File.Create(filePath))
                                {
                                    // 
                                }
                            }

                            StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);//Encoding.GetEncoding("Shift_JIS"));
                            sw.WriteLine(word);
                            sw.Close();
                            firstFlg = true;
                            Debug.Log($"{fileName}の編集内容を保存しました");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    // 作成時の処理
    private void File_AddProcess() 
    {
        if (firstFlg)
        {
            fileName = default;
            columnNum = default;
            firstFlg = false;
        }
        fileName = EditorGUILayout.TextField("ファイル名", fileName);
        columnNum = EditorGUILayout.IntField("カラム数", columnNum);
        if (columnNum != columnNames.Length)
        {
            columnNames = new string[columnNum];
        }
        if (columnNum != 0)
        {
            bool _columnItemOpen = EditorGUILayout.Foldout(columnItemOpen, "カラム名");
            if (_columnItemOpen != columnItemOpen)
            {
                columnItemOpen = _columnItemOpen;
            }
        }
        if (columnItemOpen && columnNum != 0)
        {
            for (int i = 0; i < columnNum; i++)
            {
                columnNames[i] = EditorGUILayout.TextField(columnNames[i], columnNames[i]);
            }
        }
        if (GUILayout.Button("作成"))
        {
            string word = string.Join(",", columnNames);
            folderPath = $"{Application.persistentDataPath}/BaseData";
            Debug.Log(folderPath);
            Debug.Log(fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, fileName);
            filePath += ".csv";
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    // 
                }
            }

            StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8);
            //            StreamWriter sw = new StreamWriter(filePath, true, Encoding.GetEncoding("Shift_JIS"));
            sw.WriteLine(word);
            sw.Close();
            Debug.Log($"{fileName}を生成しました\n{folderPath}");
        }
    }

    // テーブル操作
    private void Table_ControlProcess()
    {
        if (firstFlg)
        {        
            // フォルダーの確認
            folderPath = $"{Application.persistentDataPath}/BaseData";
            if (!Directory.Exists(folderPath))
            {
                // ない場合生成
                Directory.CreateDirectory(folderPath);
            }
            // フォルダー内のファイルを読み込み
            fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);
            // ファイルが存在しないなら処理終了
            if (fileNames.Length == 0)
            {
                return;
            }
            // ファイルを加工
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace(folderPath, "").Replace("\\", "").Replace(".csv", "");
            }

            // ポップアップ設定
            file_PopupDisplayOptions = new GUIContent[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                file_PopupDisplayOptions[i] = new GUIContent(fileNames[i]);
            }
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;
            firstFlg = false;
            opunFileNameFlg = false;
            addColumNameFlg = false;
        }
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        {
            // 選択中のファイル番号を保存
            file_PopupIndex = EditorGUILayout.Popup(label: new GUIContent("ファイル"), selectedIndex: file_PopupIndex, displayedOptions: file_PopupDisplayOptions);
            if (GUILayout.Button("更新"))
            {
                firstFlg = true;
                resetFlg = true;
                Debug.Log("更新しました");
            }
        }
        EditorGUILayout.EndHorizontal();

        // ファイルのidに更新があれば他の事も更新する
        if (file_PopupIndex != set_file_PopupIndex|| resetFlg)
        {
            set_file_PopupIndex = file_PopupIndex;
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;

            var fileNamePath = $"{folderPath}/{fileName}.csv";
            StreamReader sr = new StreamReader(fileNamePath, Encoding.GetEncoding("Shift_JIS"));

            List<string> line = new List<string>();
            line.Add(sr.ReadLine());
            columnNames = line[0].Split(',');

            while (sr.Peek() != -1)
            {
                line.Add(sr.ReadLine());
            }
            fileDatas = new string[line.Count - 1, columnNames.Length];
            for (int i = 0; i < line.Count - 1; i++)
            {
                var str = line[i + 1].Split(',');
                for (int j = 0; j < columnNames.Length; j++)
                {
                    fileDatas[i, j] = str[j];
                }
            }
            fileTapItemOpens = new bool[fileDatas.GetLength(0)];
            sr.Close();

            opunFileNameFlg = false;
            addColumNameFlg = false;
            resetFlg = false;
        }

        // ファイルが存在するなら
        if (fileNames.Length != 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("追加"))
                {
                    addColumNameFlg = !addColumNameFlg;
                    fileAddData = new string[columnNames.Length];
                }
                if (addColumNameFlg)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    for (int i = 0; i < fileAddData.Length; i++)
                    {
                        fileAddData[i] = EditorGUILayout.TextField(columnNames[i], fileAddData[i]);
                    }
                    if (GUILayout.Button("保存"))
                    {
                        var fileNamePath = $"{folderPath}/{fileName}.csv";
                        StreamWriter sw = new StreamWriter(fileNamePath, true, Encoding.GetEncoding("Shift_JIS"));
                        string word = string.Join(",", fileAddData);
                        sw.WriteLine(word);
                        sw.Close();
                        firstFlg = true;
                        resetFlg = false;
                        Debug.Log("データを追加しました");
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("表示"))
                {
                    opunFileNameFlg = !opunFileNameFlg;
                }
                if (opunFileNameFlg && fileDatas.Length != 0)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.box);
                    for (int i = 0; i < fileDatas.GetLength(0); i++)
                    {
                        bool _fileTapItemOpens = EditorGUILayout.Foldout(fileTapItemOpens[i],$"{columnNames[1]}:{fileDatas[i, 1]}");
                        if (_fileTapItemOpens != fileTapItemOpens[i])
                        {
                            fileTapItemOpens[i] = _fileTapItemOpens;
                        }
                        if (fileTapItemOpens[i])
                        {
                            for (int j = 0; j < fileDatas.GetLength(1); j++)
                            {
                                fileDatas[i, j] = EditorGUILayout.TextField(columnNames[j], fileDatas[i, j]);
                            }
                            EditorGUILayout.BeginHorizontal();
                            if (GUILayout.Button("保存"))
                            {
                                var fileNamePath = $"{folderPath}/{fileName}.csv";

                                StreamWriter sw = new StreamWriter(fileNamePath, false, Encoding.GetEncoding("Shift_JIS"));
                                string column = string.Join(",", columnNames);
                                sw.WriteLine(column);

                                for (int j = 0; j < fileDatas.GetLength(0); j++)
                                {
                                    string word = fileDatas[j, 0];

                                    for (int k = 1; k < fileDatas.GetLength(1); k++)
                                    {
                                        word += $",{fileDatas[j, k]}";
                                    }
                                    sw.WriteLine(word);
                                }
                                sw.Close();
                                firstFlg = true;
                            }
                            if (GUILayout.Button("削除"))
                            {
                                var fileNamePath = $"{folderPath}/{fileName}.csv";

                                StreamWriter sw = new StreamWriter(fileNamePath, false, Encoding.GetEncoding("Shift_JIS"));
                                string column = string.Join(",", columnNames);
                                sw.WriteLine(column);

                                for (int j = 0; j < fileDatas.GetLength(0); j++)
                                {
                                    if (j != i)
                                    {
                                        string word = fileDatas[j, 0];
                                        for (int k = 1; k < fileDatas.GetLength(1); k++)
                                        {
                                            word += $",{fileDatas[j, k]}";
                                        }
                                        sw.WriteLine(word);
                                    }
                                }
                                sw.Close();
                                firstFlg = true;
                                resetFlg = false;
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else if (opunFileNameFlg)
                {
                    opunFileNameFlg = !opunFileNameFlg;
                    Debug.LogWarning("データがありません");
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    #endregion

    //　メインとなる処理
    private void OnGUI()
    {
        // タブ配置処理
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            _taskTabIndex = GUILayout.Toolbar(_taskTabIndex, _taskTabToggles, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.FitToContents);
        }
        // タブ切り替わり検知
        if (_taskTabIndex != stockTaskTabIndex)
        {
            firstFlg = true;
            stockTaskTabIndex = _taskTabIndex;
        }
        // メイン処理
        switch ((TaskModo)_taskTabIndex)
        {
            case TaskModo.File_Control:
                File_ControlProcess();
                break;
            case TaskModo.File_Add:
                File_AddProcess();
                break;
            case TaskModo.Table_Control:
                Table_ControlProcess();
                break;
            default:
                break;
        }
    }
}