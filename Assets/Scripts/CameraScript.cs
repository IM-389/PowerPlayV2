using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //obtain the camera being used
    [SerializeField]public Camera mainCamera;

    void Awake()
    {        
        //set the position of the camera so that the bottom left corner is at position 0,0
        transform.position -= mainCamera.ScreenToWorldPoint(new Vector3(2, 2, 30));
        //offset the position by half a unity measure
        transform.position += new Vector3(2.5f, 2.5f, -30);
    }

    // Update is called once per frame
    void Update()
    {
        //set the position of the camera so that the bottom left corner is at position 0,0
        transform.position -= mainCamera.ScreenToWorldPoint(new Vector3(2, 2, 30));
        //offset the position by half a unity measure
        transform.position += new Vector3(2.5f,2.5f,-30);

        //Debug.Log(Helper.getObjectFromClick());
    }
}
