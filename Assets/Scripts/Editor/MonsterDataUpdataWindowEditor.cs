using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class MonsterDataUpdataWindowEditor : EditorWindow
{
    [System.Serializable]
    public class MonsterRo
    {
        public int mapNumber;
        public int stageNumber;
        public string enemyName;
        public int hp;
        public int mp;
        public int ap;
        public int dp;
        public int map;
        public int mdp;
        public int sp;
        public int lv;
        public int dropExp;
    }

    //===========================

    MonsterRo monsterRo = new MonsterRo();
    List<MonsterRo> ene_list = new List<MonsterRo>();

    string[] m_PopupDisplayOptions = new string[1];
    int m_PopupIndex;
    int checkNum;

    [MenuItem("Window/MyWindow/MonsterData")]
    private static void WindowOpen()
    {
        var window = GetWindow<MonsterDataUpdataWindowEditor>(title: "モンスターデータ");
        window.ShowAuxWindow();
    }
    void DataSave()
    {
        StreamWriter sw = new StreamWriter(@"C:/Users/user/Desktop/Private/MyCreative/CreateNow/RPGCreate_Local/Assets/Resources/MonserData.csv", false, Encoding.GetEncoding("UTF-8"));
        for (int i = 0; i < ene_list.Count; i++)
        {
            string s1 = $"{ene_list[i].mapNumber},{ene_list[i].stageNumber},{ene_list[i].enemyName},{ene_list[i].hp},{ene_list[i].mp},{ene_list[i].ap},{ene_list[i].dp},{ene_list[i].map},{ene_list[i].mdp},{ene_list[i].sp},{ene_list[i].lv},{ene_list[i].dropExp}";
            sw.WriteLine(s1);
        }
        sw.Close();
    }
    void DataLoad()
    {
        ene_list.Clear();
        AssetDatabase.Refresh();
        List<string[]> csvDatas = new List<string[]>();
        int height = 0;
        TextAsset textAsset = Resources.Load("MonserData") as TextAsset;
        StringReader reader = new StringReader(textAsset.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            // ,で区切ってCSVに格納
            csvDatas.Add(line.Split(','));
            height++; // 行数加算
        }
        reader.Close();
        m_PopupDisplayOptions = new string[height];

        for (int i = 0; i < height; i++)
        {
            monsterRo = new MonsterRo();
            monsterRo.mapNumber = int.Parse(csvDatas[i][0]);
            monsterRo.stageNumber = int.Parse(csvDatas[i][1]);
            monsterRo.enemyName = csvDatas[i][2];
            monsterRo.hp = int.Parse(csvDatas[i][3]);
            monsterRo.mp = int.Parse(csvDatas[i][4]);
            monsterRo.ap = int.Parse(csvDatas[i][5]);
            monsterRo.dp = int.Parse(csvDatas[i][6]);
            monsterRo.map = int.Parse(csvDatas[i][7]);
            monsterRo.mdp = int.Parse(csvDatas[i][8]);
            monsterRo.sp = int.Parse(csvDatas[i][9]);
            monsterRo.lv = int.Parse(csvDatas[i][10]);
            monsterRo.dropExp = int.Parse(csvDatas[i][11]);
            ene_list.Add(monsterRo);
            m_PopupDisplayOptions[i] = $"{monsterRo.mapNumber}-{monsterRo.stageNumber}:{ monsterRo.enemyName}";
        }
        Debug.Log(ene_list[0].enemyName);
        m_PopupIndex = 0;
        monsterRo = ene_list[m_PopupIndex];
    }
    private void OnGUI()
    {
        /*  
           textAsset = (TextAsset)EditorGUILayout.ObjectField(textAsset, typeof(TextAsset), true);
            text = textAsset.text;
            EditorGUILayout.TextArea(text);
         */

        if (GUILayout.Button("ロード"))
        {
            DataLoad();
        }

        m_PopupIndex = EditorGUILayout.Popup(label: "選択", selectedIndex: m_PopupIndex, displayedOptions: m_PopupDisplayOptions);
        if (checkNum != m_PopupIndex)
        {
            checkNum = m_PopupIndex;
            monsterRo = ene_list[checkNum];
        }

        EditorGUILayout.TextArea(m_PopupIndex.ToString());

        monsterRo.mapNumber = EditorGUILayout.IntField(label: "マップ数", value: monsterRo.mapNumber);
        monsterRo.stageNumber = EditorGUILayout.IntField(label: "ステージ数", value: monsterRo.stageNumber);
        monsterRo.enemyName = EditorGUILayout.TextField(label: "名前", text: monsterRo.enemyName);
        monsterRo.hp = EditorGUILayout.IntField(label: "Hp", value: monsterRo.hp);
        monsterRo.mp = EditorGUILayout.IntField(label: "Mp", value: monsterRo.mp);
        monsterRo.ap = EditorGUILayout.IntField(label: "Ap", value: monsterRo.ap);
        monsterRo.dp = EditorGUILayout.IntField(label: "Dp", value: monsterRo.dp);
        monsterRo.map = EditorGUILayout.IntField(label: "Map", value: monsterRo.map);
        monsterRo.mdp = EditorGUILayout.IntField(label: "Mdp", value: monsterRo.mdp);
        monsterRo.sp = EditorGUILayout.IntField(label: "Sp", value: monsterRo.sp);
        monsterRo.lv = EditorGUILayout.IntField(label: "Lv", value: monsterRo.lv);
        monsterRo.dropExp = EditorGUILayout.IntField(label: "DropExp", value: monsterRo.dropExp);

        if (GUILayout.Button("追加"))
        {
            monsterRo = new MonsterRo();
            monsterRo.enemyName = "new Monster";
            ene_list.Add(monsterRo);
            m_PopupIndex = ene_list.Count - 1;
            DataSave();
            DataLoad();
        }
        if (GUILayout.Button("削除"))
        {
            ene_list.RemoveAt(m_PopupIndex);
            m_PopupIndex = 0;
            DataSave();
            DataLoad();
        }
        if (GUILayout.Button("セーブ"))
        {
            Debug.Log(checkNum + "" + monsterRo.enemyName);
            ene_list[checkNum] = monsterRo;
            DataSave();
            DataLoad();
        }
    }
}