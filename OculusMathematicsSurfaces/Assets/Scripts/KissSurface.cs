using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KissSurface : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 80;
    int vSize = 80;
    public Vector3[] vertices;
    public int[] triangles;

    public GameObject CameraRig;
    public Vector3 eyeHeight = Vector3.up;


    public float constA, constB;
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];
        triangles = new int[uSize * vSize * 6];
        constA = 2.5f;
        constB = 0f;

        init_vertices();
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        init_mesh();
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
        UpdateButton();
    }

    void UpdateButton()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            eyeHeight.y += 0.02f;
            CameraRig.transform.localPosition = eyeHeight;
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            eyeHeight.y -= 0.02f;
            if (eyeHeight.y < 0.2f) eyeHeight.y = 0.2f;
            CameraRig.transform.localPosition = eyeHeight;
        }
        else if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            SceneManager.LoadScene("Scenes/Main");
        }
    }

    void init_vertices()
    {
        for (int u = 0; u <= uSize; u++)
        {
            for (int v = 0; v <= vSize; v++)
            {
                float uu = 2f * Mathf.PI * u / (uSize);
                float vv = 2f * v / (vSize) - 1f;
                Vector3 a = new Vector3(surfaceX(uu, vv), surfaceY(uu, vv), surfaceZ(uu, vv));
                //Debug.Log(surfaceX(uu, vv)+":"+ surfaceY(uu, vv)+":"+ surfaceZ(uu, vv));
                vertices[u * (vSize + 1) + v] = new Vector3();
                vertices[u * (vSize + 1) + v] = a;
            }
        }
        for (int u = 0; u < uSize; u++)
        {
            for (int v = 0; v < vSize; v++)
            {
                triangles[6 * (u * vSize + v) + 0] = u * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 2] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 1] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 3] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 5] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 4] = (u + 1) * (vSize + 1) + (v + 1);
            }
        }

    }

    void init_mesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    float surfaceX(float u, float v)
    {
        return constA * v*v*Mathf.Sqrt((1 - v) / 2f) * Mathf.Cos(u);
    }

    float surfaceY(float u, float v)
    {
        return constA * v;
    }

    float surfaceZ(float u, float v)
    {
        return constA * v * v * Mathf.Sqrt((1 - v) / 2f) * Mathf.Sin(u);
    }
}
