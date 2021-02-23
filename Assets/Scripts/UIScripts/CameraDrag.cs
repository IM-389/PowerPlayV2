using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    public float dragSpeed = 2;

    private Vector3 dragOrigin;

    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = dragSpeed;
            camera.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed);
        }
    }
}
