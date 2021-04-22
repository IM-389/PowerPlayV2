using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    public float dragSpeed = 2;
    public float buttonSpeed = 5f;

    private Vector3 dragOrigin;

    private Camera camera;

    public KeyCode zoomIn, zoomOut;

    public GameObject Griddie;
    Vector3 startPosGriddie;

    public float minZoom;
    public float maxZoom;

    [Tooltip("Minimum (lowest) values the camera's coordinates can be")]
    public Vector2 minCameraPos;
    
    [Tooltip("Maximum (highest) values the camera's coordinates cam be")]
    public Vector2 maxCameraPos;


    //public GameObject arrowManager;
    ArrowManager am;
    private void Start()
    {
        //am = arrowManager.GetComponent<ArrowManager>();
        camera = Camera.main;
        startPosGriddie = Griddie.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float speed = dragSpeed * Time.fixedDeltaTime;
            camera.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed);
            
        }
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        camera.transform.Translate(new Vector3(xAxis*buttonSpeed* Time.fixedDeltaTime, yAxis*buttonSpeed*Time.fixedDeltaTime, 0));

        // Limits camera to certain position range
        /*Vector3 newPos = camera.transform.position;
        newPos.x = Mathf.Clamp(newPos.x, minCameraPos.x, maxCameraPos.x);
        newPos.y = Mathf.Clamp(newPos.y, minCameraPos.y, maxCameraPos.y);
        camera.transform.position = newPos; */
        camera.transform.position = ClampCamera(camera.transform.position);
        if ((Input.mouseScrollDelta.y > 0.5f || Input.GetKeyDown(zoomIn)) && camera.orthographicSize > minZoom)
        {
            camera.orthographicSize -= 1;
            Griddie.transform.localScale -= new Vector3(0.14f, 0.14f);
            Griddie.transform.position = startPosGriddie;
            Vector3 pos = Camera.main.WorldToViewportPoint(Griddie.transform.position);
            pos.x = 0.24f;
            pos.y = 0.1f;
            Griddie.transform.position = Camera.main.ViewportToWorldPoint(pos);
            //am.dis -= 0.14f;
        }
        else if ((Input.mouseScrollDelta.y <= -0.5f || Input.GetKeyDown(zoomOut)) && camera.orthographicSize < maxZoom)
        {
            camera.orthographicSize += 1;
            Griddie.transform.localScale += new Vector3(0.14f, 0.14f);
            Griddie.transform.position = startPosGriddie;
            Vector3 pos = Camera.main.WorldToViewportPoint(Griddie.transform.position);
            pos.x = 0.24f;
            pos.y = 0.1f;
            Griddie.transform.position = Camera.main.ViewportToWorldPoint(pos);
            //am.dis += 0.14f;
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float cameraHeight = camera.orthographicSize;
        float cameraWidth = camera.orthographicSize * camera.aspect;
        float minX = minCameraPos.x + cameraWidth;
        float maxX = maxCameraPos.x - cameraWidth;
        float minY = minCameraPos.y + cameraHeight;
        float maxY = maxCameraPos.y - cameraHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
