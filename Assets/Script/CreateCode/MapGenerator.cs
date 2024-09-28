using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int chunkSize = 16; // 各チャンクのサイズ
    public int mapWidth = 4;   // 横に並ぶチャンク数
    public int mapHeight = 4;  // 縦に並ぶチャンク数
    public float noiseScale = 20f; // ノイズのスケール

    private Dictionary<Vector2Int, GameObject> chunks = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        GenerateMap();
    }

    // マップ全体を生成
    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapHeight; z++)
            {
                GenerateChunk(new Vector2Int(x, z));
            }
        }

        CombineAllChunks();
    }

    // 各チャンクを生成する
    void GenerateChunk(Vector2Int chunkCoord)
    {
        GameObject chunkObject = new GameObject("Chunk_" + chunkCoord.x + "_" + chunkCoord.y);
        chunkObject.transform.parent = transform; // 空オブジェクトの子に設定
        chunkObject.transform.position = new Vector3(chunkCoord.x * chunkSize, 0, chunkCoord.y * chunkSize);

        MeshFilter meshFilter = chunkObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        Mesh mesh = GenerateTerrainMesh(chunkCoord);
        meshFilter.mesh = mesh;

        // 当たり判定用の MeshCollider を追加し、メッシュを設定
        MeshCollider meshCollider = chunkObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        chunks.Add(chunkCoord, chunkObject);
    }

    // プロシージャルに地形メッシュを生成
    Mesh GenerateTerrainMesh(Vector2Int chunkCoord)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // 各頂点の高さをノイズ関数で生成
        for (int x = 0; x <= chunkSize; x++)
        {
            for (int z = 0; z <= chunkSize; z++)
            {
                float y = Mathf.PerlinNoise(
                    (chunkCoord.x * chunkSize + x) / noiseScale,
                    (chunkCoord.y * chunkSize + z) / noiseScale) * 5f;

                vertices.Add(new Vector3(x, y, z));
            }
        }

        // 三角形インデックスを設定
        for (int x = 0; x < chunkSize; x++)
        {
            for (int z = 0; z < chunkSize; z++)
            {
                int start = x * (chunkSize + 1) + z;
                triangles.Add(start);
                triangles.Add(start + 1);
                triangles.Add(start + chunkSize + 1);

                triangles.Add(start + 1);
                triangles.Add(start + chunkSize + 2);
                triangles.Add(start + chunkSize + 1);
            }
        }

        // メッシュの設定
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    // 全てのチャンクを統合して描画負荷を軽減
    void CombineAllChunks()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int index = 0;
        foreach (var filter in meshFilters)
        {
            if (filter.transform == transform) continue;

            combine[index].mesh = filter.sharedMesh;
            combine[index].transform = filter.transform.localToWorldMatrix;
            filter.gameObject.SetActive(false);
            index++;
        }

        // メッシュを統合して新しいメッシュを作成
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // 新しいメッシュフィルターとレンダラーを作成
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = meshFilters[1].GetComponent<Renderer>().sharedMaterial;
        meshFilter.mesh = combinedMesh;

        // 統合されたメッシュにも MeshCollider を追加して当たり判定を設定
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        gameObject.SetActive(true);
    }
}
