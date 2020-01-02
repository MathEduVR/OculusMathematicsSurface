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

//    public GameObject CameraRig;

    public float minX = -1f, maxX = 1f, minY = -1f, maxY = 1f, minZ = -1f, maxZ = 1f;

    MarchingCube MC;

    float InnerDivision(float x1, float x2, float rate1, float rate2)
    {
        float r1 = Mathf.Abs(rate1);
        float r2 = Mathf.Abs(rate2);
        return (r2 * x1 + r1 * x2) / (r1 + r2);
    }

    void Start()
    {
        MC = new MarchingCube();
        //GameObject[] objs = FindObjectsOfType<GameObject>();
        //for (int i = 0; i < objs.Length; i++)
        //{
        //    if (objs[i].name.Contains("OVRCameraRig"))
        //    {
        //        CameraRig = objs[i];
        //    }
        //}
        //CameraRig.transform.localPosition = eyeHeight;

        float stepX = (maxX - minX) * 0.05f;
        float stepY = (maxY - minY) * 0.05f;
        float stepZ = (maxZ - minZ) * 0.05f;
        vertices = new List<Vector3>();
        triangles = new List<int>();


        mesh = new Mesh();
        MC.SetList(vertices);

        float Delta = 0.01f;
        for (int xx = 0; xx < 20; xx++)
        {
            float x0 = minX + stepX * xx + Delta;
            float x1 = minX + stepX * (xx+1) + Delta;
            for (int yy = 0; yy < 20; yy++)
            {
                float y0 = minY + stepY * yy + Delta;
                float y1 = minY + stepY * (yy+1) + Delta;
                for (int zz=0; zz<20; zz++)
                {
                    float z0 = minZ + stepZ * zz + Delta;
                    float z1 = minZ + stepZ * (zz+1) + Delta;
                    float f000 = F(x0, y0, z0);
                    float f001 = F(x0, y0, z1);
                    float f010 = F(x0, y1, z0);
                    float f011 = F(x0, y1, z1);
                    float f100 = F(x1, y0, z0);
                    float f101 = F(x1, y0, z1);
                    float f110 = F(x1, y1, z0);
                    float f111 = F(x1, y1, z1);
                    if(xx==0 && yy==9 && zz == 10)
                    {
                        Debug.Log(f000 + "," + f001 + "," + f010 + "," + f011 + "," + f100 + "," + f101 + "," + f110 + "," + f111);
                    }
                    int count = 0;
                    int verticesCount = vertices.Count;
                    MC.SetF(f000,f001,f010,f011,f100,f101,f110,f111);
                    MC.SetXYZ(x0,y0,z0,x1,y1,z1);
                    MC.MarchingCube1();
                    int vCount = vertices.Count - verticesCount;
                    //Debug.Log(vCount);
                    if (vCount >= 3)
                    {
                        Vector3 normal = Vector3.Cross(vertices[verticesCount + 1] - vertices[verticesCount],
                        vertices[verticesCount + 2] - vertices[verticesCount]);
                        Vector3 grad = GradF(vertices[verticesCount]);
                        if ((SurfaceA && Vector3.Dot(normal, grad) > 0f) || (!SurfaceA && Vector3.Dot(normal, grad) < 0f))
                        {
                            if (vCount == 3)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                            }
                            else if (vCount == 4)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 3);
                            }
                            else if (vCount == 5)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 4);
                            }
                            else if (vCount == 6)
                            {
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 5);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 4);
                                triangles.Add(verticesCount + 5);
                            }
                        }
                        else
                        {
                            if (vCount == 3)
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                            }
                            else if (vCount == 4)
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                            }
                            else if (vCount == 5)
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 4);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount);
                            }
                            else if (vCount == 6)
                            {
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount + 1);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount + 2);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 5);
                                triangles.Add(verticesCount + 3);
                                triangles.Add(verticesCount);
                                triangles.Add(verticesCount + 5);
                                triangles.Add(verticesCount + 4);
                                triangles.Add(verticesCount + 3);
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
