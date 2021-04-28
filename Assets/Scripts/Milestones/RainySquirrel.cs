using System.Collections;
using System.Collections.Generic;
using Milestones;
using Power.V2;
using UnityEngine;

public class RainySquirrel : MilestoneBase
{
    private RandomEventManager eventManager;

    private bool ranEvent = false;
    
    private void Start()
    {
        eventManager = GameObject.FindObjectOfType<RandomEventManager>();
    }
    
    public override bool CheckCompleteMilestone()
    {
        if (!ranEvent)
        {
            eventManager.RunEvent(2);
            ranEvent = true;
        }
        
        GameObject[] allHouses = GameObject.FindGameObjectsWithTag("house");

        int houses = 0;
        
        foreach (var house in allHouses)
        {
            // If the house is powered
                if (house.GetComponent<GeneralObjectScript>().isMilestoneCounted &&
                    house.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
                {
                    // Add to the list of previously powered houses
                    ++houses;
                }
        }

        return houses >= 25;
    }
}
