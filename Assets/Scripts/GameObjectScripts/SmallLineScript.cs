using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmallLineScript : MonoBehaviour
{
    //locations of connected objects
    public GameObject[] connectedObjects = new GameObject[20];

    //the amount of objects connected via lines
    public uint amountOfConnectedObjects = 0;

    //the location of a connected object
    public Vector2 connectedObjectLocation;

    //is true when powered or provides power
    public bool connectedToPower = false;

    // Start is called before the first frame update
    void Start()
    {
        //snap to match grid
        Helper.SnapToGrid(this.transform);
        SoundManager.PlaySound("place1");
    }

    // Update is called once per frame
    void Update()
    {
        //determine if ded
        checkIfDead();

        searchForConnections();


        determineIfPowered();




        if (!BuildFunctions.simulationReset)
        {
            simulationReset();
        }

    }

    void checkIfDead()
    {
        if (null == BuildFunctions.playArea[(int)transform.position.x, (int)transform.position.y])
        {
            SoundManager.PlaySound("delete");
            Destroy(this.gameObject);
        }
    }

    void determineIfPowered()
    {
        for (int i = 0; i < amountOfConnectedObjects; i++)
        {
            if (connectedObjects[i] != null)
            {
                if (connectedObjects[i].name == "smallllines(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<SmallLineScript>().connectedToPower == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
                else if (connectedObjects[i].name == "solar(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<SolarScript>().powered == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
                else if (connectedObjects[i].name == "turbine(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<TurbineScript>().powered == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
                else if (connectedObjects[i].name == "coalCoolingTower(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<CoalScript>().powered == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
                else if (connectedObjects[i].name == "naturalgasplant(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<NaturalGasScript>().powered == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
                else if (connectedObjects[i].name == "house(Clone)" && connectedObjects[i] != null)
                {
                    if (connectedObjects[i].GetComponent<HomeScript>().connectedToPower == true)
                    {
                        connectedToPower = true;
                        break;
                    }
                    else
                    {
                        connectedToPower = false;
                    }
                }
            }
        }
    }

    void searchForConnections()
    {
        amountOfConnectedObjects = 0;
        Vector2 myLocation = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i <= BuildFunctions.lineNumber; i++)
        {

            if (myLocation == BuildFunctions.lineLocations[i])
            {
                //get location of other object
                if (i % 2 == 0)
                {

                    connectedObjectLocation = BuildFunctions.lineLocations[i + 1];

                    connectedObjects[amountOfConnectedObjects] = Helper.GetObjectFromLocation2d(connectedObjectLocation);

                    amountOfConnectedObjects++;
                }
                else if (i % 2 != 0)
                {

                    connectedObjectLocation = BuildFunctions.lineLocations[i - 1];

                    Helper.GetObjectFromLocation2d(connectedObjectLocation);
                    connectedObjects[amountOfConnectedObjects] = Helper.GetObjectFromLocation2d(connectedObjectLocation);
                    amountOfConnectedObjects++;
                }
            }
        }
    }



    public void SearchForPower(GameObject startObject, int stepLimit)
    {
        if (stepLimit > 0)
        {
            for (int i = 0; i < amountOfConnectedObjects; i++)
            {
                if (connectedObjects[i].name == "smallllines(Clone)")
                {
                    stepLimit--;
                    connectedObjects[i].GetComponent<SmallLineScript>().SearchForPower(startObject, stepLimit);
                }
                else if (connectedObjects[i].name == "solar(Clone)")
                {
                    if (connectedObjects[i].GetComponent<SolarScript>().power <= 0)
                    {
                        stepLimit--;
                        connectedObjects[i].GetComponent<SolarScript>().SearchForPower(startObject, stepLimit);
                    }
                    else
                    {
                        while (connectedObjects[i].GetComponent<SolarScript>().power > 0 && startObject.GetComponent<HomeScript>().neededPower < (startObject.GetComponent<HomeScript>().MaxneededPower))
                        {
                            connectedObjects[i].GetComponent<SolarScript>().power--;
                            startObject.GetComponent<HomeScript>().neededPower++;
                        }
                    }
                }
                else if (connectedObjects[i].name == "turbine(Clone)")
                {
                    if (connectedObjects[i].GetComponent<TurbineScript>().power <= 0)
                    {
                        stepLimit--;
                        connectedObjects[i].GetComponent<TurbineScript>().SearchForPower(startObject, stepLimit);
                    }
                    else
                    {
                        while (connectedObjects[i].GetComponent<TurbineScript>().power > 0 && startObject.GetComponent<HomeScript>().neededPower < (startObject.GetComponent<HomeScript>().MaxneededPower))
                        {
                            connectedObjects[i].GetComponent<TurbineScript>().power--;
                            startObject.GetComponent<HomeScript>().neededPower++;
                        }
                    }
                }
                else if (connectedObjects[i].name == "coalCoolingTower(Clone)")
                {
                    if (connectedObjects[i].GetComponent<CoalScript>().power <= 0)
                    {
                        stepLimit--;
                        connectedObjects[i].GetComponent<CoalScript>().SearchForPower(startObject, stepLimit);
                    }
                    else
                    {
                        while (connectedObjects[i].GetComponent<CoalScript>().power > 0 && startObject.GetComponent<HomeScript>().neededPower < (startObject.GetComponent<HomeScript>().MaxneededPower))
                        {
                            connectedObjects[i].GetComponent<CoalScript>().power--;
                            startObject.GetComponent<HomeScript>().neededPower++;
                        }
                    }
                }
                else if (connectedObjects[i].name == "naturalgasplant(Clone)")
                {
                    if (connectedObjects[i].GetComponent<NaturalGasScript>().power <= 0)
                    {
                        stepLimit--;
                        connectedObjects[i].GetComponent<NaturalGasScript>().SearchForPower(startObject, stepLimit);
                    }
                    else
                    {
                        while (connectedObjects[i].GetComponent<NaturalGasScript>().power > 0 && startObject.GetComponent<HomeScript>().neededPower < (startObject.GetComponent<HomeScript>().MaxneededPower))
                        {
                            connectedObjects[i].GetComponent<NaturalGasScript>().power--;
                            startObject.GetComponent<HomeScript>().neededPower++;
                        }
                    }
                }
                else if (connectedObjects[i].name == "house(Clone)")
                {
                    stepLimit--;
                    connectedObjects[i].GetComponent<HomeScript>().SearchForPower(startObject, stepLimit);
                }
            }
        }
    }

    void simulationReset()
    {
        
        //locations of connected objects
        Array.Clear(connectedObjects, 0, (int)++amountOfConnectedObjects);
        amountOfConnectedObjects = 0;
    }
}
