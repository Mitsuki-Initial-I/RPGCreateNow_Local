using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;

[ExecuteInEditMode]
public class CustomMeshGenerator : MonoBehaviour
{

    public Mesh mesh;
    public string meshObjectName;
    public Vector3[] vertices;
    public Vector3[] triangles;

    private MeshFilter meshFilter;
    private string defaultPath = "Assets/Mesh/";
    private void OnValidate()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
        }
    }

    /// <summary>
    /// メッシュの作成
    /// </summary>
    public void GenerateMesh()
    {
        if (vertices == null || vertices.Length < 3 || triangles == null || triangles.Length < 1)
        {
            Debug.Log("情報不足");
            return;
        }

        mesh = new Mesh();
        mesh.vertices = vertices;
        int[] triangleArray = new int[triangles.Length * 3];
        for (int i = 0; i < triangles.Length; i++)
        {
            triangleArray[i * 3] = (int)triangles[i].x;
            triangleArray[i * 3 + 1] = (int)triangles[i].y;
            triangleArray[i * 3 + 2] = (int)triangles[i].z;
        }

        mesh.triangles = triangleArray;
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        }

        Debug.Log("メッシュ作成");
    }

    /// <summary>
    /// 現在のメッシュを保存
    /// </summary>
    public void SaveMesh(bool allSave = false)
    {
        if (meshFilter.sharedMesh == null)
        {
            Debug.Log("メッシュがないです");
            return;
        }
        if (meshObjectName == "")
        {
            meshObjectName = gameObject.name;
        }

        string savePath = allSave ? $"{defaultPath}{meshObjectName}/{meshObjectName}.asset" :$"{defaultPath}{meshObjectName}.asset";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string[] nameSplit = savePath.Split('/');
        for (int i = 0; i < nameSplit.Length - 1; i++)
        {
            sb.Append(nameSplit[i] + "/");
        }
        savePath = sb.ToString();
        if (!Directory.Exists(savePath.TrimEnd()))
        {
            Debug.Log(savePath);
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }
        savePath += nameSplit[nameSplit.Length - 1];

        try
        {
            AssetDatabase.CreateAsset(meshFilter.sharedMesh, savePath);
            AssetDatabase.SaveAssets();
        }
        catch (System.Exception)
        {
            Debug.Log($"{meshObjectName}は存在してます");
        }
        
        Debug.Log($"{meshObjectName}を保存処理終了");
    }
    /// <summary>
    /// 現在の頂点データを保存
    /// </summary>
    public void SaveVertices(bool allSave = false)
    {
        string fileName = allSave ? $"{defaultPath}{meshObjectName}/Vertices_{meshObjectName}.csv"
            : $"{defaultPath}Vertices/Vertices_{System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}.csv";

        SaveCSV(vertices, fileName);
    }
    /// <summary>
    /// 現在の三角形データを保存
    /// </summary>
    public void SaveTriangles(bool allSave = false)
    {
        string fileName = allSave ? $"{defaultPath}{meshObjectName}/Triangles_{meshObjectName}.csv" 
            : $"{defaultPath}Triangles/Triangles_{System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss")}.csv";

        SaveCSV(triangles, fileName);
    }
    /// <summary>
    /// 頂点データと三角形データ保存用
    /// </summary>
    private void SaveCSV(Vector3[] date, string fileName)
    {

        if (date == null)
        {
            Debug.Log("情報がありません");
            return;
        }
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        string[] nameSplit = fileName.Split('/');
        for (int i = 0; i < nameSplit.Length-1; i++)
        {
            sb.Append(nameSplit[i] + "/");
        }
        fileName = sb.ToString();
        if (!Directory.Exists(fileName.TrimEnd()))
        {
            Debug.Log(fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
        }
        fileName += nameSplit[nameSplit.Length-1];
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("Index,x,y,z");
            for (int i = 0; i < date.Length; i++)
            {
                writer.WriteLine($"{i},{date[i].x},{date[i].y},{date[i].z}");
            }
        }
        Debug.Log($"{fileName}");
    }

    /// <summary>
    /// 頂点データと三角形データ読み込み用
    /// </summary>
    private Vector3[] LoadCSV(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Debug.Log("ファイルが見つかりません");
            return null;
        }

        string[] lines = File.ReadAllLines(fileName);
        Vector3[] date = new Vector3[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length == 4)
            {
                float x = float.Parse(values[1], CultureInfo.InvariantCulture);
                float y = float.Parse(values[2], CultureInfo.InvariantCulture);
                float z = float.Parse(values[3], CultureInfo.InvariantCulture);

                vertices[i - 1] = new Vector3(x, y, z);
            }
        }
        return date;
    }

    public void ExtractInformationFromMesh()
    {
        if (mesh == null)
        {
            Debug.Log("メッシュが設定されていません");
            return;
        }
        else
        {
            vertices = mesh.vertices;
            int[] triangleArray = mesh.triangles;
            triangles = new Vector3[triangleArray.Length / 3];
            for (int i = 0; i < triangles.Length; i++)
            {
                triangles[i].x = triangleArray[i * 3];
                triangles[i].y = triangleArray[i * 3 + 1];
                triangles[i].z = triangleArray[i * 3 + 2];
            }
            Debug.Log("再設定完了");
        }
    }

    [CustomEditor(typeof(CustomMeshGenerator))]
    public class CustomMeshGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            CustomMeshGenerator meshGenerator = (CustomMeshGenerator)target;

            if (GUILayout.Button("生成"))
            {
                meshGenerator.GenerateMesh();
            }
            if (GUILayout.Button("変換"))
            {
                meshGenerator.ExtractInformationFromMesh();
            }

            if (GUILayout.Button("全て保存"))
            {
                meshGenerator.SaveMesh(true);
                meshGenerator.SaveVertices(true);
                meshGenerator.SaveTriangles(true);
            }
            if (GUILayout.Button("メッシュの保存"))
            {
                meshGenerator.SaveMesh(false);
            }
            if (GUILayout.Button("頂点データの保存"))
            {
                meshGenerator.SaveVertices(false);
            }
            if (GUILayout.Button("三角形の保存"))
            {
                meshGenerator.SaveTriangles(false);
            }
        }
    }
}