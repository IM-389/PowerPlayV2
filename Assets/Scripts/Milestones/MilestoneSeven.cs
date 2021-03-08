using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class MilestoneSeven : MilestoneBase
{
    private GameObject[] houses;
    
    public override bool CheckCompleteMilestone()
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

        return (smartHouses >= 10);
    }
}
