using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHand : MonoBehaviour
{

    [SerializeField]
    private string handName = "IndexTip Proxy Transform";
    private Vector3 pos; 
    [SerializeField]
    bool laserPointer;

    // Update is called once per frame
    void Update()
    {
        if(laserPointer){
            mapPosToPointer();
        }else{        
            mapPosToHand();
        }
    }

    void mapPosToHand(){
        if(GameObject.Find(handName)){
        
            GameObject index_finger = GameObject.Find(handName);

            if (index_finger.transform.parent.name.Contains("Right")){
                pos = index_finger.transform.position;
                this.transform.position = pos;

                Vector3 angles = index_finger.transform.localEulerAngles;
                this.transform.localEulerAngles = angles;
            }
        }
    }

    
    void mapPosToPointer(){

        if(GameObject.Find("Right_ShellHandRayPointer(Clone)"))
        {
            GameObject pointer = GameObject.Find("Right_ShellHandRayPointer(Clone)");
            pos = pointer.transform.position;
            this.transform.position = pos;

            Quaternion angles = pointer.transform.rotation;
            this.transform.rotation = angles;
        }
    }
}
