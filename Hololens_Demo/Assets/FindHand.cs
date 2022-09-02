using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindHand : MonoBehaviour
{

    [SerializeField]
    private string handName = "IndexTip Proxy Transform";
    private Vector3 pos; 


    // Update is called once per frame
    void Update()
    {
        
        if(GameObject.Find(handName)){
            
            pos = GameObject.Find(handName).transform.position;
            this.transform.position = pos;
            
            Vector3 angles = GameObject.Find(handName).transform.localEulerAngles;
            this.transform.localEulerAngles = angles;
        }else{
            Debug.LogWarning("Couldn't Find Hand!");
        }
    }
}
