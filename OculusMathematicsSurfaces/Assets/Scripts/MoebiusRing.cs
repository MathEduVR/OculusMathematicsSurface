using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoebiusRing : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 120;//uの分割数
    int vSize = 15;//vの分割数
    public Vector3[] vertices;
    public int[] triangles;

    //public GameObject CameraRig;
    //public Vector3 eyeHeight;

    public int change =0;

    Vector3[] center;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3[uSize+1];
        GameObject[] objs = FindObjectsOfType<GameObject>();
        if (mesh == null)
        {
            InitMesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (GetComponent<Rigidbody>().useGravity == false)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                transform.position = new Vector3(0f, 1f, 4f);
                transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Rigidbody>().useGravity = false;
            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            change = 1 - change;
            InitMesh();

        }
    }

    void InitMesh()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];//頂点の座標
        triangles = new int[uSize * vSize * 6];//三角形の個数*3の長さの配列
        
        for (int i = 0; i <= uSize; i++)
        {
            center[i] = new Vector3(
                2f * Mathf.Cos(4 * Mathf.PI * i / uSize),
                2f * Mathf.Sin(4 * Mathf.PI * i / uSize),
                0f
                );
        }
        for (int u = 0; u <= uSize; u++)
        {
            int u1 = (u + 1) % uSize;
            Vector3 v0 = center[u1] - center[u];
            Vector3 v1 = center[u];
            Vector3 v2 = Vector3.Cross(v0, v1);
            v1.Normalize();
            v2.Normalize();
            float angle = 2 * Mathf.PI * u / uSize;
            Vector3 w1 = Mathf.Cos(angle) * v1 - Mathf.Sin(angle) * v2;
            for (int v = 0; v <= vSize; v++)
            {
                float vv = -1f + 2f * v / vSize; 
                vertices[u * (vSize + 1) + v] = center[u] + vv* w1;
            }
        }
        for (int u = 0; u < uSize; u++)
        {
            for (int v = 0; v < vSize; v++)
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
        }
        mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;

    }


 
}
