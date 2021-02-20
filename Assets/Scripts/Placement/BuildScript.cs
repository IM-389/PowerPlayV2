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

    List<RaycastHit2D> hitPoints = new List<RaycastHit2D>();

    public bool wireMode;

    public GameObject mouseObject;

    public GameObject wireObject1, wireObject2;

    LineRenderer lr;

    [Tooltip("Reference to the tooltip panel")]
    public GameObject tooltipPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        buildCircle = GameObject.FindWithTag("BuildCircle").transform;
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the mouse is over UI, ignore this function
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
            if (wireMode)
            {
                RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero);
                if (!hit.transform.CompareTag("Background"))
                {
                    // Sets the first wire object
                    if(wireObject1 == null)
                    {
                        wireObject1 = hit.transform.gameObject;
                        Debug.Log(wireObject1.GetComponent<GeneralObjectScript>().volts);
                    }
                    // Otherwise it sets the second wire object
                    else
                    {
                        wireObject2 = hit.transform.gameObject;
                        // Checks to make sure the same object isn't clicked twice
                        if (wireObject1 != wireObject2)
                        {
                            // If the first object is a transformer it can connect to anything
                            if (wireObject1.GetComponent<GeneralObjectScript>().GetVoltage() == 2)
                            {
                                CreateLine();
                            }
                            // Checks if the second object is either the same voltage as the first object or a transformer
                            else
                            {
                                // Checks if the second object is the same voltage as the first object
                                if ((wireObject1.GetComponent<GeneralObjectScript>().GetVoltage() == wireObject2.GetComponent<GeneralObjectScript>().GetVoltage() &&
                                    // Checks to make sure both objects aren't generators
                                    !(wireObject1.GetComponent<GeneralObjectScript>().isGenerator && wireObject2.GetComponent<GeneralObjectScript>().isGenerator) &&
                                    // Checks to make sure both objects aren't consumers
                                    !(wireObject1.GetComponent<GeneralObjectScript>().isConsumer && wireObject2.GetComponent<GeneralObjectScript>().isConsumer)) ||
                                    // Checks if second object is a transformer
                                    (wireObject2.GetComponent<GeneralObjectScript>().GetVoltage() == 2))
                                {
                                    CreateLine();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                PlaceableScript placeable = selectedBuilding.GetComponent<PlaceableScript>();

                bool blocked = false;
                RaycastHit2D origin = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero);
                // Raycasts  many dimensions depending on the object
                for (int i = 0; i > -placeable.dimensions.x; i--)
                {
                    for (int j = 0; j < placeable.dimensions.y; j++)
                    {
                        RaycastHit2D hitPoint = Physics2D.Raycast(mouseWorldPosRounded + new Vector2(i, j), Vector2.zero);
                        hitPoints.Add(hitPoint);
                    }
                }
                // Checks the list to make sure each raycast is hitting the background
                foreach (RaycastHit2D hitPoint in hitPoints)
                {
                    if (!hitPoint.transform.CompareTag("Background"))
                    {
                        blocked = true;
                    }
                }
                // If the raycast isn't blocked by a building, then place the building
                if (!blocked)
                {
                    Vector2 spawnPoint = RoundVector(origin.point);
                    GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                    Vector3 newPos = spawned.transform.position;
                    newPos.z = -1;
                    spawned.transform.position = newPos;
                }
                // Clear the list after its done
                hitPoints.Clear();
                
            }
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
        
        RaycastHit2D hitPt = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero);
        HoverScript hover = hitPt.transform.GetComponent<HoverScript>();

        if (hover != null)
        {
            hover.UpdateTooltip();
        }
        else
        {
            tooltipPanel.transform.position = new Vector2(1000, 1000);
        }
    }

    public void CreateLine()
    {
        // Creates line
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        myLine.transform.position = wireObject1.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .02f;
        lr.endWidth = .02f;
        lr.SetPosition(0, wireObject1.transform.position);
        lr.SetPosition(1, wireObject2.transform.position);
        // Creates connection from both objects
        wireObject1.GetComponent<GeneralObjectScript>().AddConnection(wireObject2);
        wireObject2.GetComponent<GeneralObjectScript>().AddConnection(wireObject1);
        // Sets objects back to null
        wireObject1 = null;
        wireObject2 = null;
        
    }
    private Vector2 RoundVector(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x);
        vec.y = (float) Math.Round(vec.y);
        return vec;
    }
    public void SelectWireMode()
    {
        wireMode = true;
        buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
    }
    public void DeselectWireMode()
    {
        wireMode = false;
        buildCircle.gameObject.SetActive(true);
        mouseObject.SetActive(true);
        wireObject1 = null;
        wireObject2 = null;
    }
    public void SelectNaturalGas()
    {
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("gasPlant"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectCoalPlant()
    {
        DeselectWireMode();
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
        DeselectWireMode();
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
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("solar"))
            {
                selectedBuilding = building;
            }
        }
    }
    // Only for Debugging
    public void SelectHouse()
    {
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("house"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectTransformer()
    {
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("transformer"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectLowPowerLines()
    {
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("Power"))
            {
                selectedBuilding = building;
            }
        }
    }
    public void SelectHighPowerLines()
    {
        DeselectWireMode();
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("HighPower"))
            {
                selectedBuilding = building;
            }
        }
    }
}
