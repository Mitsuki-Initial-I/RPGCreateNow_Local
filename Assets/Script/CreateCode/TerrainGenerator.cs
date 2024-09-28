using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[width * height];

        // 各頂点の高さをノイズ関数で計算
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float z = Mathf.PerlinNoise(x * scale, y * scale) * 10f;
                vertices[y * width + x] = new Vector3(x, z, y);
            }
        }

        // メッシュの頂点を設定
        mesh.vertices = vertices;
        // 三角形の設定とその他の処理を追加

        //gameObject.AddComponent<MeshFilter>().mesh = mesh;
        //gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    }
}
