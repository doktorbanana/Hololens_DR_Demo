using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialConnect.Wwise;
using SpatialConnect.Wwise.Core;
using UnityEngine.XR;
using Peter.Hololens;

public class TriggerStandardEvent : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        GetHololensInput.input = true;
    }

    public void unTriggerEvent(){
        GetHololensInput.input = false;
    }
}