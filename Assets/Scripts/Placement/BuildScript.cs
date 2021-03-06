using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    int layerMask = ~(1 << 8);

    public GameObject errorBox;

    public Text errorText;
    
    [Tooltip("If the player isupgrade mode")]
    public bool upgradeMode;

    [Tooltip("If the player is in removal mode")]
    public bool removalMode;

    [Tooltip("Dropdown used for selecting objects")]
    public TMP_Dropdown selection;

    [Tooltip("Text used for displaying information about the selected object")]
    public Text selectedTooltipText;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        buildCircle = GameObject.FindWithTag("BuildCircle").transform;

        //lr = GetComponent<LineRenderer>();

        moneyManager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
        SetupDropdown();
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
                errorBox.SetActive(false);
                CreateWire(mouseWorldPos);
            }
            else if (removalMode)
            {
                wireObject1 = null;
                wireObject2 = null;
                RaycastHit2D origin = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
                Debug.Log(origin.transform.tag);
                RemoveObject(origin);
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
                    RaycastHit2D origin = Physics2D.Raycast(mouseWorldPosRounded, Vector2.zero, Mathf.Infinity, layerMask);
                    Debug.Log(origin.transform.gameObject.layer);
                    Debug.Log(origin.transform.tag);
                    // Raycasts  many dimensions depending on the object
                    for (int i = 0; i > -placeable.dimensions.x; i--)
                    {
                        for (int j = 0; j < placeable.dimensions.y; j++)
                        {
                            RaycastHit2D hitPoint = Physics2D.Raycast(mouseWorldPosRounded + new Vector2(i, j), Vector2.zero, Mathf.Infinity, layerMask);
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
                        spawnPoint -= selectedBuilding.GetComponent<PlaceableScript>().positionOffset;
                        Debug.Log(spawnPoint);
                        GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                        Vector3 newPos = spawned.transform.position;
                        // newPos.z = (float)(newPos.y*0.0001)-1; Possible solution for sprite layering
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
        
        RaycastHit2D hitPt = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        HoverScript hover = hitPt.transform.GetComponent<HoverScript>();

        if (hover != null)
        {
            //Debug.Log(hitPt.transform.name);
            hover.UpdateTooltip();

            // If the player clicked on the object
            if (Input.GetMouseButtonDown(0))
            {
                // If they clicked on a consumer, make it smart
                // TODO: Tie a cost to this
                if (upgradeMode && hover.CompareTag("house") || hover.CompareTag("hospital") || hover.CompareTag("factory"))
                {
                    hover.isSmart = true;
                    hover.transform.GetChild(5).gameObject.SetActive(true);
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
        // Adds connection from wire 1 to wire 2
        if (wire1.volts == GeneralObjectScript.Voltage.LOW && (!wire2.isConsumer))
        {
            wire1.AddLVConnection(wireObject2);
        }
        else if(wire1.volts == GeneralObjectScript.Voltage.LOW && wire2.isConsumer)
        {
            wire1.AddConsumerConnection(wireObject2);
        }
        else
        {
            if ((wire1.volts == GeneralObjectScript.Voltage.TRANSFORMER &&
                wire2.volts == GeneralObjectScript.Voltage.HIGH) || wire1.volts == GeneralObjectScript.Voltage.HIGH)
            {
                wire1.AddHVConnection(wireObject2);
            }
            else
            {
                wire1.AddLVConnection(wireObject2);
            }
        }
        // Adds connection from wire 2 to wire 1
        if (wire2.volts == GeneralObjectScript.Voltage.LOW && (!wire1.isConsumer))
        {
            wire2.AddLVConnection(wireObject1);
        }
        else if (wire2.volts == GeneralObjectScript.Voltage.LOW && wire1.isConsumer)
        {
            wire2.AddConsumerConnection(wireObject1);
        }
        else
        {
            if ((wire2.volts == GeneralObjectScript.Voltage.TRANSFORMER &&
                wire1.volts == GeneralObjectScript.Voltage.HIGH) || wire2.volts == GeneralObjectScript.Voltage.HIGH)
            {
                wire2.AddHVConnection(wireObject1);
            }
            else
            {
                wire2.AddLVConnection(wireObject1);
            }
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

    /// <summary>
    /// Used to load the dropdown with the correct objects
    /// </summary>
    public void SetupDropdown()
    {
        selection.ClearOptions();
        List<string> dropdownOptions = new List<string>();
        dropdownOptions.Add("--SELECT TOOL--");
        foreach (var building in spawnableBuildings)
        {
            dropdownOptions.Add(building.GetComponent<PlaceableScript>().buildingName);
        }
        dropdownOptions.Add("Wire Mode");
        dropdownOptions.Add("Upgrade Mode");
        dropdownOptions.Add("Removal Mode");
        selection.AddOptions(dropdownOptions);
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

    /// <summary>
    /// Used to update the selected tool from the dropdown
    /// </summary>
    public void UpdateSelection()
    {
        // Get the value of the dropdown
        int selected = selection.value - 1;
        //Strings for item tooltips. We can futz with the exact text later
    
        // If the value is somewhere in the spawnable buildings list, then select that building
        if (selected >= 0 && selected < spawnableBuildings.Count)
        {
            DeselectWireMode();
            selectedBuilding = spawnableBuildings[selected];
            GeneralObjectScript sGos;
            // TODO: Replace once new pole art is in
            try
            {
                sGos = selectedBuilding.GetComponent<BuildingSpawn>().Building.GetComponent<GeneralObjectScript>();
            }
            catch (NullReferenceException e)
            {
                sGos = selectedBuilding.GetComponent<GeneralObjectScript>();
            }

            string tooltipInfo = "";
            tooltipInfo += sGos.buildingText + "\n";
            if (sGos.isGenerator)
            {
                GeneratorScript generator = sGos.GetComponent<GeneratorScript>();
                
                tooltipInfo += $"\nGeneration: {generator.amount}\n";
                
            }

            tooltipInfo += $"Cost: {sGos.cost}\nRange: {sGos.wireLength}\n";
            tooltipInfo += $"HV Connections: {sGos.maxHVConnections}\n";
            tooltipInfo += $"LV Connections: {sGos.maxLVConnections}\n";
            
            
          
            selectedTooltipText.text = tooltipInfo;
        }
        else
        {
            // Otherwise, pick from one of the tools
            int difference = selected - spawnableBuildings.Count;
            // 0 is wire mode, 1 is upgrade mode, 2 is removal mode
            switch (difference)
            {
                case 0:
                    selectedTooltipText.text = "Carries electricity like a water pipe.";
                    SelectWireMode();
                    break;
                case 1:
                    SelectUpgradeMode();
                    break;
                case 2:
                    SelectRemovalMode();
                    break;
            }
        }
    }
    
    void CreateWire(Vector2 mousePos)
    {
        string tooltipWire = "";
        GeneralObjectScript sWire;
        //tooltipWire += sWire.buildingText;
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        //Debug.Log(hit.transform.tag);
        if (hit.transform.CompareTag("Background") || hit.collider is null || hit.transform.CompareTag("Road"))
        {
            if (wireObject1 != null) 
                wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            wireObject1 = null;
            return;
        }
        // Sets the first wire object
        if (wireObject1 == null)
        {
            if (hit.transform.CompareTag("wire"))
            {
                Debug.Log(hit.transform.gameObject.GetComponent<BoxCollider2D>().size);
                return;
            }            
            wireObject1 = hit.transform.gameObject;
            wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            //Debug.Log(wireObject1.GetComponent<GeneralObjectScript>().volts);            
        }

        // Otherwise it sets the second wire object
        else
        {
            if (hit.transform.CompareTag("wire"))
            {
                return;
            }
            wireObject2 = hit.transform.gameObject;
            // Checks to make sure the same object isn't clicked twice
            if (wireObject1 == wireObject2)
            {
                errorBox.SetActive(true);
                errorText.text = "You can't click the same object twice";
                return;
            }
            Vector3 offset = wireObject1.transform.position - wireObject2.transform.position;
            Debug.Log(offset);
            float hypotenuse = Mathf.Sqrt( Mathf.Pow(Mathf.Abs(offset.x), 2) + Mathf.Pow(Mathf.Abs(offset.y),2));
            Debug.Log(hypotenuse);
            tooltipWire += "Joins buildings to give electricity to each other.";

            GeneralObjectScript wire1 = wireObject1.GetComponent<GeneralObjectScript>();
            GeneralObjectScript wire2 = wireObject2.GetComponent<GeneralObjectScript>();

            //Debug.Log(wire1.connections.Count);
            //Debug.Log(wire2.connections.Count);

            // Can't create a line longer than the wire length
            if(wire1.wireLength < hypotenuse)
            {
                errorBox.SetActive(true);
                errorText.text = "Wire cannot reach object";
                return;
            }

            // Checks and sees if connection is already made between both objects
            foreach (GameObject connect in wire1.lvConnections)
            {
                if (connect == wireObject2)
                {
                    errorBox.SetActive(true);
                    errorText.text = "Connnection is already made between these objects";
                    return;
                }
            }
            foreach (GameObject connect in wire1.hVConnections)
            {
                if (connect == wireObject2)
                {
                    errorBox.SetActive(true);
                    errorText.text = "Connnection is already made between these objects";
                    return;
                }
            }
            foreach (GameObject connect in wire1.consumerConnections)
            {
                if (connect == wireObject2)
                {
                    errorBox.SetActive(true);
                    errorText.text = "Connnection is already made between these objects";
                    return;
                }
            }

            if (((wire1.volts == GeneralObjectScript.Voltage.HIGH || (wire1.volts == GeneralObjectScript.Voltage.TRANSFORMER && wire2.volts == GeneralObjectScript.Voltage.HIGH)) && (wire1.hVConnections.Count >= wire1.maxHVConnections || wire2.hVConnections.Count >= wire2.maxHVConnections)))
            {
                errorBox.SetActive(true);
                errorText.text = "Too many high voltage connections on one object!";
                return;
            }
            
            if (((wire1.volts == GeneralObjectScript.Voltage.LOW || (wire1.volts == GeneralObjectScript.Voltage.TRANSFORMER && wire2.volts == GeneralObjectScript.Voltage.LOW)) && (wire1.lvConnections.Count >= wire1.maxLVConnections || wire2.lvConnections.Count >= wire2.maxLVConnections)))
            {
                errorBox.SetActive(true);
                errorText.text = "Too many low voltage connections on one object!";
                return;
            }
            
            /*
            // Generators and consumers can only have one connection
            if (((wire1.isGenerator || wire1.isConsumer) && wire1.connections.Count >= wire1.maxHVConnections)|| ((wire2.isGenerator || wire2.isConsumer) && wire2.connections.Count >= wire2.maxHVConnections))
            {
                errorText.text = "One of these object can only have one connection";
                return;
            }
            // Objects besides the substation can't have any more than two connections
            if ((!wire1.isSubstation && wire1.connections.Count >= wire1.maxHVConnections) || (!wire2.isSubstation && wire2.connections.Count >= wire2.maxHVConnections))
            {
                errorText.text = "One of these object can only have two connections";
                return;
            }
            // Substations can only have a maximum of three connections
            if (wire1.connections.Count >= wire1.maxHVConnections || wire2.connections.Count >= wire2.maxHVConnections)
            {
                errorText.text = "One of these object can only have three connections";
                return;
            }
            */
            
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
                    errorBox.SetActive(true);
                    errorText.text = "These objects don't have the same voltage";
                 }
                 else if (wire1.isGenerator && wire2.isGenerator)
                 {
                    errorBox.SetActive(true);
                    errorText.text = "You cannot connect a generator to another generator";
                 }
                 else if (wire1.isConsumer && wire2.isConsumer)
                 {
                    errorBox.SetActive(true);
                    errorText.text = "You cannot connect a consumer to another consumer";
                 }
            }
        }
    }

    public void RemoveObject(RaycastHit2D origin)
    {
        if (origin.transform.CompareTag("Generator") || origin.transform.CompareTag("transformer") || origin.transform.CompareTag("Power") || origin.transform.CompareTag("HighPower"))
        {
            GeneralObjectScript gos = origin.transform.GetComponent<GeneralObjectScript>();
            // Doesn't remove it if the object is unremovable
            if (gos.unRemovable)
            {
                return;
            }
            List<GameObject> allConnections = new List<GameObject>();
            allConnections.AddRange(gos.hVConnections);
            allConnections.AddRange(gos.lvConnections);
            allConnections.AddRange(gos.consumerConnections);
            foreach (var connection in allConnections)
            {
                connection.GetComponent<GeneralObjectScript>().RemoveConnection(gos.gameObject);
                gos.RemoveConnection(connection.gameObject);
            }

            moneyManager.money += gos.cost;
            Destroy(gos.gameObject);
        }
        else if (origin.transform.CompareTag("wire"))
        {
            WireScript ws = origin.transform.parent.GetComponent<WireScript>();
            GameObject object1 = ws.connect1;
            GameObject object2 = ws.connect2;
            GeneralObjectScript gos1 = object1.GetComponent<GeneralObjectScript>();
            GeneralObjectScript gos2 = object2.GetComponent<GeneralObjectScript>();
            gos1.RemoveConnection(object2);
            gos2.RemoveConnection(object1);
        }
    }
}
