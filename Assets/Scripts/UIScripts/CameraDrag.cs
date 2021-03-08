using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    public float dragSpeed = 2;

    private Vector3 dragOrigin;

    private Camera camera;

    public KeyCode zoomIn, zoomOut;

    public float minZoom;
    public float maxZoom;

    [Tooltip("Minimum (lowest) values the camera's coordinates can be")]
    public Vector2 minCameraPos;
    
    [Tooltip("Maximum (highest) values the camera's coordinates cam be")]
    public Vector2 maxCameraPos;
    
    private void Start()
    {
        camera = Camera.main;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = dragSpeed * Time.fixedDeltaTime;
            camera.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed);
            Vector3 newPos = camera.transform.position;
            newPos.x = Mathf.Clamp(newPos.x, minCameraPos.x, maxCameraPos.x);
            newPos.y = Mathf.Clamp(newPos.y, minCameraPos.y, maxCameraPos.y);
            camera.transform.position = newPos;
        }
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        camera.transform.Translate(new Vector3(xAxis*Time.fixedDeltaTime, yAxis*Time.fixedDeltaTime, 0));
        if (Input.mouseScrollDelta.y > 0.5f && camera.orthographicSize > minZoom)
        {
            camera.orthographicSize -= 1;
        }
        else if (Input.mouseScrollDelta.y <= -0.5f && camera.orthographicSize < maxZoom)
        {
            camera.orthographicSize += 1;
        }
    }
}
