using UnityEngine;
public class PerlinNoiseSample : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] float m_threshold = 0.5f;
    [SerializeField, Range(0.05f, 0.3f)] float m_scale = 0.1f;
    int sz = 50;

    void Start()
    {
        createCustomCube(Vector3.zero, new Vector3(sz, 0.1f, sz), new Color(0.4f, 0.3f, 0.9f));
        for (int x = 0; x < sz; ++x)
        {
            for (int z = 0; z < sz; ++z)
            {
                float noise = Mathf.PerlinNoise((float)x * m_scale, (float)z * m_scale);
                if (noise < m_threshold)
                {
                    float thRate = (m_threshold - noise) / (m_threshold);
                    createCustomCube(new Vector3(-sz * 0.5f + x, 0f, -sz * 0.5f + z), new Vector3(1f, thRate * 10f, 1f), new Color(1f - thRate * 1.1f, 0.8f + thRate * 0.3f, 0.9f - thRate * 1.2f));
                }
            }
        }
    }

    void Update() { }

    GameObject createCustomCube(Vector3 _position, Vector3 _scale, Color _color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.SetParent(transform);
        go.transform.localPosition = transform.position + _position;
        go.transform.localScale = _scale;
        go.GetComponent<MeshRenderer>().material.color = _color;
        return go;
    }
}
