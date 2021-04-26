using System;
using System.Collections.Generic;
using Power.V2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildScript : MonoBehaviour
{

    private Camera mainCamera;

    //private Transform buildCircle;

    public List<GameObject> spawnableBuildings = new List<GameObject>();

    public GameObject selectedBuilding;

    List<RaycastHit2D> hitPoints = new List<RaycastHit2D>();

    public bool wireMode;

    public GameObject mouseObject;

    public GameObject wireObject1, wireObject2;

    public GameObject dirtEmission;

    public GameObject sparkEmission;

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

    [Tooltip("Text used for displaying information about the selected object")]
    public Text selectedTooltipText;

    private TimeManager cityApproval;

    private string placeSound = "event:/Sound Effects/Place";

    public double totalBuildingsPlaced = 0;//increment this no matter what's placed. Divide it by clean and dirty seperately, check if the number is > a certain percentage of the total buildings AFTER a certain milestone.

    public double totalDirtyPowerPlaced = 0;//way im thinkin rn is to just use this variable to increment for coal/other "dirty" sources as opposed to tracking like 3 variables

    public double totalCleanPowerPlaced = 0; //same logic as above

    public double totalCoalPlaced = 0;

    public double totalGasPlaced = 0;

    public double totalSolarPlaced = 0;

    public double totalWindmillPlaced = 0;

    public bool trackApproval;

    //to deselect buttons
    public SlideOutUI po;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        //buildCircle = GameObject.FindWithTag("BuildCircle").transform;

        //lr = GetComponent<LineRenderer>();
        cityApproval = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        moneyManager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
        //();

        //find the slide out UI
        po = GameObject.FindObjectOfType<SlideOutUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateSatisfaction();
        // If the mouse is over UI, ignore this function
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Get the mouses world position
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPosRounded = RoundVector(mouseWorldPos);
        
        // Move the build circle to the mouse, and snap it to the grid
        //buildCircle.transform.position = mouseWorldPosRounded;
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
            else if (!upgradeMode)
            {
                wireObject1 = null;
                wireObject2 = null;
                if (selectedBuilding != null)
                {
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
                            //Debug.Log(spawnPoint);
                            GameObject spawned = Instantiate(selectedBuilding, spawnPoint, Quaternion.identity);
                            Vector3 newPos = spawned.transform.position;
                            FMODUnity.RuntimeManager.PlayOneShot(placeSound);
                            // newPos.z = (float)(newPos.y*0.0001)-1; Possible solution for sprite layering
                            newPos.z = -1;
                            spawned.transform.position = newPos;
                            moneyManager.money -= placeable.cost;//determine we have the money and we're not blocked, so deduct the cizash

                            if (selectedBuilding.CompareTag("coal"))
                            {
                                totalBuildingsPlaced++;
                                totalDirtyPowerPlaced++;
                                totalCoalPlaced++;
                                //check if day has finished, like in ConsumerScript, then do calculation for % of dirty vs clean-be sure to tell doug so he can help with milestone part
                            }
                            else if (selectedBuilding.CompareTag("solar"))
                            {
                                totalBuildingsPlaced++;
                                totalCleanPowerPlaced++;
                                totalSolarPlaced++;
                            }
                            else if (selectedBuilding.CompareTag("turbine"))
                            {
                                totalBuildingsPlaced++;
                                totalCleanPowerPlaced++;
                                totalWindmillPlaced++;
                            }
                        }

                        // Clear the list after its done

                        hitPoints.Clear();
                    }
                }
            }

        }
        // When you right click, set things to inactive
        else if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Deselected buildings");
            po.MakeButtonsWork();
            DeselectWireMode();
            selectedBuilding = null;
            removalMode = false;
            mouseObject.SetActive(false);
        }
        RaycastHit2D hitPt = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        HoverScript hover = hitPt.transform.GetComponent<HoverScript>();

        if (hover != null)
        {
            //Debug.Log(hitPt.transform.name);
            hover.UpdateTooltip();
            hover.ToggleBuildCircle(true);
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
        if (wire1.isConsumer)
        {
            if (wire2.volts == GeneralObjectScript.Voltage.LOW)
            {
                wire1.AddNonConsumerConnection(wireObject2);
                wire2.AddConsumerConnection(wireObject1);
            }
        } 
        else if (wire2.isConsumer)
        {
            if (wire1.volts == GeneralObjectScript.Voltage.LOW)
            {
                wire2.AddNonConsumerConnection(wireObject1);
                wire1.AddConsumerConnection(wireObject2);
            }
        }
        else
        {
            wire1.AddNonConsumerConnection(wireObject2);
            wire2.AddNonConsumerConnection(wireObject1);
        }
        
        // Sets objects back to nulls
        wireObject1.layer = 0;
        wireObject2.layer = 0;
        wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);

        wireObject1 = wireObject2;
        wireObject1.GetComponent<RecolorScript>().Recolor(Color.blue);
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
        //buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
    }

    public void SelectRemovalMode()
    {
        DeselectWireMode();
        removalMode = true;
        //buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
    }
    
    public void SelectWireMode()
    {
        wireMode = true;
        //buildCircle.gameObject.SetActive(false);
        mouseObject.SetActive(false);
    }
    public void DeselectWireMode()
    {
        wireMode = false;
        upgradeMode = false;
        if (wireObject1 != null)
        {
            wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);
            wireObject1.layer = 0;
        }

        if (wireObject2 != null)
        {
            wireObject2.GetComponent<RecolorScript>().Recolor(Color.white);
            wireObject2.layer = 0;
        }
        
        removalMode = false;

        //buildCircle.gameObject.SetActive(true);
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
        selectedBuilding = spawnableBuildings[1];
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
        selectedBuilding = spawnableBuildings[6];
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
        selectedBuilding = spawnableBuildings[4];
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
        selectedBuilding = spawnableBuildings[2];
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
        selectedBuilding = spawnableBuildings[3];
    }
    public void SelectLowPowerLines()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("Power"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[0];
    }
    public void SelectHighPowerLines()
    {
        DeselectWireMode();
        /*
        foreach (GameObject building in spawnableBuildings)
        {
            if (building.CompareTag("HighPower"))
            {
                selectedBuilding = building;
            }
        }
        */
        selectedBuilding = spawnableBuildings[2];
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
    public void SelectSubstation()
    {
        DeselectWireMode();
        /*
        {
            foreach (GameObject building in spawnableBuildings)
            {
                if (building.CompareTag("factory"))
                {
                    selectedBuilding = building;
                }
            }
        }
        */
        selectedBuilding = spawnableBuildings[5];
    }

    /// <summary>
    /// Used to update the selected tool from the dropdown
    /// </summary>
    public void UpdateSelection()
    {
        // Get the value of the dropdown
        int selected = 0;// selection.value - 1;
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
                PowerAmountInfo amountInfo = sGos.GetComponent<PowerAmountInfo>();
                
                tooltipInfo += $"\nGeneration: {amountInfo.amountGenerated}\n";
                
            }

            tooltipInfo += $"Cost: {sGos.refundAmount}\nRange: {sGos.wireLength}\n";
            tooltipInfo += $"Connections: {sGos.maxConnections}\n";
            //tooltipInfo += $"LV Connections: {sGos.maxLVConnections}\n";
            
            

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
        Debug.Log(hit.transform.tag);
        if (hit.transform.CompareTag("Background") || hit.collider is null || hit.transform.CompareTag("Road") || hit.transform.CompareTag("tree") || hit.transform.CompareTag("wire") || hit.transform.CompareTag("fog"))
        {
            if (wireObject1 != null)
            {
                wireObject1.layer = 0;
                wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);
            }
            wireObject1 = null;
            return;
        }
        // Sets the first wire object
        if (wireObject1 == null)
        {
            wireObject1 = hit.transform.gameObject;
            wireObject1.layer = 2;
            wireObject1.GetComponent<RecolorScript>().Recolor(Color.blue);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Object Select");
        }

        // Otherwise it sets the second wire object
        else
        {
            wireObject2 = hit.transform.gameObject;
            FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Object Select");
            // Checks to make sure the same object isn't clicked twice
            if (wireObject1 == wireObject2)
            {
                errorBox.SetActive(true);
                errorText.text = "You can't click the same object twice";
                /*
                if (wireObject1.tag == "house")
                {
                    wireObject1.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    wireObject1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                }

                wireObject1 = null;
                wireObject2 = null;
                */
                return;
            }
            //Vector3 offset = wireObject1.transform.position - wireObject2.transform.position;
            //Debug.Log(offset);
            //float hypotenuse = Mathf.Sqrt( Mathf.Pow(Mathf.Abs(offset.x), 2) + Mathf.Pow(Mathf.Abs(offset.y),2));
            //Debug.Log(hypotenuse);
            //tooltipWire += "Joins buildings to give electricity to each other.";

            GeneralObjectScript wire1 = wireObject1.GetComponent<GeneralObjectScript>();
            GeneralObjectScript wire2 = wireObject2.GetComponent<GeneralObjectScript>();
            

            // Checks and sees if connection is already made between both objects
            /*
            foreach (GameObject connect in wire1.lvConnections)
            {
                if (connect == wireObject2)
                {
                    errorBox.SetActive(true);
                    errorText.text = "Connnection is already made between these objects";
                    return;
                }
            }
            */
            foreach (GameObject connect in wire1.nonConsumerConnections)
            {
                if (connect == wireObject2)
                {
                    wireObject1.layer = 0;
                    wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);
                    wireObject1 = wireObject2;
                    wireObject1.GetComponent<RecolorScript>().Recolor(Color.blue);
                    return;
                }
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Object Select");
            }
            foreach (GameObject connect in wire1.consumerConnections)
            {
                if (connect == wireObject2)
                {
                    wireObject1.layer = 0;
                    wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);
                    wireObject1 = wireObject2;
                    wireObject1.GetComponent<RecolorScript>().Recolor(Color.blue);
                    return;
                }
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Object Select");
            }
            
            wireObject2.layer = 11;
            LayerMask searchingMask = 1 << 11;
            RaycastHit2D hit2d = Physics2D.Linecast(wire1.transform.position, wire2.transform.position, searchingMask);

            if (hit2d.collider != null)
            {
                Debug.Log($"Hit {hit2d.transform.name}", hit2d.transform.gameObject);
                Vector2 offset = (Vector2)wire1.transform.position - hit2d.point;
                float sqrHypotenuse = offset.sqrMagnitude;
                float sqrLength = wire1.wireLength * wire1.wireLength;
                // Can't create a line longer than the wire length
                if (sqrLength < sqrHypotenuse)
                {
                    errorBox.SetActive(true);
                    errorText.text = "Wire cannot reach object";
                    wireObject2.layer = 0;
                    return;
                }
            }
            
            if((wireObject1.CompareTag("Substation") && wire2.isConsumer) || (wire1.isConsumer && wireObject2.CompareTag("Substation")))
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "Cannot connect substation to a consumer";
                return;
            }
            if(wire1.isConsumer && wire2.isConsumer)
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "Cannot create a connection between consumers";
                return;
            }
            // Makes sure that the objects don't connect to more than they are allowed. First condition is because
            // connections to consumers don't count towards the limit
            if (!(wire1.isConsumer || wire2.isConsumer) && (wire1.nonConsumerConnections.Count >= wire1.maxConnections || wire2.nonConsumerConnections.Count >= wire2.maxConnections))
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "Too many connections on one object!";
                return;
            }
            if((wire1.isConsumer && !wire2.isConsumer) && wire1.nonConsumerConnections.Count >= wire1.maxConnections)
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "Too many connections on one object!";
                return;
            }
            if((wire2.isConsumer && !wire1.isConsumer) && wire2.nonConsumerConnections.Count >= wire2.maxConnections)
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "Too many connections on one object!";
                return;
            }
            if ((wire1.volts == GeneralObjectScript.Voltage.HIGH && wire2.isConsumer)
                || wire2.volts == GeneralObjectScript.Voltage.HIGH && wire1.isConsumer)
            {
                wireObject2.layer = 0;
                errorBox.SetActive(true);
                errorText.text = "You cannot connect this generator directly to a consumer!";
                return;
            }
            wireObject1.layer = 0;
            wireObject2.layer = 0;
            wireObject1.GetComponent<RecolorScript>().Recolor(Color.white);
            CreateLine();
        }
    }

    public void RemoveObject(RaycastHit2D origin)
    {
        if (origin.transform.CompareTag("Generator") || origin.transform.CompareTag("transformer") 
            || origin.transform.CompareTag("Power") || origin.transform.CompareTag("HighPower")
            || origin.transform.CompareTag("Substation"))
        {
            GeneralObjectScript gos = origin.transform.GetComponent<GeneralObjectScript>();
            // Doesn't remove it if the object is unremovable
            if (gos.unRemovable)
            {
                return;
            }
            List<GameObject> allConnections = new List<GameObject>();
            allConnections.AddRange(gos.nonConsumerConnections);
            //allConnections.AddRange(gos.lvConnections);
            allConnections.AddRange(gos.consumerConnections);
            foreach (var connection in allConnections)
            {
                connection.GetComponent<GeneralObjectScript>().RemoveConnection(gos.gameObject);
                gos.RemoveConnection(connection.gameObject);
            }

            moneyManager.money += gos.refundAmount;
            Instantiate(dirtEmission, new Vector3(gos.gameObject.transform.position.x,
            gos.gameObject.transform.position.y, 0), Quaternion.identity);
            Destroy(gos.gameObject);
        }
        else if (origin.transform.CompareTag("wire"))
        {

            WireScript ws = origin.transform.parent.GetComponent<WireScript>();
            GameObject object1 = ws.connect1;
            GameObject object2 = ws.connect2;

            Instantiate(sparkEmission, new Vector3((object1.gameObject.transform.position.x + 
            object2.gameObject.transform.position.x) / 2,
            (object1.gameObject.transform.position.y + object2.gameObject.transform.position.y) / 2,
            0), Quaternion.identity);

            GeneralObjectScript gos1 = object1.GetComponent<GeneralObjectScript>();
            GeneralObjectScript gos2 = object2.GetComponent<GeneralObjectScript>();
            gos1.RemoveConnection(object2);
            gos2.RemoveConnection(object1);
        }
    }

    // Old satisfaction update
    /*
    private void UpdateSatisfaction()
    {

        //do math calculations for % amount of each and check if day ends. kinda scuff, but should work
        if (trackApproval)
        {
            if (cityApproval.hours >= 25)
            {
                if (totalDirtyPowerPlaced / totalBuildingsPlaced >= 1)
                {
                    cityApproval.cityApproval -= 35;
                }
                else if(totalDirtyPowerPlaced / totalBuildingsPlaced >= 0.90)
                {
                    cityApproval.cityApproval -= 20;
                }
                else if (totalDirtyPowerPlaced / totalBuildingsPlaced >= 0.75)
                {
                    cityApproval.cityApproval -= 10;
                    if (totalCoalPlaced / totalBuildingsPlaced > 0.50)
                    {
                        cityApproval.cityApproval -= 15;
                    }
                    else if (totalCoalPlaced / totalBuildingsPlaced > 0.90)
                    {
                        cityApproval.cityApproval -= 20;
                    }
                    /*
                    else if(totalGasPlaced / totalBuildingsPlaced > 0.50)
                    {
                        cityApproval.cityApproval -= 10;
                    }
                    //
                }

                if (totalCleanPowerPlaced / totalBuildingsPlaced >= 1)
                {
                    cityApproval.cityApproval += 35;
                }
                else if (totalCleanPowerPlaced / totalBuildingsPlaced >= 0.90)
                {
                    cityApproval.cityApproval += 20;
                }
                else if (totalCleanPowerPlaced / totalBuildingsPlaced >= 0.75)
                {
                    cityApproval.cityApproval += 10;
                    if (totalSolarPlaced / totalBuildingsPlaced > 0.50)
                    {
                        cityApproval.cityApproval += 15;
                    }
                    else if (totalWindmillPlaced / totalBuildingsPlaced > 0.50)
                    {
                        cityApproval.cityApproval += 10;
                    }
                }
                
                
            }
        }
    }
    */
}
