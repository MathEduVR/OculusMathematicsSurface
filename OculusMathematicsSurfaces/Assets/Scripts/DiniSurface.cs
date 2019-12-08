using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiniSurface : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 80;//uの分割数
    int vSize = 80;//vの分割数
    public Vector3[] vertices;//頂点の列
    public int[] triangles;//三角形のデータ
    public int change;
    public bool sideA;

    public float constA;//定数
    public float constB;// 0 - .5
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];//頂点の座標
        triangles = new int[uSize * vSize * 6];//三角形の個数*3の長さの配列
        constA = 1.1f;

        init_vertices();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        init_mesh();

        change = 0;
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
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            change = 1 - change;
            init_vertices();
            init_mesh();

        }
        else if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            constB -= 0.025f;
            if (constB < 0f)
            {
                constB = 0f;
            }
            init_vertices();
            init_mesh();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            constB += 0.025f;
            if (constB > .3f)
            {
                constB = .3f;
            }
            init_vertices();
            init_mesh();
        }

    }

    void init_vertices()
    {
        for (int u = 0; u <= uSize; u++)
        {
            for (int v = 0; v <= vSize; v++)
            {
                float uu = 4f * Mathf.PI * u / (uSize);
                float vv = 0.02f + (Mathf.PI * 0.5f - 0.02f) * v / (vSize);
                vertices[u * (vSize + 1) + v] = new Vector3(surfaceX(uu, vv), surfaceY(uu, vv) + 2f, surfaceZ(uu, vv));
            }
        }
        for (int u = 0; u < uSize; u++)//三角形の頂点のデータを作っている
        {
            for (int v = 0; v < vSize; v++)
            {
                if (sideA)
                {
                    triangles[6 * (u * vSize + v) + 0] = u * (vSize + 1) + v;
                    triangles[6 * (u * vSize + v) + 1] = (u + 1) * (vSize + 1) + v;
                    triangles[6 * (u * vSize + v) + 2] = u * (vSize + 1) + (v + 1);
                    if (change == 0)
                    {
                        triangles[6 * (u * vSize + v) + 3] = u * (vSize + 1) + (v + 1);
                        triangles[6 * (u * vSize + v) + 4] = (u + 1) * (vSize + 1) + v;
                        triangles[6 * (u * vSize + v) + 5] = (u + 1) * (vSize + 1) + (v + 1);
                    }
                    else
                    {
                        triangles[6 * (u * vSize + v) + 3] = 0;
                        triangles[6 * (u * vSize + v) + 4] = 0;
                        triangles[6 * (u * vSize + v) + 5] = 0;

                    }
                }
                else
                {
                    triangles[6 * (u * vSize + v) + 0] = u * (vSize + 1) + v;
                    triangles[6 * (u * vSize + v) + 2] = (u + 1) * (vSize + 1) + v;
                    triangles[6 * (u * vSize + v) + 1] = u * (vSize + 1) + (v + 1);
                    if (change == 0)
                    {
                        triangles[6 * (u * vSize + v) + 3] = u * (vSize + 1) + (v + 1);
                        triangles[6 * (u * vSize + v) + 5] = (u + 1) * (vSize + 1) + v;
                        triangles[6 * (u * vSize + v) + 4] = (u + 1) * (vSize + 1) + (v + 1);
                    }
                    else
                    {
                        triangles[6 * (u * vSize + v) + 3] = 0;
                        triangles[6 * (u * vSize + v) + 4] = 0;
                        triangles[6 * (u * vSize + v) + 5] = 0;

                    }
                }
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
        return constA * (Mathf.Cos(u) * Mathf.Sin(v));
    }

    float surfaceY(float u, float v)
    {
        return constA * (Mathf.Cos(v) + Mathf.Log( Mathf.Tan(v*0.5f)))+constB * u;
    }

    float surfaceZ(float u, float v)
    {
        return constA * (Mathf.Sin(u) * Mathf.Sin(v));
    }
}
