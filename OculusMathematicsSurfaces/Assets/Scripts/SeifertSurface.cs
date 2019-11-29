using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SeifertSurface : MonoBehaviour
{
    public bool SurfaceA;
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    Vector3[] crossing;

    int sizeVertex = 0, sizeFace = 0;
    int countVertex = 0, countFace = 0;
    readonly int countPart2 = 40;

    public Material SeifertSurfaceMaterial;

    Vector3 Vx, Vy, Vz;
    Vector3 CeilingHeight = new Vector3(0f, 0f, -1f);
    readonly float EdgeLength = 0.5f;
    public GameObject CameraRig;
    public Vector3 eyeHeight = Vector3.up;

    // Start is called before the first frame update
    void Start()
    {
        countVertex = 0;
        countFace = 0;

        sizeVertex = 3 * (4 + (2 * countPart2 + 2) + 3 + (countPart2 + 2));
        sizeFace = 3 * (6 + (countPart2 * 6) + 3 + (countPart2 * 3));

        vertices = new Vector3[sizeVertex];
        triangles = new int[sizeFace];

        Vx = EdgeLength * (new Vector3(1f, 0f, 0f));
        Vy = EdgeLength * (new Vector3(Mathf.Cos(Mathf.PI / 3f), Mathf.Sin(Mathf.PI / 3f), 0));
        Vz = EdgeLength * (new Vector3(Mathf.Cos(2f * Mathf.PI / 3f), Mathf.Sin(2f * Mathf.PI / 3f), 0));

        crossing = new Vector3[countPart2 + 1];
        for (int i = 0; i <= countPart2; i++)
        {
            crossing[i] = new Vector3(0f, 0f, 0.3f * Mathf.Sin(Mathf.PI * i / countPart2) * Mathf.Sin(Mathf.PI * i / countPart2));
        }

        Quaternion Rot = Quaternion.Euler(0f, 0f, 0f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);
        Rot = Quaternion.Euler(0f, 0f, -120f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);
        Rot = Quaternion.Euler(0f, 0f, 120f);
        MakePart1(Rot);
        MakePart2(Rot);
        MakePart3(Rot);
        MakePart4(Rot);



        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = SeifertSurfaceMaterial;

        GameObject[] objs = FindObjectsOfType<GameObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name.Contains("OVRCameraRig"))
            {
                CameraRig = objs[i];
            }
        }
        CameraRig.transform.localPosition = eyeHeight;

    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp))
        //{
        //    eyeHeight.y += 0.02f;
        //    CameraRig.transform.localPosition = eyeHeight;
        //}
        //else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown))
        //{
        //    eyeHeight.y -= 0.02f;
        //    if (eyeHeight.y < 0.2f) eyeHeight.y = 0.2f;
        //    CameraRig.transform.localPosition = eyeHeight;
        //}
        //else 
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }

    void MakePart1(Quaternion rot)
    {
        vertices[countVertex + 0] = rot * (CeilingHeight);
        vertices[countVertex + 1] = rot * (CeilingHeight +Vx);
        vertices[countVertex + 2] = rot * (CeilingHeight +Vy);
        vertices[countVertex + 3] = rot * (CeilingHeight +Vz);

        triangles[countFace + 0] = countVertex + 0;
        triangles[countFace + 1] = (SurfaceA) ? countVertex + 1 : countVertex + 2;
        triangles[countFace + 2] = (SurfaceA) ? countVertex + 2 : countVertex + 1;
        triangles[countFace + 3] = countVertex + 0;
        triangles[countFace + 4] = (SurfaceA) ? countVertex + 2 : countVertex + 3;
        triangles[countFace + 5] = (SurfaceA) ? countVertex + 3 : countVertex + 2;

        countVertex += 4;
        countFace += 6;
    }


    void MakePart2(Quaternion rot)
    {
        Vector3 v1 = CeilingHeight + Vx;
        Vector3 v2 = CeilingHeight + Vx + Vy;
        Vector3 v3 = 2 * Vy + Vx;
        Vector3 v4 = 3 * Vy;
        for (int i = 0; i <= countPart2; i++)
        {
            vertices[countVertex + i] =
                rot * (CubicBezier(v1, v2, v3, v4, (1f * i / countPart2)) - crossing[i]);
        }
         v1 = CeilingHeight + Vy;
         v2 = CeilingHeight + Vx + Vy;
         v3 = 2 * Vx + Vy;
         v4 = 3 * Vx;
        for (int i = 0; i <= countPart2; i++)
        {
            vertices[countVertex + (countPart2 + 1) + i] =
                rot * (CubicBezier(v1, v2, v3, v4, (1f * i / countPart2)) + crossing[i]);
        }

        for (int i = 0; i < countPart2; i++)
        {
            triangles[countFace + 6 * i + 0] = countVertex + i;
            triangles[countFace + 6 * i + 3] = countVertex + i + 1;
            if (SurfaceA)
            {
                triangles[countFace + 6 * i + 1] = countVertex + i + 1;
                triangles[countFace + 6 * i + 2] = countVertex + (countPart2 + 1) + i;
                triangles[countFace + 6 * i + 4] = countVertex + (countPart2 + 1) + i + 1;
                triangles[countFace + 6 * i + 5] = countVertex + (countPart2 + 1) + i;
            }
            else
            {
                triangles[countFace + 6 * i + 1] = countVertex + (countPart2 + 1) + i;
                triangles[countFace + 6 * i + 2] = countVertex + i + 1;
                triangles[countFace + 6 * i + 4] = countVertex + (countPart2 + 1) + i;
                triangles[countFace + 6 * i + 5] = countVertex + (countPart2 + 1) + i + 1;
            }
        }



        countVertex += (2 * countPart2 + 2);
        countFace += (countPart2 * 6);
    }

    void MakePart3(Quaternion rot)
    {
        vertices[countVertex + 0] = rot * (new Vector3(0f, 0f, 0f));
        vertices[countVertex + 1] = rot * (3 * Vx);
        vertices[countVertex + 2] = rot * (3 * Vy);

        triangles[countFace + 0] = countVertex + 0;
        triangles[countFace + 1] = (SurfaceA) ? countVertex + 1 : countVertex + 2;
        triangles[countFace + 2] = (SurfaceA) ? countVertex + 2 : countVertex + 1;

        countVertex += 3;
        countFace += 3;
    }

    void MakePart4(Quaternion rot)
    {
        Vector3 v1 = 3 * Vy;
        Vector3 v2 = 3 * Vy + Vz;
        Vector3 v3 = 3 * Vz + Vy;
        Vector3 v4 = 3 * Vz;
        vertices[countVertex + 0] = rot * (new Vector3(0f, 0f, 0f));
        for (int i = 0; i <= countPart2; i++)
        {
            vertices[countVertex + 1 + i] = rot * (CubicBezier(v1, v2, v3, v4, (1f * i / countPart2)));
        }

        for (int i = 0; i < countPart2; i++)
        {
            triangles[countFace + 3 * i + 0] = countVertex + 0;
            triangles[countFace + 3 * i + 1] = (SurfaceA) ? countVertex + 1 + i : countVertex + 2 + i;
            triangles[countFace + 3 * i + 2] = (SurfaceA) ? countVertex + 2 + i : countVertex + 1 + i;
        }
        countVertex += (countPart2+2);
        countFace += (3*countPart2);
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
}
