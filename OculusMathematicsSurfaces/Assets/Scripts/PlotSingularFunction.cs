using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotSingularFunction : MonoBehaviour
{
    public Vector3[] Vertices;
    public int[] Triangles;
    public int DivR, DivT;
    float MaxR, MinR, MaxT, MinT, stepR, stepT;

    public Mesh MyMesh;
    public bool GraphA;

    public int GraphNumber;

    public GameObject CameraRig;
    public Vector3 eyeHeight = Vector3.up;


    void Start()
    {
        MaxR = 3f;
        if (GraphNumber == 3) MaxR = 1.5f;
        MinR = 0f;
        MaxT = Mathf.PI * 2f;
        MinT = 0f;
        DivR = 80;
        DivT = 40;
        stepR = MaxR / DivR;
        stepT = MaxT / DivT;
        MyMesh = new Mesh();
        MakeMeshData();

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }

    void MakeMeshData()
    {
        Vertices = new Vector3[(DivR + 1) * (DivT + 1)];
        Triangles = new int[DivR * DivT * 6];
        for(int r = 0; r <= DivR; r++)
        {
            float rr = MinR + stepR * r;
            for (int t = 0; t <= DivT; t++)
            {
                float tt = MinT + stepT * t;
                float x = rr * Mathf.Cos(tt);
                float y = rr * Mathf.Sin(tt);
                float z = Func(rr, tt);
                if (GraphNumber == 2)
                {
                    if (x < 0) x = -Mathf.Sqrt(-x);
                    else x = Mathf.Sqrt(x);

                }
                Vertices[t + r * (DivT + 1)] = new Vector3(x, z, y);
            }
        }
        for (int r = 0; r < DivR; r++)
        {
            int r1 = r + 1;
            for (int t = 0; t < DivT; t++)
            {
                int t1 = t + 1;
                if (GraphA) {
                    Triangles[(t + r * DivT) * 6 + 0] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 1] = t1 + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 2] = t + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 3] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 4] = t1 + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 5] = t1 + r1 * (DivT + 1);
                }
                else
                {
                    Triangles[(t + r * DivT) * 6 + 0] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 1] = t + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 2] = t1 + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 3] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 4] = t1 + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 5] = t1 + r * (DivT + 1);
                }
            }
        }
        MyMesh.vertices = Vertices;
        MyMesh.triangles = Triangles;
        MyMesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = MyMesh;
    }

    float Func(float rr,float tt)
    {
        float x = rr * Mathf.Cos(tt);
        float y = rr * Mathf.Sin(tt);
        switch (GraphNumber)
        {
            case 0:
                if (x * x + y * y == 0f)
                {
                    float xx = Mathf.Cos(tt);
                    float yy = Mathf.Sin(tt);
                    return xx * yy;
                }
                return (x * y) / (x * x + y * y);
            case 1:
                if (x * x + y * y == 0f)
                {
                    return 0;
                }
                return (0.2f * x * x * x + 0.1f * x * x * y + 0.4f * y * y * y) / (x * x + y * y);
            case 2:
                if (x * x + y * y == 0f)
                {
                    float xx = Mathf.Cos(tt);
                    float yy = Mathf.Sin(tt);
                    return Mathf.Abs(xx) * yy;
                }
                return (Mathf.Abs(x) * y) / (x * x + y * y);
            case 3:
                return (x * x * x - x * x + y * y) * 0.5f;
            default:
                if (x * x + y * y == 0f)
                {
                    float xx = Mathf.Cos(tt);
                    float yy = Mathf.Sin(tt);
                    return xx * yy;
                }
                return (0.2f * x * x * x + x * y + 0.4f * y * y * y) / (x * x + y * y);
        }
    }


}
