using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnneperSurface : MonoBehaviour { 
    public Vector3[] Vertices;
public int[] Triangles;
public int DivR, DivT;
float MaxR, MinR, MaxT, MinT, stepR, stepT;

public Mesh MyMesh;
public bool GraphA;
public int change;
//public int GraphNumber;

public GameObject CameraRig;
public Vector3 eyeHeight = Vector3.up;
    float constA = 0.3f;


void Start()
{
    MaxR = 3f;
    MinR = 0f;
    MaxT = Mathf.PI ;
    MinT = -Mathf.PI;
    DivR = 40;
    DivT = 120;
    stepR = (MaxR-MinR) / DivR;
    stepT = (MaxT-MinT) / DivT;
    MyMesh = new Mesh();
    change = 0;
    MakeMeshData();

}

// Update is called once per frame
void Update()
{
    if (OVRInput.GetDown(OVRInput.Button.Start))
    {
        SceneManager.LoadScene("Scenes/Main");
    }
    else if (OVRInput.GetDown(OVRInput.Button.Two))
    {
        change = 1 - change;
        MakeMeshData();
    }
}

void MakeMeshData()
{
    Vertices = new Vector3[(DivR + 1) * (DivT + 1)];
    Triangles = new int[DivR * DivT * 6];
    for (int r = 0; r <= DivR; r++)
    {
        float rr = MinR + stepR * r;
        for (int t = 0; t <= DivT; t++)
        {
            float tt = MinT + stepT * t;
            Vertices[t + r * (DivT + 1)] = Func(rr, tt);
        }
    }
    for (int r = 0; r < DivR; r++)
    {
        int r1 = r + 1;
        for (int t = 0; t < DivT; t++)
        {
            int t1 = t + 1;
            if (GraphA)
            {
                Triangles[(t + r * DivT) * 6 + 0] = t + r * (DivT + 1);
                Triangles[(t + r * DivT) * 6 + 1] = t1 + r1 * (DivT + 1);
                Triangles[(t + r * DivT) * 6 + 2] = t + r1 * (DivT + 1);
                if (change == 0)
                {
                    Triangles[(t + r * DivT) * 6 + 3] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 4] = t1 + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 5] = t1 + r1 * (DivT + 1);
                }
                else
                {
                    Triangles[(t + r * DivT) * 6 + 3] = 0;
                    Triangles[(t + r * DivT) * 6 + 5] = 0;
                    Triangles[(t + r * DivT) * 6 + 4] = 0;
                }
            }
            else
            {
                Triangles[(t + r * DivT) * 6 + 0] = t + r * (DivT + 1);
                Triangles[(t + r * DivT) * 6 + 1] = t + r1 * (DivT + 1);
                Triangles[(t + r * DivT) * 6 + 2] = t1 + r1 * (DivT + 1);
                if (change == 0)
                {
                    Triangles[(t + r * DivT) * 6 + 3] = t + r * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 4] = t1 + r1 * (DivT + 1);
                    Triangles[(t + r * DivT) * 6 + 5] = t1 + r * (DivT + 1);
                }
                else
                {
                    Triangles[(t + r * DivT) * 6 + 3] = 0;
                    Triangles[(t + r * DivT) * 6 + 5] = 0;
                    Triangles[(t + r * DivT) * 6 + 4] = 0;
                }
            }
        }
    }
    MyMesh.vertices = Vertices;
    MyMesh.triangles = Triangles;
    MyMesh.RecalculateNormals();
    GetComponent<MeshFilter>().mesh = MyMesh;
}

Vector3 Func(float rr, float tt)
{
    float x =constA*( rr*Mathf.Cos(tt)-1f/3f*rr*rr*rr*Mathf.Cos(3f*tt));
    float y =constA*( -rr * Mathf.Sin(tt) - 1f / 3f * rr * rr * rr * Mathf.Sin(3f * tt));
    float z =constA*( rr*rr*Mathf.Cos(2f*tt));

    return new Vector3(x, z, y);
}
}

