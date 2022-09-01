using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialConnect.Wwise;
using SpatialConnect.Wwise.Core;


public class TriggerStandardEvent : MonoBehaviour
{
    public InputBehaviour inputBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(){
        Debug.Log("Trigger Event here");
        //OnStandardInteractionStateChanged(true);
    }
}
