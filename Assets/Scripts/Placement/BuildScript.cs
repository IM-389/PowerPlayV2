using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    [Tooltip("Reference to the tooltip panel")]
    public GameObject tooltipPanel;
    
    private MoneyManager moneyManager;

    private string clip = "place";

    //LineRenderer lr;
    
    public Text errorText;
    
    [Tooltip("If the player isupgrade mode")]
    public bool upgradeMode;

    [Tooltip("If the player is in removal mode")]
    public bool removalMode;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        buildCircle = GameObject.FindWithTag("BuildCircle").transform;

        //lr = GetComponent<LineRenderer>();

        moneyManager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
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
            // If in wire/connection mode
            if (wireMode)
            {
                errorText.text = "";
                CreateWire(mouseWorldPosRounded);
            }
            else if (removalMode)
            {
                RaycastHit2D origin = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero);
                if (origin.transform.CompareTag("Generator") || origin.transform.CompareTag("transformer"))
                {
                    GeneralObjectScript gos = origin.transform.GetComponent<GeneralObjectScript>();
                    foreach (var connection in gos.connections)
                    {
                        connection.GetComponent<GeneralObjectScript>().connections.Remove(gos.gameObject);
                    }
                    Destroy(gos.gameObject);
                }
            }
            else
            {
                wireObject1 = null;
                wireObject2 = null;
                PlaceableScript placeable = selectedBuilding.GetComponent<PlaceableScript>();

                bool blocked = false;
                if (moneyManager.money >= placeable.cost)
                {
                    Debug.Log("under money > placeablecost");
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
                        Debug.Log("In the blocked if");
                        Vector2 spawnPoint = RoundVector(origin.point);
                        GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                        Vector3 newPos = spawned.transform.position;
                        newPos.z = -1;
                        spawned.transform.position = newPos;
                        SoundManager.PlaySound("place");
                        moneyManager.money -= placeable.cost;//determine we have the money and we're not blocked, so deduct the cizash
                        
                    }
                  
                    // Clear the list after its done
                    
                    hitPoints.Clear();
                }
            }
            // Old boxcast code
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
            Debug.Log(hitPt.transform.name);
            hover.UpdateTooltip();

            // If the player clicked on the object
            if (Input.GetMouseButtonDown(0))
            {
                // If they clicked on a consumer, make it smart
                // TODO: Tie a cost to this
                if (upgradeMode && hover.CompareTag("house") || hover.CompareTag("hospital") || hover.CompareTag("factory"))
                {
                    hover.isSmart = true;
                }
            }
        }
        else
        {
            tooltipPanel.transform.position = new Vector2(1000, 1000);
        }
    }

    public void CreateLine()
    {
        GeneralObjectScript wire1 = wireObject1.GetComponent<GeneralObjectScript>();
        GeneralObjectScript wire2 = wireObject2.GetComponent<GeneralObjectScript>();
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

        if (wire1.isConsumer)
        {
            wire1.AddConnection(wireObject2);
            wire2.AddConsumerConnection(wireObject1);
        }
        else if (wire2.isConsumer)
        {
            wire1.AddConsumerConnection(wireObject2);
            wire2.AddConnection(wireObject1);
        }
        else
        {
            // Creates connection from both objects
            wire1.AddConnection(wireObject2);
            wire2.AddConnection(wireObject1);
        }
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

    public void SelectUpgradeMode()
    {
        DeselectWireMode();
        upgradeMode = true;
        buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
    }

    public void SelectRemovalMode()
    {
        DeselectWireMode();
        removalMode = true;
        buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
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
        upgradeMode = false;
        if (wireObject1 != null)
        {
            wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }

        removalMode = false;

        buildCircle.gameObject.SetActive(true);
        mouseObject.SetActive(true);
        wireObject1 = null;
        wireObject2 = null;
    }
    public void SelectNaturalGas()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("gasPlant"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[2];
    }
    public void SelectCoalPlant()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("coal"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[0];
    }
    public void SelectWindTurbine()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("turbine"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[5];
    }
    public void SelectSolarPanel()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("solar"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[3];
    }
    // Only for Debugging
    public void SelectHouse()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("house"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[1];
    }
    public void SelectTransformer()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("transformer"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[4];
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
    public void SelectHospital()
    {
        {
            DeselectWireMode();
            foreach (GameObject building in spawnableBuildings)
            {
                if (building.CompareTag("hospital"))
                {
                    selectedBuilding = building;
                }
            }
        }
    }
    public void SelectFactory()
    {
        {
            DeselectWireMode();
            foreach (GameObject building in spawnableBuildings)
            {
                if (building.CompareTag("factory"))
                {
                    selectedBuilding = building;
                }
            }
        }
    }
    
    void CreateWire(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.transform.CompareTag("Background"))
        {
            if (wireObject1 != null) 
                wireObject1.GetComponent<SpriteRenderer>().color = Color.white;
            wireObject1 = null;
            return;
        }
        // Sets the first wire object
        if (wireObject1 == null)
        {
                wireObject1 = hit.transform.gameObject;
                wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                Debug.Log(wireObject1.GetComponent<GeneralObjectScript>().volts);
        }
        // Otherwise it sets the second wire object
        else
        {
            wireObject2 = hit.transform.gameObject;
            // Checks to make sure the same object isn't clicked twice
            if (wireObject1 == wireObject2)
            {
                errorText.text = "You can't click the same object twice";
                return;
            }
            Vector3 offset = wireObject1.transform.position - wireObject2.transform.position;
            Debug.Log(offset);
            float hypotenuse = Mathf.Sqrt( Mathf.Pow(Mathf.Abs(offset.x), 2) + Mathf.Pow(Mathf.Abs(offset.y),2));
            Debug.Log(hypotenuse);


            GeneralObjectScript wire1 = wireObject1.GetComponent<GeneralObjectScript>();
            GeneralObjectScript wire2 = wireObject2.GetComponent<GeneralObjectScript>();

            Debug.Log(wire1.connections.Count);
            Debug.Log(wire2.connections.Count);

            // Can't create a line longer than the wire length
            if(wire1.wireLength < hypotenuse)
            {
                errorText.text = "Wire cannot reach object";
                return;
            }

            // Checks and sees if connection is already made between both objects
            foreach (GameObject connect in wire1.connections)
            {
                if (connect == wireObject2)
                {
                    errorText.text = "Connnection is already made between these objects";
                    return;
                }
            }
            // Generators and consumers can only have one connection
            if (((wire1.isGenerator || wire1.isConsumer) && wire1.connections.Count >= wire1.maxConnectiions)|| ((wire2.isGenerator || wire2.isConsumer) && wire2.connections.Count >= wire2.maxConnectiions))
            {
                errorText.text = "One of these object can only have one connection";
                return;
            }
            // Objects besides the substation can't have any more than two connections
            if ((!wire1.isSubstation && wire1.connections.Count >= wire1.maxConnectiions) || (!wire2.isSubstation && wire2.connections.Count >= wire2.maxConnectiions))
            {
                errorText.text = "One of these object can only have two connections";
                return;
            }
            // Substations can only have a maximum of three connections
            if (wire1.connections.Count >= wire1.maxConnectiions || wire2.connections.Count >= wire2.maxConnectiions)
            {
                errorText.text = "One of these object can only have three connections";
                return;
            }

            
            // If the first object is a transformer it can connect to anything
            if (wire1.GetVoltage() == 2)
            {
                wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                CreateLine();
            }
            // Checks if the second object is either the same voltage as the first object or a transformer
            else
            {
                 // Checks if the second object is the same voltage as the first object
                 if ((wire1.GetVoltage() == wire2.GetVoltage() &&
                 // Checks to make sure both objects aren't generators
                 !(wire1.isGenerator && wire2.isGenerator) &&
                 // Checks to make sure both objects aren't consumers
                 !(wire1.isConsumer && wire2.isConsumer)) ||
                 // Checks if second object is a transformer
                 (wire2.GetVoltage() == 2))
                 {
                     wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                     CreateLine();
                 }
                 else if (wire1.GetVoltage() != wire2.GetVoltage())
                 {
                     errorText.text = "These objects don't have the same voltage";
                 }
                 else if (wire1.isGenerator && wire2.isGenerator)
                 {
                     errorText.text = "You cannot connect a generator to another generator";
                 }
                 else if (wire1.isConsumer && wire2.isConsumer)
                 {
                    errorText.text = "You cannot connect a consumer to another consumer";
                 }


            }

        }
        
    }
}
