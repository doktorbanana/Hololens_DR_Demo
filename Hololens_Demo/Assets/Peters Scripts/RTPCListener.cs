using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTPCListener : MonoBehaviour
{
    maxAlpha = 0.9f;

    [SerializeField]
    string rtpcID;

    float value;
    Material material;
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        int type = 1;
        AkSoundEngine.GetRTPCValue(rtpcID, gameObject, 0, out value, ref type);
        value += 48;
        value /= 24;        
        color = material.color;
        color.a = Mathf.Clamp(value, 0, maxAlpha );
        material.color = color;

    }
}
