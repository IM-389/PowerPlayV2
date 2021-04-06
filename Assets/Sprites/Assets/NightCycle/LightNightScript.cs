using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightNightScript : MonoBehaviour
{
    public GameObject childLight;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(NightOnOff.night == true)
        {
            childLight.SetActive(true);

        }
        if(NightOnOff.night == false)
        {
            childLight.SetActive(false);
        }
    }
}
