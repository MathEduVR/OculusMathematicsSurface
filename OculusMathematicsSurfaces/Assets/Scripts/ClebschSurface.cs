using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClebschSurface : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject PointLight;
    public GameObject Player;


    public List<Vector3> vertices;
    public List<int> triangles;
    float scale = 1f;
    //float step = 0.02f;

    public bool SurfaceA;
    Mesh mesh;
    public Material SurfaceMaterial;

    public GameObject CameraRig;
    public Vector3 eyeHeight;

    public GameObject Line;
    public int LineNumber;

    private float f000, f001, f010, f011, f100, f101, f110, f111;
    private float x0, y0, z0;
    private float x1, y1, z1;

    public float startX, endX, startY, endY, startZ, endZ;

    MarchingCube MC;

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
        float scale = 2f / border;
        float step = 0.04f;
        vertices = new List<Vector3>();
        triangles = new List<int>();


        mesh = new Mesh();
        MC = new MarchingCube();
        MC.SetList(vertices);
        float Delta = 0.009f;
        int DivNumberX = Mathf.CeilToInt((endX - startX) / step);
        for (int xx = 0; xx<DivNumberX; xx++)
        {
            x0 = startX + xx * step + Delta;
            x1 = startX + (xx + 1) * step + Delta;
            int DivNumberY = Mathf.CeilToInt((endY - startY) / step);
            for (int yy=0; yy<DivNumberY; yy++)
            {
                y0 = startY + yy * step + Delta;
                y1 = startY + (yy + 1) * step + Delta;
                int DivNumberZ = Mathf.CeilToInt((endZ - startZ) / step);
                for (int zz=0; zz<DivNumberZ; zz++)
                {
                    z0 = startZ + zz * step + Delta;
                    z1 = startZ + (zz + 1) * step + Delta;
                    //
                    f000 = F(x0, y0, z0);
                    f001 = F(x0, y0, z1);
                    f010 = F(x0, y1, z0);
                    f011 = F(x0, y1, z1);
                    f100 = F(x1, y0, z0);
                    f101 = F(x1, y0, z1);
                    f110 = F(x1, y1, z0);
                    f111 = F(x1, y1, z1);
                    if (xx == 41 && yy == 16 && zz == 39)
                    {
                        Debug.Log(f000 + "," + f001 + "," + f010 + "," + f011 + "," + f100 + "," + f101 + "," + f110 + "," + f111);
                    }

                    MC.SetF(f000, f001, f010, f011, f100, f101, f110, f111);
                    MC.SetXYZ(x0, y0, z0, x1, y1, z1);
                    int verticesCount = vertices.Count;
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
        LineNumber = 0;

    }

    float F(float x, float y, float z)
    {
        // formula 1(original) 
        //float w = x + y + z + 1f;
        //return x * x * x + y * y * y + z * z * z + 1f - w * w * w;

        // formula 2(Hunt) 
        //float s3 = x * x * x + y * y * y + z * z * z;
        //float s21 = x * x * y + y * y * z + z * z * x + x * x * z + z * z * y + y * y * x;
        //float s111 = x * y * z;
        //float s11 = x * y + y * z + z * x;
        //float s2 = x * x + y * y + z * z;
        //float s1 = x + y + z;
        //return 81 * s3 - 189 * s21 + 54 * s111 + 126 * s11 - 9 * s2 - 9 * s1 + 1;

        //formula 3(ClebSch and Klein)

        return 64 * x * x * x + 48 * x * x * y- 192 * z * z * x + 48 * z * z * y
            - 31 * y * y * y - 54 * y * y - 24 * y;
    }

    Vector3 GradF(Vector3 V)
    {

        float x = V.x;
        float y = V.y;
        float z = V.z;

        // formula 1(original) 
        //float w = x + y + z + 1f;
        //return new Vector3(3f * x * x - 3f * w * w, 3f * y * y - 3f * w * w, 3f * z * z - 3f * w * w);

        // formula 2(Hunt) 
        //float s3x = 3f * x * x;
        //float s21x = 2f * x * y + 2f * x * z + y * y + z * z;
        //float s111x = y * z;
        //float s11x = y+z;
        //float s2x = 2f * x;
        //float s1x = 1f;
        //float gx = 81 * s3x - 189 * s21x + 54 * s111x + 126 * s11x - 9 * s2x - 9 * s1x;

        //float s3y = 3f * y * y;
        //float s21y = 2f * x * y + 2f * y * z + x * x + z * z;
        //float s111y = x * z;
        //float s11y = x + z;
        //float s2y = 2f * y;
        //float s1y = 1f;
        //float gy = 81 * s3y - 189 * s21y + 54 * s111y + 126 * s11y - 9 * s2y - 9 * s1y;

        //float s3z = 3f * z * z;
        //float s21z = 2f * x * z + 2f * y * z + x * x + y * y;
        //float s111z = x * y;
        //float s11z = x + y;
        //float s2z = 2f * z;
        //float s1z = 1f;
        //float gz = 81 * s3z - 189 * s21z + 54 * s111z + 126 * s11z - 9 * s2z - 9 * s1z;
        //return new Vector3(gx,gy,gz);

        //formula 3(ClebSch and Klein)
        float gx = 3 * 64 * x * x + 2 * 48 * x * y - 192 * z * z;
        float gy = 48 * x * x + 48 * z * z
            - 3 * 31 * y * y - 2 * 54 * y - 24;
        float gz = -2 * 192 * z * x + 2 * 48 * z * y;
        return new Vector3(gx, gy, gz);
    }



    // Update is called once per frame
    void Update()
    {
        int NumberOfLines = 27;
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            LineNumber = (LineNumber + NumberOfLines - 1) % NumberOfLines;
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            LineNumber = (LineNumber + 1) % NumberOfLines;
        }
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }

        // position of Line
        Vector3 LinePosition;
        Vector3 LineDirection;
        float phi = (1 + Mathf.Sqrt(5)) / 2f;// 黄金比
        Vector4 x1 = new Vector4(-1f / 3f, 1f / 12f, Mathf.Sqrt(3) / 3f, 0);
        Vector4 x2 = new Vector4(-1f / 3f, 1f / 12f, -Mathf.Sqrt(3) / 3f, 0);
        Vector4 x3 = new Vector4(2f / 3f, 1f / 12f, 0f, 0f);
        Vector4 x4 = new Vector4(0, 1f / 4f, 0, -1f / 3f);
        Vector4 x5 = -x1 - x2 - x3 - x4;

        switch (LineNumber)
        {
            case 0:    //D_12_34
                Vector4 e1 = x1 + x2;
                Vector4 e2 = x3 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 1:    //D_12_35
                e1 = x1 + x2;
                e2 = x3 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 2:    //D_12_45
                e1 = x1 + x2;
                e2 = x4 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 3:    //D_13_24
                e1 = x1 + x3;
                e2 = x2 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 4:    //D_13_25
                e1 = x1 + x3;
                e2 = x2 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 5:    //D_13_45
                e1 = x1 + x3;
                e2 = x4 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 6:    //D_14_23
                e1 = x1 + x4;
                e2 = x2 + x3;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 7:    //D_14_25
                e1 = x1 + x4;
                e2 = x2 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 8:    //D_14_35
                e1 = x1 + x4;
                e2 = x3 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 9:    //D_15_23
                e1 = x1 + x5;
                e2 = x2 + x3;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 10:    //D_15_24
                e1 = x1 + x5;
                e2 = x2 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 11:    //D_15_34
                e1 = x1 + x5;
                e2 = x3 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 12:    //D_23_45
                e1 = x2 + x3;
                e2 = x4 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 13:    //D_24_35
                e1 = x2 + x4;
                e2 = x3 + x5;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 14:    //D_25_34
                e1 = x2 + x5;
                e2 = x3 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 15:    //Delta_1234
                e1 = x1 + phi * x2 + x3;
                e2 = phi * x1 + x2 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 16:    //Delta_1243
                e1 = x1 + phi * x2 + x4;
                e2 = phi * x1 + x2 + x3;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 17:    //Delta_1324
                e1 = x1 + phi * x3 + x2;
                e2 = phi * x1 + x3 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 18:    //Delta_1342
                e1 = x1 + phi * x3 + x4;
                e2 = phi * x1 + x3 + x2;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 19:    //Delta_1423
                e1 = x1 + phi * x4 + x2;
                e2 = phi * x1 + x4 + x3;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 20:    //Delta_1432
                e1 = x1 + phi * x4 + x3;
                e2 = phi * x1 + x4 + x2;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 21:    //Delta_2314
                e1 = x2 + phi * x3 + x1;
                e2 = phi * x2 + x3 + x4;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 22:    //Delta_2341
                e1 = x2 + phi * x3 + x4;
                e2 = phi * x2 + x3 + x1;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 23:    //Delta_2413
                e1 = x2 + phi * x4 + x1;
                e2 = phi * x2 + x4 + x3;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 24:    //Delta_2431
                e1 = x2 + phi * x4 + x3;
                e2 = phi * x2 + x4 + x1;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 25:    //Delta_3412
                e1 = x3 + phi * x4 + x1;
                e2 = phi * x3 + x4 + x2;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            case 26:    //Delta_3421
                e1 = x3 + phi * x4 + x2;
                e2 = phi * x3 + x4 + x1;
                LinePosition = GetLinePosition(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                LineDirection = GetLineDirection(e1.x, e1.y, e1.z, e1.w, e2.x, e2.y, e2.z, e2.w);
                break;
            default:
                LinePosition = new Vector3(0f, 0f, 0);
                LineDirection = new Vector3(1f, -1f, 0f);
                break;
        }
        LinePosition *= scale;
        Quaternion LineRotation = Quaternion.FromToRotation(new Vector3(0f, 1f, 0f), LineDirection);
        Line.transform.position = LinePosition;
        Line.transform.rotation = LineRotation;
    }
    Vector3 GetLineDirection(float a, float b, float c, float p, float d, float e, float f, float q)
    {
        //ax+by+cz=p
        //dx+ey+fz=q
        return new Vector3(
            b * f - c * e,
            c * d - a * f,
            a * e - b * d);
    }
    Vector3 GetLinePosition(float a, float b, float c, float p, float d, float e, float f, float q)
    {
        // ax+by+cz=p
        // dx+ey+fz=q
        // gx+hy+iz=0
        float g = b * f - c * e;
        float h = c * d - a * f;
        float i = a * e - b * d;
        float den = a * (e * i - h * f) + d * (h * c - b * i) + g * (b * f - e * c);
        float xx = (p * (e * i - h * f) + q * (h * c - b * i) + 0 * (b * f - e * c)) / den;
        float yy = (a * (q * i - 0 * f) + d * (0 * c - p * i) + g * (p * f - q * c)) / den;
        float zz = (a * (e * 0 - h * q) + d * (h * p - b * 0) + g * (b * q - e * p)) / den;
        return new Vector3(xx, yy, zz);
    }

    void MarchingCube0() 
    {
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
