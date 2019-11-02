using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trefoil : MonoBehaviour
{
    public Mesh mesh = null;
    readonly int meridian = 12;
    public Vector3[] vertices;
    public int[] triangles;
    public GameObject CameraRig;
    public Vector3 eyeHeight = Vector3.up;

    Vector3[] center;
    Vector3[] verocity;
    Vector3[] curvature;
    Vector3[] torsion;
    // Start is called before the first frame update

    Vector3[] crossing;


    readonly float EdgeLength = 0.5f;
    readonly float EdgeWeight = 0.03f;
    Vector3 Vx, Vy, Vz;
    Vector3 CeilingHeight = new Vector3(0f, 0f, -1f);
    int countVertex = 0;
    int countFace = 0;
    int countCenter = 0;
    int sizeVertex = 0;
    int sizeFace = 0;
    int sizeCenter = 0;

    readonly int countPart2 = 40;

    Quaternion Rot = Quaternion.identity;

    void Start()
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name.Contains("OVRCameraRig"))
            {
                CameraRig = objs[i];
            }
        }

        Vx = EdgeLength * (new Vector3(1f,0f,0f));
        Vy = EdgeLength * (new Vector3(Mathf.Cos(Mathf.PI / 3f),Mathf.Sin(Mathf.PI/3f),0));
        Vz = EdgeLength * (new Vector3(Mathf.Cos(2f * Mathf.PI / 3f), Mathf.Sin(2f * Mathf.PI / 3f), 0));
        sizeVertex = 3 * meridian * (2 + (countPart2 + 1) + (countPart2 + 1) + (countPart2 + 1));
        sizeFace = 3 * meridian * 6 * (1 + countPart2 + countPart2 + countPart2);
        sizeCenter = 3 * (2 + (countPart2 + 1) + (countPart2 + 1) + (countPart2 + 1));
        init_sequence();
        countVertex = countFace = countCenter = 0;

        crossing = new Vector3[countPart2+1];
        for(int i = 0; i<=countPart2; i++)
        {
            crossing[i] = new Vector3(0f, 0f, 0.3f * Mathf.Sin(Mathf.PI * i / countPart2)* Mathf.Sin(Mathf.PI * i / countPart2));
        }

        Rot = Quaternion.Euler(0f,0f,120f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);
        Rot = Quaternion.Euler(0f, 0f, 0f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);
        Rot = Quaternion.Euler(0f, 0f, -120f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);


        set_mesh();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init_sequence()
    {
        center = new Vector3[sizeCenter];
        verocity = new Vector3[sizeCenter];
        curvature = new Vector3[sizeCenter];
        torsion = new Vector3[sizeCenter];
        mesh = new Mesh();
        vertices = new Vector3[sizeVertex];
        triangles = new int[sizeFace];
    }

    void MakePart1(Quaternion rot)
    {
        // part1 その１
        center[countCenter + 0] = CeilingHeight + (-Vz);
        center[countCenter + 1] = CeilingHeight + (Vx);
        verocity[countCenter + 0] = center[countCenter + 1] - center[countCenter + 0];
        verocity[countCenter + 0].Normalize();
        curvature[countCenter + 0] = GetNormal(verocity[countCenter + 0]);
        curvature[countCenter + 0].Normalize();
        torsion[countCenter + 0] = Vector3.Cross(verocity[countCenter + 0], curvature[countCenter + 0]);
        torsion[countCenter + 0].Normalize();
        for (int i = 0; i < meridian; i++)
        {
            vertices[countVertex + i] =
                rot * (center[countCenter + 0]
                + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * i / meridian) * curvature[countCenter + 0]
                + EdgeWeight * Mathf.Sin(Mathf.PI * 2f * i / meridian) * torsion[countCenter + 0]);
            //Debug.Log(vertices[i].x + "," + vertices[i].y + "," + vertices[i].z);
            int j = countVertex + i + meridian;
            vertices[j] =
                rot * (center[countCenter + 1]
                + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * i / meridian) * curvature[countCenter + 0]
                + EdgeWeight * Mathf.Sin(Mathf.PI * 2f * i / meridian) * torsion[countCenter + 0]);
            //Debug.Log(vertices[j].x + "," + vertices[j].y + "," + vertices[j].z);
            //Debug.Log(center[1] + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * i / meridian) * curvature[0]);
        }
        for (int i = 0; i < meridian; i++)
        {
            triangles[countFace + 6 * i + 0] = countVertex + i;
            triangles[countFace + 6 * i + 2] = countVertex + meridian + i;
            triangles[countFace + 6 * i + 1] = countVertex + meridian + (i + 1) % meridian;
            triangles[countFace + 6 * i + 3] = countVertex + i;
            triangles[countFace + 6 * i + 5] = countVertex + meridian + (i + 1) % meridian;
            triangles[countFace + 6 * i + 4] = countVertex + (i + 1) % meridian;
        }
        countCenter += 2;
        countVertex += 2 * meridian;
        countFace += 6 * meridian;
    }

    void MakePart2(Quaternion rot)
    {
        Vector3 v1 = CeilingHeight + Vx;
        Vector3 v2 = CeilingHeight + Vx + Vy;
        Vector3 v3 = 2 * Vy + Vx;
        Vector3 v4 = 3 * Vy;
        for (int i = 0; i <= countPart2; i++)
        {
            center[countCenter + i] =
                CubicBezier(v1, v2, v3, v4, (1f * i / countPart2)) - crossing[i];
        }
        for (int i = 0; i < countPart2; i++)
        {
            verocity[countCenter + i] = center[countCenter + i + 1] - center[countCenter + i];
            verocity[countCenter + i].Normalize();
        }
        verocity[countCenter + countPart2] = verocity[countCenter + countPart2 - 1];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            curvature[countCenter + i] = verocity[countCenter + i + 1] - verocity[countCenter + i];
            curvature[countCenter + i].Normalize();
        }
        curvature[countCenter + countPart2] = curvature[countCenter + countPart2 - 1] = curvature[countCenter + countPart2 - 2];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            torsion[countCenter + i] = Vector3.Cross(verocity[countCenter + i], curvature[countCenter + i]);
            torsion[countCenter + i].Normalize();
        }
        torsion[countCenter + countPart2] = torsion[countCenter + countPart2 - 1] = torsion[countCenter + countPart2 - 2];

        for (int i = 0; i <= countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                vertices[countVertex + i * meridian + j] = 
                    rot * (center[countCenter + i]
                    + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * j / meridian) * curvature[countCenter + i]
                    + EdgeWeight * Mathf.Sin(Mathf.PI * 2f * j / meridian) * torsion[countCenter + i]);
                //Debug.Log(countVertex + i * meridian + j);
            }
        }
        for (int i = 0; i < countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                triangles[countFace + (i * meridian + j) * 6 + 0] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 2] = countVertex + (i+1) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 1] = countVertex + (i+1) * meridian + (j+1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 3] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 5] = countVertex + (i+1) * meridian + (j+1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 4] = countVertex + (i) * meridian + (j+1) % meridian;
            }
        }
        countCenter += (countPart2 + 1);
        countVertex += (countPart2 + 1) * meridian;
        countFace += countPart2 * meridian * 6;
    }

    void MakePart3(Quaternion rot)
    {
        Vector3 v1 = 3 * Vy;
        Vector3 v2 = 3 * Vy + Vz;
        Vector3 v3 = 3 * Vz + Vy;
        Vector3 v4 = 3 * Vz;
        for (int i = 0; i <= countPart2; i++)
        {
            center[countCenter + i] = CubicBezier(v1, v2, v3, v4, (1f * i / countPart2));
        }
        for (int i = 0; i < countPart2; i++)
        {
            verocity[countCenter + i] = center[countCenter + i + 1] - center[countCenter + i];
            verocity[countCenter + i].Normalize();
        }
        verocity[countCenter + countPart2] = verocity[countCenter + countPart2 - 1];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            curvature[countCenter + i] = verocity[countCenter + i + 1] - verocity[countCenter + i];
            curvature[countCenter + i].Normalize();
        }
        curvature[countCenter + countPart2] = curvature[countCenter + countPart2 - 1] = curvature[countCenter + countPart2 - 2];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            torsion[countCenter + i] = Vector3.Cross(verocity[countCenter + i], curvature[countCenter + i]);
            torsion[countCenter + i].Normalize();
        }
        torsion[countCenter + countPart2] = torsion[countCenter + countPart2 - 1] = torsion[countCenter + countPart2 - 2];

        for (int i = 0; i <= countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                vertices[countVertex + i * meridian + j] =
                    rot * (center[countCenter + i]
                    + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * j / meridian) * curvature[countCenter + i]
                    + EdgeWeight * Mathf.Sin(Mathf.PI * 2f * j / meridian) * torsion[countCenter + i]);
                //Debug.Log(countVertex + i * meridian + j);
            }
        }
        for (int i = 0; i < countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                triangles[countFace + (i * meridian + j) * 6 + 0] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 2] = countVertex + (i + 1) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 1] = countVertex + (i + 1) * meridian + (j + 1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 3] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 5] = countVertex + (i + 1) * meridian + (j + 1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 4] = countVertex + (i) * meridian + (j + 1) % meridian;
            }
        }
        countCenter += (countPart2 + 1);
        countVertex += (countPart2 + 1) * meridian;
        countFace += countPart2 * meridian * 6;
    }

    void MakePart4(Quaternion rot)
    {
        Vector3 v1 = 3 * Vz;
        Vector3 v2 = 2 * Vz - Vx;
        Vector3 v3 = CeilingHeight + Vz - Vx;
        Vector3 v4 = CeilingHeight - Vx;
        for (int i = 0; i <= countPart2; i++)
        {
            center[countCenter + i] = CubicBezier(v1, v2, v3, v4, (1f * i / countPart2)) + crossing[i];
        }
        for (int i = 0; i < countPart2; i++)
        {
            verocity[countCenter + i] = center[countCenter + i + 1] - center[countCenter + i];
            verocity[countCenter + i].Normalize();
        }
        verocity[countCenter + countPart2] = verocity[countCenter + countPart2 - 1];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            curvature[countCenter + i] = verocity[countCenter + i + 1] - verocity[countCenter + i];
            curvature[countCenter + i].Normalize();
        }
        curvature[countCenter + countPart2] = curvature[countCenter + countPart2 - 1] = curvature[countCenter + countPart2 - 2];
        for (int i = 0; i < countPart2 - 1; i++)
        {
            torsion[countCenter + i] = Vector3.Cross(verocity[countCenter + i], curvature[countCenter + i]);
            torsion[countCenter + i].Normalize();
        }
        torsion[countCenter + countPart2] = torsion[countCenter + countPart2 - 1] = torsion[countCenter + countPart2 - 2];

        for (int i = 0; i <= countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                vertices[countVertex + i * meridian + j] =
                    rot * (center[countCenter + i]
                    + EdgeWeight * Mathf.Cos(Mathf.PI * 2f * j / meridian) * curvature[countCenter + i]
                    + EdgeWeight * Mathf.Sin(Mathf.PI * 2f * j / meridian) * torsion[countCenter + i]);
                //Debug.Log(countVertex + i * meridian + j);
            }
        }
        for (int i = 0; i < countPart2; i++)
        {
            for (int j = 0; j < meridian; j++)
            {
                triangles[countFace + (i * meridian + j) * 6 + 0] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 2] = countVertex + (i + 1) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 1] = countVertex + (i + 1) * meridian + (j + 1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 3] = countVertex + (i) * meridian + (j);
                triangles[countFace + (i * meridian + j) * 6 + 5] = countVertex + (i + 1) * meridian + (j + 1) % meridian;
                triangles[countFace + (i * meridian + j) * 6 + 4] = countVertex + (i) * meridian + (j + 1) % meridian;
            }
        }
        countCenter += (countPart2 + 1);
        countVertex += (countPart2 + 1) * meridian;
        countFace += countPart2 * meridian * 6;
    }

    Vector3 CubicBezier(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float t)
    {
        float u = 1f - t;
        Vector3 v12 = v1 * u + v2 * t;
        Vector3 v23 = v2 * u + v3 * t;
        Vector3 v34 = v3 * u + v4 * t;
        Vector3 f1223 = v12 * u + v23 * t;
        Vector3 f2334 = v23 * u + v34 * t;
        return f1223 * u + f2334 * t;
    }


    Vector3 GetNormal(Vector3 v)
    {
        if (v.x==0f && v.y == 0f)
        {
            return Vector3.forward;
        }
        else
        {
            return new Vector3(-v.y, v.x, 0f);
        }
    }

    void set_mesh()
    {
        if (mesh == null) mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

}
