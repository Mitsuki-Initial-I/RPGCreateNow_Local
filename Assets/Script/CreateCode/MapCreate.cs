using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    public GameObject[] prefabMapObjects;       // 設定するオブジェクト
    public Transform mother;                    // 生成位置
    public Vector3Int mapScale;                 // 生成座標
    public string fieldtName;                   // エリア名
    private GameObject fieldObject;             // エリアオブジェクト

    private void CreateItems()
    {
        fieldObject = new GameObject(fieldtName);
        fieldObject.transform.parent = mother;
        for (int i = 0; i < mapScale.y; i++)
        {
            for (int j = 0; j < mapScale.x; j++)
            {
                for (int k = 0; k < mapScale.z; k++)
                {
                    GameObject createMapObject = null;
                    if (prefabMapObjects.Length != 0)
                    {

                    }
                    else
                    {
                        createMapObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    }
                    createMapObject.transform.position = new Vector3(j, i, k);
                    createMapObject.transform.parent = fieldObject.transform;
                }
            }
        }
    }

    void MeshFusion()
    {
        // MeshFilterの取得
        MeshFilter[] meshFilters = fieldObject.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        // 設定
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        // 統合
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = combinedMesh;

        // 設定
        fieldObject.AddComponent<MeshFilter>().mesh = combinedMesh;
        fieldObject.AddComponent<MeshRenderer>().sharedMaterial = meshFilters[0].GetComponent<Renderer>().sharedMaterial;
        fieldObject.SetActive(true);
    }

    private void Start()
    {
        CreateItems();
        MeshFusion();
    }
}
