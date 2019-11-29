using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoebiusRing : MonoBehaviour
{
    public Mesh mesh = null;
    int longitude = 60;
    public Vector3[] vertices;
    public int[] triangles;

    public GameObject CameraRig;
    public Vector3 eyeHeight;

    Vector3[] center;

    // Start is called before the first frame update
    void Start()
    {
        center = new Vector3[longitude];
        GameObject[] objs = FindObjectsOfType<GameObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name.Contains("OVRCameraRig"))
            {
                CameraRig = objs[i];
            }
        }
        if (mesh == null)
        {
            init_mesh();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown))
        //{
        //    eyeHeight.y -= 0.01f;
        //    if (eyeHeight.y < 0.2)
        //    {
        //        eyeHeight.y = 0.2f;
        //    }
        //    CameraRig.transform.localPosition = eyeHeight;
        //}
        //else if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp))
        //{
        //    eyeHeight.y += 0.01f;
        //    CameraRig.transform.localPosition = eyeHeight;
        //}
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }

    void init_mesh()
    {
        vertices = new Vector3[longitude * 4];
        triangles = new int[3 * 4 * longitude];

        for (int i = 0; i < longitude; i++)
        {
            center[i] = new Vector3(
                2f * Mathf.Cos(2 * Mathf.PI * i / longitude),
                2f * Mathf.Sin(2 * Mathf.PI * i / longitude),
                0f
                );
        }
        for (int i = 0; i < longitude; i++)
        {
            int i1 = (i + 1) % longitude;
            Vector3 v0 = center[i1] - center[i];
            Vector3 v1 = center[i];
            Vector3 v2 = Vector3.Cross(v0, v1);
            v1.Normalize();
            v2.Normalize();
            float angle = Mathf.PI * i / longitude;
            Vector3 w1 = Mathf.Cos(angle) * v1 - Mathf.Sin(angle) * v2;
            Vector3 w2 = Mathf.Sin(angle) * v1 + Mathf.Cos(angle) * v2;
            vertices[i * 4 + 0] = new Vector3();
            vertices[i * 4 + 1] = new Vector3();
            vertices[i * 4 + 2] = new Vector3();
            vertices[i * 4 + 3] = new Vector3();
            vertices[i * 4 + 0] = center[i] + w1 + w2 * 0.01f;
            vertices[i * 4 + 1] = center[i] - w1 + w2 * 0.01f;
            vertices[i * 4 + 2] = center[i] + w1 - w2 * 0.01f;
            vertices[i * 4 + 3] = center[i] - w1 - w2 * 0.01f;
        }
        for (int i = 0; i < longitude; i++)
        {
            int i1 = (i + 1) % longitude;
            if (i < longitude - 1)
            {
                triangles[12 * i + 0] = 4 * i;
                triangles[12 * i + 1] = 4 * i + 1;
                triangles[12 * i + 2] = 4 * i1;
                triangles[12 * i + 3] = 4 * i1;
                triangles[12 * i + 4] = 4 * i + 1;
                triangles[12 * i + 5] = 4 * i1 + 1;
                triangles[12 * i + 6] = 4 * i + 2;
                triangles[12 * i + 7] = 4 * i1 + 2;
                triangles[12 * i + 8] = 4 * i + 3;
                triangles[12 * i + 9] = 4 * i + 3;
                triangles[12 * i + 10] = 4 * i1 + 2;
                triangles[12 * i + 11] = 4 * i1 + 3;
            }
            else
            {
                triangles[12 * i + 0] = 4 * i;
                triangles[12 * i + 1] = 4 * i + 1;
                triangles[12 * i + 2] = 4 * i1 + 3;
                triangles[12 * i + 3] = 4 * i1 + 3;
                triangles[12 * i + 4] = 4 * i + 1;
                triangles[12 * i + 5] = 4 * i1 + 2;
                triangles[12 * i + 6] = 4 * i + 2;
                triangles[12 * i + 7] = 4 * i1 + 1;
                triangles[12 * i + 8] = 4 * i + 3;
                triangles[12 * i + 9] = 4 * i + 3;
                triangles[12 * i + 10] = 4 * i1 + 1;
                triangles[12 * i + 11] = 4 * i1 + 0;
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
