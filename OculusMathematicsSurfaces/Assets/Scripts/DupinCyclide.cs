using UnityEngine;
using UnityEngine.SceneManagement;

public class DupinCyclide : MonoBehaviour
{
    public Mesh mesh = null;
    int uSize = 80;
    int vSize = 80;
    public Vector3[] vertices;
    public int[] triangles;

    public GameObject CameraRig;
    public Vector3 eyeHeight = Vector3.up;

    public float rate1, rate2;

    public float constA, constB, constC, constD;
    // Start is called before the first frame update
    void Start()
    {
        vertices = new Vector3[(uSize + 1) * (vSize + 1)];
        triangles = new int[uSize * vSize * 6];
        // 0 < b < a
        // c^2 = a^2 - b^2
        // d>0
        // (c < d)
        rate1 = 0.95f;
        rate2 = 1.2f;
        constA = 1f;
        constB = constA * rate1;
        constC = Mathf.Sqrt(constA * constA - constB * constB);
        constD = constC * rate2;

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
        else if (OVRInput.Get(OVRInput.Button.One))// rate1 --
        {
            rate1 -= 0.002f;
            if (rate1 < 0.01f) rate1 = 0.01f;
            constA = 1f;
            constB = constA * rate1;
            constC = Mathf.Sqrt(constA * constA - constB * constB);
            constD = constC * rate2;
            init_vertices();
            init_mesh();
        }
        else if (OVRInput.Get(OVRInput.Button.Two))// rate1 ++
        {
            rate1 += 0.002f;
            if (rate1 > 0.99f) rate1 = 0.99f;
            constA = 1f;
            constB = constA * rate1;
            constC = Mathf.Sqrt(constA * constA - constB * constB);
            constD = constC * rate2;
            init_vertices();
            init_mesh();
        }
        else if (OVRInput.Get(OVRInput.Button.Three))// rate2 --
        {
            rate2 -= 0.01f;
            if (rate2 < 0.01f) rate2 = 0.01f;
            constA = 1f;
            constB = constA * rate1;
            constC = Mathf.Sqrt(constA * constA - constB * constB);
            constD = constC * rate2;
            init_vertices();
            init_mesh();
        }
        else if (OVRInput.Get(OVRInput.Button.Four))// rate2 ++
        {
            rate2 += 0.01f;
            constA = 1f;
            constB = constA * rate1;
            constC = Mathf.Sqrt(constA * constA - constB * constB);
            constD = constC * rate2;
            init_vertices();
            init_mesh();
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
                float vv = 2f * Mathf.PI * v / (vSize);
                Vector3 a = new Vector3(surfaceX(uu, vv), surfaceY(uu, vv), surfaceZ(uu, vv));
                //Debug.Log(surfaceX(uu, vv)+":"+ surfaceY(uu, vv)+":"+ surfaceZ(uu, vv));
                vertices[u * (vSize + 1) + v] = new Vector3() ;
                vertices[u * (vSize + 1) + v] = a;
            }
        }
        for (int u = 0; u < uSize; u++)
        {
            for (int v = 0; v < vSize; v++)
            {
                triangles[6 * (u * vSize + v) + 0] = u * (vSize+1) + v;
                triangles[6 * (u * vSize + v) + 1] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 2] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 3] = u * (vSize + 1) + (v + 1);
                triangles[6 * (u * vSize + v) + 4] = (u + 1) * (vSize + 1) + v;
                triangles[6 * (u * vSize + v) + 5] = (u + 1) * (vSize + 1) + (v + 1);
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
        return (constD * (constC - constA * Mathf.Cos(u) * Mathf.Cos(v)) + constB * constB * Mathf.Cos(u)) / (constA - constC * Mathf.Cos(u) * Mathf.Cos(v));
    }

    float surfaceY(float u, float v)
    {
        return (constB * Mathf.Sin(u) * (constA - constD * Mathf.Cos(v))) / (constA - constC * Mathf.Cos(u) * Mathf.Cos(v));
    }

    float surfaceZ(float u, float v)
    {
        return (constB * Mathf.Sin(v) * (constC * Mathf.Cos(u) - constD)) / (constA - constC * Mathf.Cos(u) * Mathf.Cos(v));
    }
}
