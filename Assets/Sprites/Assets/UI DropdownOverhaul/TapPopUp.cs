using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapPopUp : MonoBehaviour
{
    RectTransform tabPos;
    public GameObject teleportLocation;
    //float upMax = 5;

    // Start is called before the first frame update
    void Start()
    {
        tabPos = transform.GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //gameObject.transform.GetComponent<RectTransform> = tabPos;
    }


    private void OnMouseDown()
    {
        //gameObject.transform.GetComponent < RectTransform >() += 45;
        //gameObject.transform.position = teleportLocation.transform.GetComponent<RectTransform>();
        //print("yyeeet");
    }
}
