using System.Collections;
using System.Collections.Generic;
using Milestones;
using UnityEngine;

public class BranchingOut : MilestoneBase
{
    public override bool CheckCompleteMilestone()
    {
        GameObject[] substations = GameObject.FindGameObjectsWithTag("Substation");
        
        // Find all houses
        GameObject[] houses = GameObject.FindGameObjectsWithTag("house");

        int poweredHouses = 0;

        // Loop through all houses
        foreach (var house in houses)
        {
            // Reference to the power storage script
            StorageScript storage = house.GetComponent<StorageScript>();
            GeneralObjectScript gos = house.GetComponent<GeneralObjectScript>();
            if (gos.isMilestoneCounted && storage.powerStored > 0)
            {
                ++poweredHouses;
            }
        }
        
        return substations.Length >= 2 && poweredHouses >= 10;
    }
}
