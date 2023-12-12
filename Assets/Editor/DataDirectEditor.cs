using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections;

public class DataDirectEditor : EditorWindow
{
    // �s�����܂��ȍ�Ƃ�Enum(�I���^�u�Ɩ��ڂȊ֌W)
    enum TaskModo
    {
        File_Control,
        File_Add,
        Table_Control,
    }

    // �I���^�u
    private readonly string[] _taskTabToggles = { "�t�@�C������", "�t�@�C���쐬", "�e�[�u������" };
    private readonly string[] _tableTabToggles = { "�v�f�ꗗ" };

    private int _taskTabIndex;          // �I�𒆂̃^�u��id
    private int stockTaskTabIndex;      // �I�����Ă����^�u��id
    private int file_PopupIndex;        // �t�@�C���|�b�v�A�b�v��id
    private int set_file_PopupIndex;    // 
    private int columnNum;              // �J�����̐�
    private string folderPath;          // �t�H���_�[�̃p�X
    private string fileName = default;  // �t�@�C�����͎��̃t�@�C����
    private bool firstFlg = true;       // �^�u���؂�ւ��������
    private bool columnItemOpen = true; // �J�����̐܂���
    private bool opunFileNameFlg;
    private bool addColumNameFlg;
    private bool resetFlg;

    private string[] fileNames;                         // �ۑ�����Ă���f�[�^�t�@�C���̖��O
    private string[] columnNames = new string[0];       // �ϐ���
    private string[] fileAddData = new string[0];
    private string[,] fileDatas = new string[0,0];
    private bool[] fileTapItemOpens = new bool[0];
    GUIContent[] file_PopupDisplayOptions;              // �t�@�C���ꗗ

    [MenuItem("Window/DataDirectEdiotr")]
    static void Open()
    {
        // �G�f�B�^�E�B���h�E�̐����A���O�ݒ�
        var window = GetWindow<DataDirectEditor>();
        window.titleContent = new GUIContent("�f�[�^�̊Ǘ��E�ҏW");
    }

    #region �e���ڂ̏���
    // ���쎞�̏���
    private void File_ControlProcess() 
    {
        // �I�����ꂽ���ɍs������
        if (firstFlg)
        {
            opunFileNameFlg = false;
            addColumNameFlg = false;
            // �t�H���_�[�̊m�F
            folderPath = $"{Application.persistentDataPath}/BaseData";
            if (!Directory.Exists(folderPath))
            {
                // �Ȃ��ꍇ����
                Directory.CreateDirectory(folderPath);
            }

            // �t�H���_�[���̃t�@�C����ǂݍ���
            fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);
            // �t�@�C�������݂��Ȃ��Ȃ珈���I��
            if (fileNames.Length == 0)
            {
                return;
            }
            // �t�@�C�������H
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace(folderPath, "").Replace("\\", "").Replace(".csv", "");
            }

            // �|�b�v�A�b�v�ݒ�
            file_PopupDisplayOptions = new GUIContent[fileNames.Length];
            for (int i = 0; i < fileNames.Length; i++)
            {
                file_PopupDisplayOptions[i] = new GUIContent(fileNames[i]);
            }
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;
            firstFlg = false;
        }
        // �I�𒆂̃t�@�C���ԍ���ۑ�
        file_PopupIndex = EditorGUILayout.Popup(label: new GUIContent("�t�@�C��"), selectedIndex: file_PopupIndex, displayedOptions: file_PopupDisplayOptions);

        if (file_PopupIndex != set_file_PopupIndex)
        {
            set_file_PopupIndex = file_PopupIndex;
            fileName = file_PopupDisplayOptions[file_PopupIndex].text;
            opunFileNameFlg = false;
            addColumNameFlg = false;
        }

        // �t�@�C�������݂���Ȃ�
        if (fileNames.Length != 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("�t�@�C����");
                fileName = EditorGUILayout.TextField(fileName);
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    if (GUILayout.Button("�\��"))
                    {
                        var fileNamePath = $"{folderPath}/{file_PopupDisplayOptions[file_PopupIndex].text}.csv";
                        StreamReader sr = new StreamReader(fileNamePath);
                        {
                            string line = sr.ReadLine();
                            columnNames = line.Split(',');
                        }
                        opunFileNameFlg = !opunFileNameFlg;
                    }
                    if (GUILayout.Button("�ҏW"))
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
                    if (GUILayout.Button("�폜"))
                    {
                        Debug.LogError($"{file_PopupDisplayOptions[file_PopupIndex].text}���폜���܂���");
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
                        EditorGUILayout.LabelField("�J�����ꗗ");
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
                        EditorGUILayout.LabelField("�J�����ҏW");
                        columnNum = EditorGUILayout.IntField("�J������", columnNum);
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
                        if (GUILayout.Button("�ۑ�"))
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
                            Debug.Log($"{fileName}�̕ҏW���e��ۑ����܂���");
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    // �쐬���̏���
    private void File_AddProcess() 
    {
        if (firstFlg)
        {
            fileName = default;
            columnNum = default;
            firstFlg = false;
        }
        fileName = EditorGUILayout.TextField("�t�@�C����", fileName);
        columnNum = EditorGUILayout.IntField("�J������", columnNum);
        if (columnNum != columnNames.Length)
        {
            columnNames = new string[columnNum];
        }
        if (columnNum != 0)
        {
            bool _columnItemOpen = EditorGUILayout.Foldout(columnItemOpen, "�J������");
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
        if (GUILayout.Button("�쐬"))
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
            Debug.Log($"{fileName}�𐶐����܂���\n{folderPath}");
        }
    }

    // �e�[�u������
    private void Table_ControlProcess()
    {
        if (firstFlg)
        {        
            // �t�H���_�[�̊m�F
            folderPath = $"{Application.persistentDataPath}/BaseData";
            if (!Directory.Exists(folderPath))
            {
                // �Ȃ��ꍇ����
                Directory.CreateDirectory(folderPath);
            }
            // �t�H���_�[���̃t�@�C����ǂݍ���
            fileNames = Directory.GetFiles(folderPath, "*.csv", SearchOption.AllDirectories);
            // �t�@�C�������݂��Ȃ��Ȃ珈���I��
            if (fileNames.Length == 0)
            {
                return;
            }
            // �t�@�C�������H
            for (int i = 0; i < fileNames.Length; i++)
            {
                fileNames[i] = fileNames[i].Replace(folderPath, "").Replace("\\", "").Replace(".csv", "");
            }

            // �|�b�v�A�b�v�ݒ�
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
            // �I�𒆂̃t�@�C���ԍ���ۑ�
            file_PopupIndex = EditorGUILayout.Popup(label: new GUIContent("�t�@�C��"), selectedIndex: file_PopupIndex, displayedOptions: file_PopupDisplayOptions);
            if (GUILayout.Button("�X�V"))
            {
                firstFlg = true;
                resetFlg = true;
                Debug.Log("�X�V���܂���");
            }
        }
        EditorGUILayout.EndHorizontal();

        // �t�@�C����id�ɍX�V������Α��̎����X�V����
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

        // �t�@�C�������݂���Ȃ�
        if (fileNames.Length != 0)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                if (GUILayout.Button("�ǉ�"))
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
                    if (GUILayout.Button("�ۑ�"))
                    {
                        var fileNamePath = $"{folderPath}/{fileName}.csv";
                        StreamWriter sw = new StreamWriter(fileNamePath, true, Encoding.GetEncoding("Shift_JIS"));
                        string word = string.Join(",", fileAddData);
                        sw.WriteLine(word);
                        sw.Close();
                        firstFlg = true;
                        resetFlg = false;
                        Debug.Log("�f�[�^��ǉ����܂���");
                    }
                    EditorGUILayout.EndVertical();
                }
                if (GUILayout.Button("�\��"))
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
                            if (GUILayout.Button("�ۑ�"))
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
                            if (GUILayout.Button("�폜"))
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
                    Debug.LogWarning("�f�[�^������܂���");
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    #endregion

    //�@���C���ƂȂ鏈��
    private void OnGUI()
    {
        // �^�u�z�u����
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            _taskTabIndex = GUILayout.Toolbar(_taskTabIndex, _taskTabToggles, new GUIStyle(EditorStyles.toolbarButton), GUI.ToolbarButtonSize.FitToContents);
        }
        // �^�u�؂�ւ�茟�m
        if (_taskTabIndex != stockTaskTabIndex)
        {
            firstFlg = true;
            stockTaskTabIndex = _taskTabIndex;
        }
        // ���C������
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