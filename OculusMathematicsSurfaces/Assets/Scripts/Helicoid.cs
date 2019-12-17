using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Helicoid : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 80;//uの分割数
    int vSize = 80;//vの分割数
    public Vector3[] vertices;//頂点の列
    public int[] triangles;//三角形のデータ
    public int change;
    public bool sideA;
    //public GameObject CameraRig;
    //public Vector3 eyeHeight = Vector3.up;

    public float constA;//定数
    public float alpha;// 0 - pi/2
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];//頂点の座標
        triangles = new int[uSize * vSize * 6];//三角形の個数*3の長さの配列
        constA = 1.1f;

        InitVertices();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        InitMesh();

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
            InitVertices();
            InitMesh();

        }
        else if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            alpha -= 0.1f;
            if (alpha < 0f)
            {
                alpha = 0f;
            }
            InitVertices();
            InitMesh();
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            alpha += 0.1f;
            if (alpha > Mathf.PI/2f)
            {
                alpha = Mathf.PI / 2f;
            }
            InitVertices();
            InitMesh();
        }

    }

    void InitVertices()
    {
        for (int u = 0; u <= uSize; u++)
        {
            for (int v = 0; v <= vSize; v++)
            {
                float uu = -Mathf.PI / 2f + 2f * Mathf.PI * u / (uSize);
                float vv = -2f + 4f * v / (vSize);
                vertices[u * (vSize + 1) + v] = new Vector3(surfaceX(uu, vv), surfaceY(uu, vv), surfaceZ(uu, vv));
            }
        }
        for (int u = 0; u < uSize; u++)//三角形の頂点のデータを作っている
        {
            for (int v = 0; v < vSize; v++)
            {
                if (sideA) {
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

    void InitMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    readonly float E = 2.718281828459f;
    float Cosh(float x)
    {
        return (Mathf.Pow(E, x) + Mathf.Pow(E, -x)) * 0.5f;
    }
    float Sinh(float x)
    {
        return (Mathf.Pow(E, x) - Mathf.Pow(E, -x)) * 0.5f;
    }


    float surfaceX(float u, float v)
    {
        return constA * (Mathf.Cos(alpha) * Sinh(v) * Mathf.Sin(u) + Mathf.Sin(alpha) * Cosh(v) * Mathf.Cos(u));
    }

    float surfaceY(float u, float v)
    {
        return (constA * (u * Mathf.Cos(alpha) + v * Mathf.Sin(alpha)));
    }

    float surfaceZ(float u, float v)
    {
        return (constA * (-Mathf.Cos(alpha) * Sinh(v) * Mathf.Cos(u) + Mathf.Sin(alpha) * Cosh(v) * Mathf.Sin(u)));
    }
}
