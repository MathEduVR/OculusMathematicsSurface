using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RightHandRay : MonoBehaviour
{
    public Vector3 RayStart;
    public Vector3 RayCtrl;
    public Quaternion RayDirection;
    public Vector3 CtrlPt;

    public GameObject PlayerController;
    public GameObject RightHand;
    public GameObject CameraRig;

    public GameObject RightRay;

    Ray ray;
    readonly Vector3 homePosition = new Vector3(0f,1f,-5f);

    public Material CursorOffMaterial, CursorOnMaterial;
    public Collider col;
    public Vector3 eyeHeight = Vector3.up;

    TextMesh[] allCaptions;

    

    // Start is called before the first frame update
    void Start()
    {
        StartRoutine();
        allCaptions = FindObjectsOfType<TextMesh>();
        if (allCaptions != null)
            for (int i = 0; i < allCaptions.Length; i++)
                allCaptions[i].gameObject.transform.localPosition = new Vector3(100f, 100f, 100f);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateRoutine();
        UpdateRayRoutine();
        UpdateButton();
    }

    public void StartRoutine()
    {

        GameObject[] objs = FindObjectsOfType<GameObject>();
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].name.Contains("RightHandAnchor"))
            {
                RightHand = objs[i];
            }
            if (objs[i].name.Contains("OVRCameraRig"))
            {
                CameraRig = objs[i];
            }
            if (objs[i].name.Contains("OVRPlayerController"))
            {
                PlayerController = objs[i];
            }
        }
        CtrlPt = RightHand.transform.rotation * Vector3.forward;
        ray = new Ray();
        PlayerController.transform.position = Common.ControllerPosition;
        PlayerController.transform.rotation = Common.ControllerRotation;
        CameraRig.transform.localPosition = Common.CameraRigPosition;
        CameraRig.transform.rotation = Common.CameraRigRotation;
    }

    public void UpdateRoutine()
    {
        RayStart = RightHand.transform.position;
        RayDirection = RightHand.transform.rotation;
        RayCtrl = RayDirection * Vector3.forward;
        ray.origin = RayStart;
        ray.direction = RayCtrl;


        bool hitTF =  Physics.Raycast(ray, out RaycastHit hit);

        if (hitTF)
        {
            transform.position = hit.point;
            col = hit.collider;
        }
        else
        {
            transform.position = Vector3.down; //RayStart + RayCtrl ;
            col = null;
        }
 
    }
    public void UpdateRayRoutine()
    {
        if(RightRay != null) { 
            RightRay.transform.position = RayStart;
            RightRay.transform.rotation = RayDirection * Quaternion.Euler(90, 0, 0);
        }
    }
    void UpdateButton()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start)){
            CameraRig.transform.localPosition = Common.CameraRigPosition;
            CameraRig.transform.rotation = Common.CameraRigRotation;
            PlayerController.transform.position = Common.ControllerPosition;
            PlayerController.transform.rotation = Common.ControllerRotation;
        }
        if (col != null && col.name.Contains("box_"))
        {
            if (allCaptions != null)
                for (int i = 0; i < allCaptions.Length; i++)
                    allCaptions[i].gameObject.transform.localPosition = new Vector3(100f, 100f, 100f);
            GameObject obj = col.gameObject;
            TextMesh childText = obj.GetComponentInChildren<TextMesh>();
            //Debug.Log(childText);
            if (childText != null)
            {
                childText.gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0);
            }
            GetComponent<MeshRenderer>().material = CursorOnMaterial;
            if (OVRInput.Get(OVRInput.Button.One) 
                || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                Common.ControllerPosition = PlayerController.transform.position;
                Common.ControllerRotation = PlayerController.transform.rotation;
                Common.CameraRigPosition = CameraRig.transform.localPosition;
                Common.CameraRigRotation = CameraRig.transform.rotation;
                //GameObject colObj = col.gameObject;
                Debug.Log(obj.name);
                if (obj.name.Contains("box_Moebius"))
                {
                    SceneManager.LoadScene("Scenes/MoebiusRing");
                }
                else if (obj.name.Contains("box_DupinCyclide"))
                {
                    SceneManager.LoadScene("Scenes/DupinCyclide");
                }
                else if (obj.name.Contains("box_SeifertSurface"))
                {
                    SceneManager.LoadScene("Scenes/SeifertSurface");
                }
                else if (obj.name.Contains("box_ClebschSurface1"))
                {
                    SceneManager.LoadScene("Scenes/ClebschSurface1");
                }
                else if (obj.name.Contains("box_KissSurface"))
                {
                    SceneManager.LoadScene("Scenes/KissSurface");
                }
                else if (obj.name.Contains("box_RomanSurface"))
                {
                    SceneManager.LoadScene("Scenes/RomanSurface");
                }
                else if (obj.name.Contains("box_BoySurface"))
                {
                    SceneManager.LoadScene("Scenes/BoySurface");
                }
                //room 2
                else if (obj.name.Contains("box_FunctionPlot01"))
                {
                    SceneManager.LoadScene("Scenes/FunctionPlot01");
                }
                else if (obj.name.Contains("box_FunctionPlot02"))
                {
                    SceneManager.LoadScene("Scenes/FunctionPlot02");
                }
                else if (obj.name.Contains("box_SaddlePoint"))
                {
                    SceneManager.LoadScene("Scenes/SaddlePoint");
                }
                //room 3
                else if (obj.name.Contains("box_BoursMinimal"))
                {
                    SceneManager.LoadScene("Scenes/BoursMinimal");
                }
                //CatalansSurface
                else if (obj.name.Contains("box_CatalansMinimal"))
                {
                    SceneManager.LoadScene("Scenes/CatalansMinimal");
                }
                //FakeGyroid
                else if (obj.name.Contains("box_FakeGyroid"))
                {
                    SceneManager.LoadScene("Scenes/FakeGyroid");
                }
                //Helicoid
                else if (obj.name.Contains("box_Helicoid"))
                {
                    SceneManager.LoadScene("Scenes/Helicoid");
                }
            }
        }
        else
        {
            GetComponent<MeshRenderer>().material = CursorOffMaterial;
        }
    }
}
