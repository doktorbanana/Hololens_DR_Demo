using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asset_Manager : MonoBehaviour
{   
    [SerializeField]
    private GameObject coffee = null;
    [SerializeField]
    private GameObject arcade = null;
    [SerializeField]
    private GameObject speech = null;


    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if(coffee && arcade && speech){
            if(Input.GetKeyDown("h")){
                coffee.SetActive(!coffee.activeSelf);
                arcade.SetActive(!arcade.activeSelf);
                speech.SetActive(!speech.activeSelf);
            }
        }else{
            Debug.LogWarning("3D Assets not assigned in Inspector. Can't Hide...");
        }
    }
}
