using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildScript : MonoBehaviour
{

    private Camera mainCamera;

    private Transform buildCircle;

    public List<GameObject> spawnableBuildings = new List<GameObject>();

    public GameObject selectedBuilding;

    public List<RaycastHit2D> hitPoints = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        buildCircle = GameObject.FindWithTag("BuildCircle").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Get the mouses world position
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPosRounded = RoundVector(mouseWorldPos);
        
        // Move the build circle to the mouse, and snap it to the grid
        buildCircle.transform.position = mouseWorldPosRounded;
        //Helper.SnapToGrid(buildCircle);

        // If the player clicked the button, check if the cursor is over the background
        if (Input.GetMouseButtonDown(0))
        {
            PlaceableScript placeable = selectedBuilding.GetComponent<PlaceableScript>();
            
            bool blocked = false;
            RaycastHit2D origin = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero);
            // Raycast from the mouse position to the background
            for(int i = 0; i < placeable.dimensions.x; i++)
            {
                for(int j = 0; j < placeable.dimensions.y; j++)
                {
                    RaycastHit2D hitPoint = Physics2D.Raycast(mouseWorldPosRounded + new Vector2(i, j), Vector2.zero);
                    hitPoints.Add(hitPoint);
                }
            }

            foreach(RaycastHit2D hitPoint in hitPoints)
            {
                if (!hitPoint.transform.CompareTag("Background"))
                {
                    blocked = true;
                }
            }
            if (!blocked)
            {
                Vector2 spawnPoint = RoundVector(origin.point);
                GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                Vector3 newPos = spawned.transform.position;
                newPos.z = -1;
                spawned.transform.position = newPos;
            }
            hitPoints.Clear();
            
            /*
            RaycastHit2D hit;
            hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
            RaycastHit2D boxHit;
            boxHit = Physics2D.BoxCast(mouseWorldPos, placeable.dimensions, 0f, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Placeable"));
            Debug.Log("Hit: " + hit.transform.name);
            if (boxHit.transform != null)
                Debug.Log("BoxHit: " + boxHit.transform.name);
            // If the raycast hit the background, then the cursor is over the background 
            if (hit.transform.CompareTag("Background") && boxHit.transform is null)
            {
                Vector2 spawnPoint = RoundVector(hit.point);
                GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                Vector3 newPos = spawned.transform.position;
                newPos.z = -1;
                spawned.transform.position = newPos;
            }
            */

        }
    }

    private Vector2 RoundVector(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x);
        vec.y = (float) Math.Round(vec.y);
        return vec;
    }
    public void SelectNaturalGas()
    {
        foreach(GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("gasPlant"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectCoalPlant()
    {
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("coal"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectWindTurbine()
    {
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("turbine"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectSolarPanel()
    {
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("solar"))
            {
                selectedBuilding = building;
            }
        }
    }
    
}
