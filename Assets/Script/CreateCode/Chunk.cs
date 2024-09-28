using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chunk : MonoBehaviour
{
    public int chunkSize = 16;  // 各チャンクのサイズ

    void Start()
    {
        GenerateChunk();
    }

    void GenerateChunk()
    {
        // メッシュデータの作成
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // チャンク内の立方体を作成する（例: ボクセルワールド）
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    // 各座標に立方体を生成（例: メッシュを設定する処理）
                    CreateCube(vertices, triangles, new Vector3(x, y, z));
                }
            }
        }

        // メッシュの設定
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        // メッシュフィルターとレンダラーの設定
        //gameObject.AddComponent<MeshFilter>().mesh = mesh;
        //gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }

    void CreateCube(List<Vector3> vertices, List<int> triangles, Vector3 position)
    {
        // 各面の座標設定（例）
        vertices.Add(position + new Vector3(0, 0, 0));  // 顔の頂点
        vertices.Add(position + new Vector3(1, 0, 0));
        vertices.Add(position + new Vector3(1, 1, 0));
        vertices.Add(position + new Vector3(0, 1, 0));
        // 他の面も同様に追加

        // 三角形のインデックス設定（例）
        int vertIndex = vertices.Count - 4;
        triangles.Add(vertIndex);
        triangles.Add(vertIndex + 1);
        triangles.Add(vertIndex + 2);
        // 他の面も同様に設定
    }
}
