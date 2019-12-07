using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeGyroid : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject PointLight;
    public GameObject Player;


    public List<Vector3> vertices;
    public List<int> triangles;
    float scale = 1f;

    public bool SurfaceA;
    Mesh mesh;
    public Material SurfaceMaterial;

    public GameObject CameraRig;
    public Vector3 eyeHeight;

    public GameObject Line;
    public int LineNumber;


    float InnerDivision(float x1, float x2, float rate1, float rate2)
    {
        float r1 = Mathf.Abs(rate1);
        float r2 = Mathf.Abs(rate2);
        return (r2 * x1 + r1 * x2) / (r1 + r2);
    }

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
        CameraRig.transform.localPosition = eyeHeight;

        float border = 2f;
        float step = border * 0.05f;
        vertices = new List<Vector3>();
        triangles = new List<int>();


        mesh = new Mesh();

        for (float x0 = -border; x0 <= border; x0 += step)
        {
            float x1 = x0 + step;
            for (float y0 = -border; y0 <= border; y0 += step)
            {
                float y1 = y0 + step;
                for (float z0 = -border; z0 <= border; z0 += step)
                {
                    float z1 = z0 + step;
                    float f000 = F(x0, y0, z0);
                    float f001 = F(x0, y0, z1);
                    float f010 = F(x0, y1, z0);
                    float f011 = F(x0, y1, z1);
                    float f100 = F(x1, y0, z0);
                    float f101 = F(x1, y0, z1);
                    float f110 = F(x1, y1, z0);
                    float f111 = F(x1, y1, z1);
                    int verticesCount = vertices.Count;
                    int count = 0;
                    if (f000 * f001 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x0,
                            y0,
                            InnerDivision(z0, z1, f000, f001)
                            ));
                        count++;
                    }
                    if (count < 3 && f000 * f100 < 0f)
                    {
                        vertices.Add(new Vector3(
                            InnerDivision(x0, x1, f000, f100),
                            y0,
                            z0
                            ));
                        count++;
                    }
                    if (count < 3 && f000 * f010 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x0,
                            InnerDivision(y0, y1, f000, f010),
                            z0
                            ));
                        count++;
                    }

                    if (count < 3 && f001 * f101 < 0f)
                    {
                        vertices.Add(new Vector3(
                            InnerDivision(x0, x1, f001, f101),
                            y0,
                            z1
                            ));
                        count++;
                    }
                    if (count < 3 && f001 * f011 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x0,
                            InnerDivision(y0, y1, f001, f011),
                            z1
                            ));
                        count++;
                    }
                    if (count < 3 && f100 * f110 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x1,
                            InnerDivision(y0, y1, f100, f110),
                            z0
                            ));
                        count++;
                    }
                    if (count < 3 && f100 * f101 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x1,
                            y0,
                            InnerDivision(z0, z1, f100, f101)
                            ));
                        count++;
                    }
                    if (count < 3 && f010 * f011 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x0,
                            y1,
                            InnerDivision(z0, z1, f010, f011)
                            ));
                        count++;
                    }
                    if (count < 3 && f010 * f110 < 0f)
                    {
                        vertices.Add(new Vector3(
                            InnerDivision(x0, x1, f010, f110),
                            y1,
                            z0
                            ));
                        count++;
                    }

                    if (count < 3 && f110 * f111 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x1,
                            y1,
                            InnerDivision(z0, z1, f110, f111)
                            ));
                        count++;
                    }
                    if (count < 3 && f011 * f111 < 0f)
                    {
                        vertices.Add(new Vector3(
                            InnerDivision(x0, x1, f011, f111),
                            y1,
                            z1
                            ));
                        count++;
                    }
                    if (count < 3 && f101 * f111 < 0f)
                    {
                        vertices.Add(new Vector3(
                            x1,
                            InnerDivision(y0, y1, f101, f111),
                            z1
                            ));
                        count++;
                    }
                    int vCount = vertices.Count - verticesCount;
                    //Debug.Log(vCount);
                    if (vCount >= 3)
                    {
                        Vector3 normal = Vector3.Cross(vertices[verticesCount + 1] - vertices[verticesCount],
                            vertices[verticesCount + 2] - vertices[verticesCount]);
                        Vector3 grad = GradF(vertices[verticesCount]);
                        if (SurfaceA)
                        {
                            if (Vector3.Dot(normal, grad) > 0f)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                            }
                            else
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                            }
                        }
                        else
                        {
                            if (Vector3.Dot(normal, grad) < 0f)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                            }
                            else
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                            }
                        }
                    }

                }
            }
        }
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] *= scale;
        }
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = SurfaceMaterial;
        LineNumber = 0;

    }

    float F(float x, float y, float z)
    {
        float pi2 =  Mathf.PI;
        return Mathf.Cos(x * pi2) * Mathf.Sin(y * pi2) 
            + Mathf.Cos(y * pi2) * Mathf.Sin(z * pi2)
            + Mathf.Cos(z * pi2) * Mathf.Sin(x * pi2);
    }

    Vector3 GradF(Vector3 V)
    {
        float pi2 = Mathf.PI;

        float x = V.x * pi2;
        float y = V.y * pi2;
        float z = V.z * pi2;

        float gx = (Mathf.Cos(z) * Mathf.Cos(x) - Mathf.Sin(x) * Mathf.Sin(y)) * pi2;
        float gy = (Mathf.Cos(x) * Mathf.Cos(y) - Mathf.Sin(y) * Mathf.Sin(z)) * pi2;
        float gz = (Mathf.Cos(y) * Mathf.Cos(z) - Mathf.Sin(z) * Mathf.Sin(x)) * pi2;
        return new Vector3(gx, gy, gz);
    }



    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }


}
