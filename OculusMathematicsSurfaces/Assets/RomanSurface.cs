using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RomanSurface : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 80;//uの分割数
    int vSize = 80;//vの分割数
    public Vector3[] vertices;//頂点の列
    public int[] triangles;//三角形のデータ



    public float constA;//定数
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];//頂点の座標
        triangles = new int[uSize * vSize * 6];//三角形の個数*3の長さの配列
        constA = 1f;

        init_vertices();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        init_mesh();

    }

    // Update is called once per frame
    void Update()
    {
        UpdateButton();
    }

    void UpdateButton()
    {
       if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }

    void init_vertices()
    {
        for (int u = 0; u <= uSize; u++)
        {
            for (int v = 0; v <= vSize; v++)
            {
                float uu = 2f * Mathf.PI * u / (uSize);
                float vv = -Mathf.PI/2f+ Mathf.PI * v / (vSize);
                vertices[u * (vSize + 1) + v] = new Vector3(surfaceX(uu, vv), surfaceY(uu, vv), surfaceZ(uu, vv));
            }
        }
        for (int u = 0; u < uSize; u++)//三角形の頂点のデータを作っている
        {
            for (int v = 0; v < vSize; v++)
            {
                triangles[6 * (u * vSize + v) + 0] = u * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 1] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 2] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 3] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 5] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 4] = (u + 1) * (vSize + 1) + (v + 1);
            }
        }

    }

    void init_mesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    float surfaceX(float u, float v)
    {
        return (constA * Mathf.Sin(2*u)*Mathf.Sin(v)*Mathf.Sin(v));
    }

    float surfaceY(float u, float v)
    {
        return (constA * Mathf.Sin(u) * Mathf.Cos(2*v) );
    }

    float surfaceZ(float u, float v)
    {
        return (constA * Mathf.Cos(u) * Mathf.Sin(2*v) );
    }
}
