using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRay : MonoBehaviour
{
    Vector3 RayStart;
    Vector3 RayCtrl;

    public GameObject RightHand;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        for(int i=0; i<objs.Length; i++)
        {
            if(objs[i].name== "RightHandAnchor")
            {
                RightHand = objs[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
