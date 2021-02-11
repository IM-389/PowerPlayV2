using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildScript : MonoBehaviour
{

    private Camera mainCamera;

    private Transform buildCircle;

    public List<GameObject> spawnableBuildings = new List<GameObject>();

    public GameObject selectedBuilding;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        buildCircle = GameObject.FindWithTag("BuildCircle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouses world position
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPosRounded = RoundVector(mouseWorldPos);
        
        // Move the build circle to the mouse, and snap it to the grid
        buildCircle.transform.position = mouseWorldPosRounded;
        Helper.SnapToGrid(buildCircle);

        // If the player clicked the button, check if the cursor is over the background
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast from the mouse position to the background
            RaycastHit2D hit;
            hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            Debug.Log(hit.transform.name);
            Debug.DrawLine(mouseWorldPos, hit.point);
            // If the raycast hit the background, then the cursor is over the background 
            if (hit.transform.CompareTag("Background"))
            {
                Vector2 spawnPoint = RoundVector(hit.point);
                GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                Vector3 newPos = spawned.transform.position;
                newPos.z = -1;
                spawned.transform.position = newPos;
            }
        }
    }

    private Vector2 RoundVector(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x);
        vec.y = (float) Math.Round(vec.y);
        return vec;
    }
}
