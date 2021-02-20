﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalScript : MonoBehaviour
{
    public bool powered = true;

    public int pollution = 10;

    public static int cost = 40;

    //the max amount of providable power
    public uint maxPower = 20;

    //the amount of providable power
    public uint power = 20;

    public Phase2Manager phase2;

    public static int upkeep = 10;

    //locations of connected objects
    public GameObject[] connectedObjects = new GameObject[20];

    public uint amountOfConnectedObjects = 0;

    public Vector2 connectedObjectLocation;

    public uint daysToBeBroken = 0;

    void Awake()
    {
        phase2 = FindObjectOfType<Phase2Manager>();
        //snap to match grid
        Helper.SnapToGrid(this.transform);
        //cost = price;
        Phase2Manager.currency -= cost;
        phase2.UpdateCurrency();

        SoundManager.PlaySound("place2");
    }

    // Update is called once per frame
    void Update()
    {
        checkIfDead();

        searchForConnections();

        if (BuildFunctions.simulationReset)
        {
            //no need for this right? nothing's here. maybe put a debug.log to see what's up.
        }
        else
        {
            SimulationReset();
        }

    }

    void checkIfDead()
    {
        if (null == BuildFunctions.playArea[(int)transform.position.x, (int)transform.position.y])
        {
            //return some cost of this item from the total currency
            Phase2Manager.currency += cost;
            //update the currency ui element
            phase2.UpdateCurrency();
            SoundManager.PlaySound("delete");

            Destroy(this.gameObject);
        }
    }
    void SimulationReset()
    {
        if (daysToBeBroken == 0)
        {
            power = maxPower;
        }
        else
        {
            power = 0;
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
    {//there's a LOT here-might be best to split them into seperate functions
        if (stepLimit > 0)
        {
            for (int i = 0; i < amountOfConnectedObjects; i++)
            {
                if (connectedObjects[i].name == "smallllines(Clone)")
                {
                    stepLimit--;
                    connectedObjects[i].GetComponent<SmallLineScript>().SearchForPower(startObject, stepLimit);
                }
                else if (connectedObjects[i].name == "solar(Clone)")//interesting way to handle this-we may wanna keep this in mind going forward if we need to look for specific objects.
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


    
}