using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class MilestoneSeven : MilestoneBase
{
    private GameObject[] houses;
    
    public override bool CompleteMilestone()
    {
        houses = GameObject.FindGameObjectsWithTag("house");

        int smartHouses = 0;
        
        foreach (var house in houses)
        {
            if (house.GetComponent<HoverScript>().isSmart)
            {
                ++smartHouses;
            }
        }
            
        if (smartHouses >= 5)
        {
            return true;
        }

        return false;
    }

    public override void SetMilestoneProperties()
    {
        sequenceNumber = 7;
        milestoneName = "Building the Smart Grid";
        milestoneText = "Put down 5 Smart Meters";
    }
}
