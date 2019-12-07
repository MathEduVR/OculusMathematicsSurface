using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common : MonoBehaviour
{
    //受け渡し用
    public static Vector3 ControllerPosition;
    public static Quaternion ControllerRotation;
    public static Vector3 CameraRigPosition;
    public static Quaternion CameraRigRotation;
    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this);
        ControllerPosition = new Vector3(0, 1.5f, -3);
        ControllerRotation = Quaternion.identity;
        CameraRigPosition = Vector3.up;
        CameraRigRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
