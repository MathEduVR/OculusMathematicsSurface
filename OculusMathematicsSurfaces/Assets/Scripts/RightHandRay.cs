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
        CameraRig.transform.localPosition = eyeHeight;
        CameraRig.transform.rotation = Quaternion.identity;
        PlayerController.transform.position = homePosition;
        PlayerController.transform.rotation = Quaternion.identity;
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
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            eyeHeight.y -= 0.01f;
            if (eyeHeight.y < 0.2)
            {
                eyeHeight.y = 0.2f;
            }
            CameraRig.transform.localPosition = eyeHeight;
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            eyeHeight.y += 0.01f;
            CameraRig.transform.localPosition = eyeHeight;
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
            CameraRig.transform.localPosition = eyeHeight;
            CameraRig.transform.rotation = Quaternion.identity;
            PlayerController.transform.position = homePosition;
            PlayerController.transform.rotation = Quaternion.identity;
        }
        if (col != null && col.name.Contains("box_"))
        {
            if(allCaptions!=null)
                for (int i = 0; i < allCaptions.Length; i++)
                    allCaptions[i].gameObject.transform.localPosition = new Vector3(0f, -10f, 0f);
            GameObject obj = col.gameObject;
            TextMesh childText = obj.GetComponentInChildren<TextMesh>();
            //Debug.Log(childText);
            if (childText != null)
            {
                childText.gameObject.transform.localPosition = new Vector3(0f, 0.5f, 0);
            }
            GetComponent<MeshRenderer>().material = CursorOnMaterial;
            if (OVRInput.Get(OVRInput.Button.One) 
                || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) 
                || OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
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
            }
        }
        else
        {
            GetComponent<MeshRenderer>().material = CursorOffMaterial;
        }
    }
}
